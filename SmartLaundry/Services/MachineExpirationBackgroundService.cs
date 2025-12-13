using SmartLaundry.Model;

namespace SmartLaundry.Services
{
    public class MachineExpirationBackgroundService : BackgroundService
    {
        private readonly ILogger<MachineExpirationBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Check every 1 minute

        public MachineExpirationBackgroundService(
            ILogger<MachineExpirationBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Machine Expiration Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckExpiredMachines();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking expired machines.");
                }

                // Wait before next check
                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("Machine Expiration Background Service is stopping.");
        }

        private async Task CheckExpiredMachines()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var notificationService = scope.ServiceProvider.GetRequiredService<IMachineNotificationService>();

                var now = DateTime.Now;

                // Find machines that have expired but haven't been marked as expired
                var expiredMachines = dbContext.Machines
                    .Where(m => m.ExpirationTime.HasValue &&
                               m.ExpirationTime.Value <= now )
                    .ToList();

                if (expiredMachines.Any())
                {
                    _logger.LogInformation($"Found {expiredMachines.Count} expired machines.");

                    foreach (var machine in expiredMachines)
                    {
                        // Mark as expired
                        machine.MachineState = MachineState.Available;
                        machine.ExpirationTime = null;

                        _logger.LogInformation($"Machine {machine.Id} has expired and is now available.");

                        // Send notification
                        await notificationService.NotifyMachineAvailable(machine.Id);
                    }

                    // Save changes
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
