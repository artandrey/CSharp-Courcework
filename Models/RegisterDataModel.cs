using System.ComponentModel.DataAnnotations;
namespace Validation.Models;
public class RegisterDataModel
{
    [Required]
    [StringLength(maximumLength: 20, MinimumLength = 2)]
    public string? Name { get; set; }



    [Required]
    [EmailAddress]
    public string? Email { get; set; }


    [Required]
    [StringLength(maximumLength: 20, MinimumLength = 8)]
    public string? Password { get; set; }
}