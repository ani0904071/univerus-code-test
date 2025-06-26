using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonApi.Data;
using PersonApi.Models;


namespace PersonApi.Controllers;

using Microsoft.EntityFrameworkCore;
using PersonApi.Services;

[ApiController]
[Route("api/[controller]")]
public class PersonTypesController : ControllerBase
{

    private readonly IPersonTypeService _personTypesService;

    public PersonTypesController(IPersonTypeService personTypesService)
    {
        _personTypesService = personTypesService;
    }

    // GET: api/persons
    [HttpGet]
    public async Task<ActionResult<List<PersonType>>> GetAll()
    {
        return Ok(await _personTypesService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonType>> GetById(int id)
    {
        var personType = await _personTypesService.GetByIdAsync(id);
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

        try
        {
            var created = await _personTypesService.CreateAsync(newPersonType);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
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
