namespace PersonApi.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class PersonType
{   
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "Description must be under 30 characters.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Description can only contain letters, and spaces.")]
    public string Description { get; set; } = string.Empty;

    // Navigation property (optional but useful for EF)
    [JsonIgnore]
    public ICollection<Person>? Persons { get; set; }
}