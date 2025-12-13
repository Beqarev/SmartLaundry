using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLaundry.Entities;

public class Machine
{
    [Key]
    public int Id { get; set; }
    public required string MachineState { get; set; }
    public DateTime? ExpirationTime { get; set; }

    [ForeignKey("UserEmail")]
    public User User { get; set; }
    public string? UserEmail { get; set; }
}