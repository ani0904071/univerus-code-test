using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonApi.Data;
using PersonApi.Models;


namespace PersonApi.Controllers;

using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PersonTypesController : ControllerBase
{

    private readonly PersonAPIContext _context;
    public PersonTypesController(PersonAPIContext context)
    {
        _context = context;
    }

    // GET: api/persons
    [HttpGet]
    public async Task<ActionResult<List<PersonType>>> GetAll()
    {
        var result = await _context.PersonTypes
        .OrderBy(pt => pt.Id)
        .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonType>> GetById(int id)
    {
        var personType = await _context.PersonTypes.FindAsync(id);
        if (personType == null) return NotFound();
        return Ok(personType);
    }

    [HttpPost]
    public async Task<ActionResult<PersonType>> Create(PersonType newPersonType)
    {
        if (newPersonType == null)
        {
            return BadRequest("PersonType data is required.");
        }

        // Check for duplicates (case-insensitive)
        bool exists = await _context.PersonTypes
            .AnyAsync(pt => pt.Description.ToLower() == newPersonType.Description.ToLower());

        if (exists)
        {
            return Conflict($"Duplicate detected: '{newPersonType.Description}' already exists.");
        }

        _context.PersonTypes.Add(newPersonType);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = newPersonType.Id }, newPersonType);
    }


    // [HttpPut("{id:int}")]
    // public IActionResult Update(int id, PersonType updatedPersonType)
    // {
    //     var personType = PersonTypes.FirstOrDefault(p => p.Id == id);
    //     if (personType == null) return NotFound();

    //     personType.PersonTypeDescription = updatedPersonType.PersonTypeDescription;


    //     return NoContent();
    // }

    // [HttpDelete("{id:int}")]
    // public IActionResult Delete(int id)
    // {
    //     var personType = PersonTypes.FirstOrDefault(p => p.Id == id);
    //     if (personType == null) return NotFound();

    //     PersonTypes.Remove(personType);
    //     return NoContent();
    // }
}
