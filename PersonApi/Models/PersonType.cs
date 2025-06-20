namespace PersonApi.Models;

public class PersonType
{
    public int Id { get; set; }

    public string PersonTypeDescription { get; set; } = string.Empty;

    // Navigation property (optional but useful for EF)
    public ICollection<Person>? Persons { get; set; }
}