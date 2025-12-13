using Microsoft.AspNetCore.Mvc;
using SmartLaundry.DTO;
using SmartLaundry.Model;
using SmartLaundry.Services;
using Machine = SmartLaundry.Entities.Machine;

namespace SmartLaundry.Controllers;

public class UserController : ControllerBase
{
    private readonly DataContext _context;

    public UserController(DataContext context, IMachineNotificationService machineNotificationService)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<List<GetMachineDto>>> GetMachines()
    {
        // get machines from database
        var machines = _context.Machines.ToList();

        machines.ForEach(UpdateMachine);

        _context.SaveChanges();

        var result = machines.Select(e => e.ToGetMachineDto()).ToList();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> NotifyMe(string userEmail)
    {
        // getting user 
        var user = _context.Users.Find(userEmail);

        user.NeedNotify = true;
        
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
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

        if (machine.User.UserEmail != string.Empty ||
            machine.User.UserEmail != null)
        {
            return BadRequest("Machine is already reserved by another user.");
        }

        machine.MachineState = MachineState.Reserved;
        machine.User.UserEmail = userEmail;
        machine.ExpirationTime = DateTime.Now.AddMinutes(3);

        await _context.SaveChangesAsync();

        return Ok();
    }

    private void UpdateMachine(Machine machine)
    {
        if (machine.MachineState == MachineState.Reserved && machine.ExpirationTime <= DateTime.Now)
        {
            machine.MachineState = MachineState.Available;
            machine.User.UserEmail = null;
            machine.ExpirationTime = null;
        }
    }
}