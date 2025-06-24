using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonApi.Models;

public class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Name must be at least 2 character long.")]
    [MaxLength(30, ErrorMessage = "Name must be under 30 characters.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(5, 60, ErrorMessage = "Age must be between 5 and 60.")]
    public int Age { get; set; }

    [Required]
    public int PersonTypeId { get; set; }

    // Navigation property
    public PersonType? PersonType { get; set; }
}
