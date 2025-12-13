using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartLaundry.DTO;
using SmartLaundry.Model;
using System.Reflection.PortableExecutable;
using Machine = SmartLaundry.Entities.Machine;


namespace SmartLaundry.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MachineController : ControllerBase
{
    private readonly DataContext _context;

    public MachineController(DataContext context)
    {
        _context = context;
    }


    [HttpPut("update-machine-state")]
    public async Task<IActionResult> UpdateState([FromBody] DetectionMachineDto machineDto)
    {
        // getting machine by id 
        var currentMachine = _context.Machines.Find(machineDto.Id);

        if (currentMachine.MachineState == machineDto.MachineState)
        {
            return Ok();
        }

        if (machineDto.MachineState == MachineState.Running)
        {
            currentMachine.MachineState = MachineState.Running;
        }
        else if (machineDto.MachineState == MachineState.Available)
        {
            currentMachine.MachineState = MachineState.Reserved;
            currentMachine.ExpirationTime = DateTime.UtcNow.AddMinutes(3);
            currentMachine.UserEmail = null;
        }
        await _context.SaveChangesAsync();

        // save changes to database

        return Ok();
    }

}