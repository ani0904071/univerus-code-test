using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Models;
using PersonApi.Services;


namespace PersonApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PersonsController : ControllerBase
{

    private readonly IPersonService _personService;

    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }

    // GET: api/persons
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<List<Person>>> GetAllV1()
    {
        return Ok(await _personService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<Person>> GetByIdV1(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid person ID.");
        }

        Person? person = null;
        try
        {
            person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok(person);
    }

    // GET: api/v2/persons MOCK
    [HttpGet("{id:int}")]
    [MapToApiVersion("2.0")]
    public async Task<ActionResult<Person>> GetByIdV2(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid person ID.");
        }

        Person? person = null;
        try
        {
            person = await _personService.GetByIdAsyncV2(id);
            if (person == null) return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok(person);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<Person>> CreateV1([FromBody] Person newPerson)
    {
        if (newPerson == null)
            return BadRequest("Person data is required.");

        var created = await _personService.CreateAsync(newPerson);
        if (created == null)
            return BadRequest("Invalid PersonTypeId.");

        return CreatedAtAction(nameof(GetByIdV1), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateV1(int id, Person updatedPerson)
    {
        if (id != updatedPerson.Id)
        {
            return BadRequest("Person ID mismatch.");
        }

        if (updatedPerson == null)
        {
            return BadRequest("Person data is required.");
        }

        try
        {
            var success = await _personService.UpdateAsync(id, updatedPerson);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteV1(int id)
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
