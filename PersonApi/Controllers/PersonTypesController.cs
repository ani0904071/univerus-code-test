using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonApi.Data;
using PersonApi.Models;


namespace PersonApi.Controllers;

using Microsoft.EntityFrameworkCore;
using PersonApi.Services;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PersonTypesController : ControllerBase
{

    private readonly IPersonTypeService _personTypesService;

    public PersonTypesController(IPersonTypeService personTypesService)
    {
        _personTypesService = personTypesService;
    }

    // GET: api/persons
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<List<PersonType>>> GetAllV1()
    {
        return Ok(await _personTypesService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<PersonType>> GetByIdV1(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid person type ID.");
        }

        PersonType personType;
        try
        {
            personType = await _personTypesService.GetByIdAsync(id);
            if (personType == null) return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok(personType);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<PersonType>> CreateV1(PersonType newPersonType)
    {
        if (newPersonType == null)
        {
            return BadRequest("PersonType data is required.");
        }

        try
        {
            var created = await _personTypesService.CreateAsync(newPersonType);
            return CreatedAtAction(nameof(GetByIdV1), new { id = created.Id }, created);
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
