namespace PersonApi.Models;

public class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public int PersonTypeId { get; set; }

    // Navigation property
    public PersonType? PersonType { get; set; }
}
