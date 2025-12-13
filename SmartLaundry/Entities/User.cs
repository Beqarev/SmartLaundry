using System.ComponentModel.DataAnnotations;

namespace SmartLaundry.Entities;

public class User
{
    [Key]
    public required string UserEmail { get; set; }
    public bool NeedNotify { get; set; }
    public List<Machine> Machines { get; set; }
}