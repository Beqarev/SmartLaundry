namespace SmartLaundry.Entities;

public class User
{
    public required string UserEmail { get; set; }
    public bool NeedNotify { get; set; }
}