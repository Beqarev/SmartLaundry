namespace SmartLaundry.Model;

public class Machine
{
    public int Id { get; set; }
    public required string MachineState { get; set; }
    public DateTime ExpirationTime { get; set; }
    public required string CurrentUserEmail { get; set; }
}