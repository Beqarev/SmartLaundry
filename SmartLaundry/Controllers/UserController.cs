using Microsoft.AspNetCore.Mvc;
using SmartLaundry.DTO;
using SmartLaundry.Model;
using SmartLaundry.Services;
using Machine = SmartLaundry.Entities.Machine;

namespace SmartLaundry.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMachineNotificationService _machineNotificationService;

    public UserController(DataContext context, IMachineNotificationService machineNotificationService)
    {
        _context = context;
        _machineNotificationService = machineNotificationService;
    }


    [HttpGet("get-machine")]
    public async Task<ActionResult<List<GetMachineDto>>> GetMachines()
    {
        // get machines from database
        var machines = _context.Machines.ToList();

        bool needNotify = false;
        int machineId = 0;
        foreach (var machine in machines)
        {
            if (machine.MachineState == MachineState.Reserved && machine.ExpirationTime <= DateTime.UtcNow)
            {
                machine.MachineState = MachineState.Available;
                machine.UserEmail = null;
                machine.ExpirationTime = null;

                machineId = machine.Id;
                needNotify = true;
            }
        }
        if (needNotify)
        {
            await _machineNotificationService.NotifyMachineAvailable(machineId);
        }

        await _context.SaveChangesAsync();

        var result = machines.Select(e => e.ToGetMachineDto()).ToList();

        return Ok(result);
    }

    [HttpPost("notify-me")]
    public async Task<ActionResult> NotifyMe(string userEmail)
    {
        // getting user 
        var user = _context.Users.Find(userEmail);

        user.NeedNotify = true;
        
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("reserve-machine")]
    public async Task<ActionResult> ReserveMachine(int machineId, string userEmail)
    {
        var machine = _context.Machines.Find(machineId);

        if (machine == null)
        {
            return NotFound("Machine not found.");
        }

        if (machine.MachineState != MachineState.Available)
        {
            return BadRequest("Machine is not available for reservation.");
        }

        if (machine.MachineState == MachineState.Reserved)
        {
            return BadRequest("Machine is already reserved by another user.");
        }

        machine.MachineState = MachineState.Reserved;
        machine.UserEmail = userEmail;
        machine.ExpirationTime = DateTime.UtcNow.AddMinutes(3);

        await _context.SaveChangesAsync();

        return Ok();
    }
}