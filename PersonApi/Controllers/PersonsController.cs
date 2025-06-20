using Microsoft.AspNetCore.Mvc;
using PersonApi.Models;


namespace PersonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{


    private static readonly List<Person> Persons = new List<Person>
    {
        new Person { Id = 1, PersonName = "John Doe", PersonAge = 30, PersonTypeId = 1 },
        new Person { Id = 2, PersonName = "Jane Smith", PersonAge = 25, PersonTypeId = 2 },
    };

    // GET: api/persons
    [HttpGet]
    public ActionResult<List<Person>> GetAll()
    {
        return Ok(Persons);
    }

    [HttpGet("{id}")]
    public ActionResult<Person> GetById(int id)
    {
        var person = Persons.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    [HttpPost]
    public ActionResult<Person> Create(Person person)
    {
        person.Id = Persons.Max(p => p.Id) + 1;
        Persons.Add(person);
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Person updatedPerson)
    {
        var person = Persons.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();

        person.PersonName = updatedPerson.PersonName;
        person.PersonType = updatedPerson.PersonType;
        person.PersonAge = updatedPerson.PersonAge;
        person.PersonTypeId = updatedPerson.PersonTypeId;

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var person = Persons.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();

        Persons.Remove(person);
        return NoContent();
    }
}
