using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Models;

namespace PersonApi.Services;

public class PersonTypeService : IPersonTypeService
{
    private readonly PersonAPIContext _context;

    public PersonTypeService(PersonAPIContext context)
    {
        _context = context;
    }

    public async Task<List<PersonType>> GetAllAsync()
    {
        return await _context.PersonTypes.ToListAsync();
    }

    public async Task<PersonType?> GetByIdAsync(int id)
    {
        return await _context.PersonTypes.FindAsync(id);
    }
    public async Task<PersonType> CreateAsync(PersonType newPersonType)
    {
        if (newPersonType == null)
            throw new ArgumentNullException(nameof(newPersonType), "PersonType data is required.");

        bool exists = await _context.PersonTypes
            .AnyAsync(pt => pt.Description.ToLower() == newPersonType.Description.ToLower());

        if (exists)
            throw new InvalidOperationException($"Duplicate detected: '{newPersonType.Description}' already exists.");

        _context.PersonTypes.Add(newPersonType);
        await _context.SaveChangesAsync();

        return newPersonType;
    }
}
