using Microsoft.AspNetCore.SignalR;

namespace SmartLaundry.Services
{
    public class MachineNotificationHub : Hub
    {
        public async Task SendMachineAvailableNotification(string machineId)
        {
            await Clients.All.SendAsync("MachineAvailable", machineId);
        }
    }
}
