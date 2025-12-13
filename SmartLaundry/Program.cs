using Microsoft.EntityFrameworkCore;
using SmartLaundry;
using SmartLaundry.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSignalR();

// Register services
builder.Services.AddScoped<IMachineNotificationService, MachineNotificationService>();
builder.Services.AddHostedService<MachineExpirationBackgroundService>();

var app = builder.Build();

// Map SignalR hub

app.MapHub<MachineNotificationHub>("/machineNotificationHub");

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();