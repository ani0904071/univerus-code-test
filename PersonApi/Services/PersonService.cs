using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Models;

namespace PersonApi.Services;

public class PersonService : IPersonService
{

    private readonly PersonAPIContext _context;

    public PersonService(PersonAPIContext context)
    {
        _context = context;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _context.Persons
            .OrderBy(p => p.Age)
            .Include(p => p.PersonType)
            .ToListAsync();
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        var person = await _context.Persons
            .Include(p => p.PersonType)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
            throw new InvalidOperationException($"Person with Id {id} not found.");

        return person;
    }

    // Version 2 method to get person by Id without PersonType
    public async Task<Person> GetByIdAsyncV2(int id)
    {
        var person = await _context.Persons
            //.Include(p => p.PersonType)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
            throw new InvalidOperationException($"Person with Id {id} not found.");

        return person;
    }

    public async Task<Person> CreateAsync(Person newPerson)
    {
        var personTypeExists = await _context.PersonTypes
            .AnyAsync(pt => pt.Id == newPerson.PersonTypeId);

        if (!personTypeExists)
            throw new InvalidOperationException($"Invalid PersonTypeId: {newPerson.PersonTypeId} does not exist.");

        _context.Persons.Add(newPerson);
        await _context.SaveChangesAsync();

        var createdPerson = await _context.Persons
            .Include(p => p.PersonType)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == newPerson.Id);

        if (createdPerson == null)
            throw new InvalidOperationException("Failed to create the person.");

        return createdPerson;
    }

    public async Task<bool> UpdateAsync(int id, Person updatedPerson)
    {
        if (id != updatedPerson.Id)
            return false;

        var person = await _context.Persons.FindAsync(id);
        if (person == null)
            return false;

        // Validate PersonTypeId
        var personTypeExists = await _context.PersonTypes.AnyAsync(pt => pt.Id == updatedPerson.PersonTypeId);
        if (!personTypeExists)
            throw new InvalidOperationException($"Invalid PersonTypeId: {updatedPerson.PersonTypeId} does not exist.");

        person.Name = updatedPerson.Name;
        person.Age = updatedPerson.Age;
        person.PersonTypeId = updatedPerson.PersonTypeId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return false;

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }


}