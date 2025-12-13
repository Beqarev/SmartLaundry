using Microsoft.AspNetCore.SignalR;

namespace SmartLaundry.Services
{
    public interface IMachineNotificationService
    {
        Task NotifyMachineAvailable(string machineId, string machineName);
    }
    public class MachineNotificationService
    {
        private readonly IHubContext<MachineNotificationHub> _hubContext;
        private readonly IUserRepository _userRepository; // Your user repository

        public MachineNotificationService(
            IHubContext<MachineNotificationHub> hubContext,
            IUserRepository userRepository)
        {
            _hubContext = hubContext;
            _userRepository = userRepository;
        }

        public async Task NotifyMachineAvailable(string machineId, string machineName)
        {
            // Get all users who need notification
            var usersToNotify = await _userRepository.GetUsersWithNotificationAsync();

            if (usersToNotify.Any())
            {
                // Send notification to all users
                await _hubContext.Clients.All.SendAsync(
                    "MachineAvailable",
                    new
                    {
                        MachineId = machineId,
                        MachineName = machineName,
                        Timestamp = DateTime.UtcNow
                    });

                // Update NeedNotify to false for all users
                await _userRepository.ResetNotificationFlagsAsync(usersToNotify.Select(u => u.Id));
            }
        }
    }
}
