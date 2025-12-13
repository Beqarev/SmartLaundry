using SmartLaundry.Model;

namespace SmartLaundry.DTO;

public class GetMachineDto
{
    public int MachineId { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public string MachineState { get; set; }
}