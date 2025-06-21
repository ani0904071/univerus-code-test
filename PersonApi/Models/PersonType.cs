namespace PersonApi.Models;
using System.Text.Json.Serialization;

public class PersonType
{
    public int Id { get; set; }

    public string PersonTypeDescription { get; set; } = string.Empty;

    // Navigation property (optional but useful for EF)
    [JsonIgnore] 
    public ICollection<Person>? Persons { get; set; }
}