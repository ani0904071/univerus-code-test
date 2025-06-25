using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Models;
using PersonApi.Services;


namespace PersonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{

    private readonly IPersonService _personService;

    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }

    // GET: api/persons
    [HttpGet]
    public async Task<ActionResult<List<Person>>> GetAll()
    {
        return Ok(await _personService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Person>> GetById(int id)
    {
        var person = await _personService.GetByIdAsync(id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<Person>> Create(Person newPerson)
    {   
        if (newPerson == null)
            return BadRequest("Person data is required.");
            
        var created = await _personService.CreateAsync(newPerson);
        if (created == null)
            return BadRequest("Invalid PersonTypeId.");

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Person updatedPerson)
    {
        if (id != updatedPerson.Id)
        {
            return BadRequest("Person ID mismatch.");
        }
        if (updatedPerson == null)
        {
            return BadRequest("Person data is required.");
        }
        var success = await _personService.UpdateAsync(id, updatedPerson);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid person ID.");
        }

        if (!await _personService.DeleteAsync(id))
            return NotFound();

        return NoContent();
    }
}
