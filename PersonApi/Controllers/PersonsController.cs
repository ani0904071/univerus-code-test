using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Models;


namespace PersonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{


    // private static readonly List<Person> Persons = new List<Person>
    // {
    //     new Person { Id = 1, PersonName = "John Doe", PersonAge = 30, PersonTypeId = 1 },
    //     new Person { Id = 2, PersonName = "Jane Smith", PersonAge = 25, PersonTypeId = 2 },
    // };

    private readonly PersonAPIContext _context;
    public PersonsController(PersonAPIContext context)
    {
        _context = context;
    }

    // GET: api/persons
    [HttpGet]
    public async Task<ActionResult<List<Person>>> GetAll()
    {
        return Ok(await _context.Persons.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetById(int id)
    {
        var person = await _context.Persons
            .FindAsync(id);
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

        _context.Persons.Add(newPerson);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = newPerson.Id }, newPerson);
    }

    // [HttpPut("{id}")]
    // public IActionResult Update(int id, Person updatedPerson)
    // {
    //     var person = Persons.FirstOrDefault(p => p.Id == id);
    //     if (person == null) return NotFound();

    //     person.PersonName = updatedPerson.PersonName;
    //     person.PersonType = updatedPerson.PersonType;
    //     person.PersonAge = updatedPerson.PersonAge;
    //     person.PersonTypeId = updatedPerson.PersonTypeId;

    //     return NoContent();
    // }
    
    // [HttpDelete("{id}")]
    // public IActionResult Delete(int id)
    // {
    //     var person = Persons.FirstOrDefault(p => p.Id == id);
    //     if (person == null) return NotFound();

    //     Persons.Remove(person);
    //     return NoContent();
    // }
}
