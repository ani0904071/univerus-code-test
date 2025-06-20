using Microsoft.AspNetCore.Mvc;
using PersonApi.Models;


namespace PersonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonTypesController : ControllerBase
{


    private static readonly List<PersonType> PersonTypes = new List<PersonType>
    {
        new PersonType { Id = 1, PersonTypeDescription = "Teacher" },
        new PersonType { Id = 2, PersonTypeDescription = "Student" },       
    };

    // GET: api/persons
    [HttpGet]
    public ActionResult<List<Person>> GetAll()
    {
        return Ok(PersonTypes);
    }

    [HttpGet("{id}")]
    public ActionResult<Person> GetById(int id)
    {
        var personType = PersonTypes.FirstOrDefault(p => p.Id == id);
        if (personType == null) return NotFound();
        return Ok(personType);
    }

    [HttpPost]
    public ActionResult<Person> Create(PersonType personType)
    {
        personType.Id = PersonTypes.Max(p => p.Id) + 1;
        PersonTypes.Add(personType);
        return CreatedAtAction(nameof(GetById), new { id = personType.Id }, personType);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, PersonType updatedPersonType)
    {
        var personType = PersonTypes.FirstOrDefault(p => p.Id == id);
        if (personType == null) return NotFound();

        personType.PersonTypeDescription = updatedPersonType.PersonTypeDescription;
    

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var person = PersonTypes.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();

        PersonTypes.Remove(person);
        return NoContent();
    }
}
