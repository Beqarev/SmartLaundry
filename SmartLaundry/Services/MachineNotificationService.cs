using Microsoft.AspNetCore.SignalR;

namespace SmartLaundry.Services
{
    public interface IMachineNotificationService
    {
        Task NotifyMachineAvailable(int machineId);
    }
    public class MachineNotificationService : IMachineNotificationService
    {
        private readonly IHubContext<MachineNotificationHub> _hubContext;
        private readonly DataContext _context;

        public MachineNotificationService(
            IHubContext<MachineNotificationHub> hubContext,
            DataContext userRepository)
        {
            _hubContext = hubContext;
            _context = userRepository;
        }

        public async Task NotifyMachineAvailable(int machineId)
        {
            // Get all users who need notification
            var usersToNotify = _context.Users
                .Where(u => u.NeedNotify)
                .ToList();

            if (usersToNotify.Any())
            {
                // Send notification to all users
                await _hubContext.Clients.All.SendAsync(
                    "MachineAvailable",
                    new
                    {
                        MachineId = machineId,
                        Timestamp = DateTime.UtcNow
                    });

                // Update NeedNotify to false for all users
                usersToNotify.ForEach(u => u.NeedNotify = false);
                await _context.SaveChangesAsync();
            }
        }
    }
}
