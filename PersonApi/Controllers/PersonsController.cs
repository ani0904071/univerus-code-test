using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Models;


namespace PersonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{

    private readonly PersonAPIContext _context;
    public PersonsController(PersonAPIContext context)
    {
        _context = context;
    }

    // GET: api/persons
    [HttpGet]
    public async Task<ActionResult<List<Person>>> GetAll()
    {
        return Ok(await _context.Persons
                            .Include(p => p.PersonType)
                            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Person>> GetById(int id)
    {
        var person = await _context.Persons
                                    .Include(p => p.PersonType)
                                    .FirstOrDefaultAsync(p => p.Id == id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult<Person>> Create(Person newPerson)
    {
        if (newPerson == null)
        {
            return BadRequest("Person data is required.");
        }

        // ✅ Check if PersonTypeId exists
        bool personTypeExists = await _context.PersonTypes
            .AnyAsync(pt => pt.Id == newPerson.PersonTypeId);

        if (!personTypeExists)
        {
            return BadRequest($"Invalid PersonTypeId: {newPerson.PersonTypeId} does not exist.");
        }

        _context.Persons.Add(newPerson);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = newPerson.Id }, newPerson);
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

        // Check if the person exists
        var person = await _context.Persons.FindAsync(id);

        if (person == null)
        {
            return NotFound();
        }

        // Update the person properties
        person.Name = updatedPerson.Name;
        person.PersonType = updatedPerson.PersonType;
        person.Age = updatedPerson.Age;
        person.PersonTypeId = updatedPerson.PersonTypeId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid person ID.");
        }

        // Check if the person exists
        var person = await _context.Persons.FindAsync(id);
        if (person == null)
        {
            return NotFound();
        }


        _context.Remove(person);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
