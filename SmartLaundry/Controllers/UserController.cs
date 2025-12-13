using Microsoft.AspNetCore.Mvc;
using SmartLaundry.DTO;
using SmartLaundry.Model;
using Machine = SmartLaundry.Entities.Machine;
using User = SmartLaundry.Entities.User;

namespace SmartLaundry.Controllers;

public class UserController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GetMachineDto>>> GetMachines()
    {
        // get machines from database
        var machines = new List<Machine>
        {
        };

        machines.ForEach(UpdateMachine);
        // save changes to db

        var result = machines.Select(e => e.ToGetMachineDto()).ToList();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> NotifyMe()
    {
        // getting user 
        var user = new User() { UserEmail = "fdasfd"};

        user.NeedNotify = true;
        // save to db
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> ReserveMachine(int machineId)
    {
        // getting machine by id
        var machine = new Machine
        {
            Id = machineId,
            MachineState = MachineState.Available,
            CurrentUserEmail = string.Empty
        };

        if (machine.MachineState != MachineState.Available)
        {
            return BadRequest("Machine is not available for reservation.");
        }

        if (machine.CurrentUserEmail != string.Empty)
        {
            return BadRequest("Machine is already reserved by another user.");
        }

        machine.MachineState = MachineState.Reserved;
        machine.CurrentUserEmail = "currentUser";
        machine.ExpirationTime = DateTime.Now.AddMinutes(3);

        // save to db
        return Ok();
    }

    private void UpdateMachine(Machine machine)
    {
        if (machine.MachineState == MachineState.Reserved && machine.ExpirationTime <= DateTime.Now)
        {
            machine.MachineState = MachineState.Available;
            machine.CurrentUserEmail = string.Empty;
            machine.ExpirationTime = null;
        }
    }
}