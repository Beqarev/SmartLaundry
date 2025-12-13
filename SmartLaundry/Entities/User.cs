namespace SmartLaundry.Entities;

public class User
{
    public int Id { get; set; }
    public required string UserEmail { get; set; }
    public bool NeedNotify { get; set; }
}