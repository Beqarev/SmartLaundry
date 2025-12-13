using Microsoft.EntityFrameworkCore;
using SmartLaundry.Entities;
using SmartLaundry.Model;

namespace SmartLaundry;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<Machine> Machines { get; set; }
    public DbSet<User> Users { get; set; }
}