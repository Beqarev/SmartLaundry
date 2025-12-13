using System.ComponentModel.DataAnnotations;

namespace SmartLaundry.Entities;

public class Machine
{
    [Key]
    public int Id { get; set; }
    public required string MachineState { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public User User { get; set; }
}