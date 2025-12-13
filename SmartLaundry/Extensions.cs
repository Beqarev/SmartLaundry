using SmartLaundry.DTO;

namespace SmartLaundry
{
    public static class Extensions
    {
        public static GetMachineDto ToGetMachineDto(this Entities.Machine machine)
        {
            return new GetMachineDto
            {
                MachineId = machine.Id,
                ExpirationTime = machine.ExpirationTime,
                MachineState = machine.MachineState
            };
        }
    }
}
