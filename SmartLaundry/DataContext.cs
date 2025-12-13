using Microsoft.EntityFrameworkCore;
using SmartLaundry.Model;

namespace SmartLaundry;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<Machine> Machines { get; set; }
}