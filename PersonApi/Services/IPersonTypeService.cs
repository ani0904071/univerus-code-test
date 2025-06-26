using PersonApi.Models;

namespace PersonApi.Services;

public interface IPersonTypeService
{
    Task<List<PersonType>> GetAllAsync();
    Task<PersonType> GetByIdAsync(int id);
    Task<PersonType> CreateAsync(PersonType newPersonType);
}

