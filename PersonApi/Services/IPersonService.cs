using PersonApi.Models;

namespace PersonApi.Services;

public interface IPersonService
{
    Task<List<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task<Person> CreateAsync(Person newPerson);
    Task<bool> UpdateAsync(int id, Person updatedPerson);
    Task<bool> DeleteAsync(int id);

    // version 2 method without PersonType
    Task<Person> GetByIdAsyncV2(int id);
     
}

