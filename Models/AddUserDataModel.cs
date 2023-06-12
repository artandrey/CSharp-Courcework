using System.ComponentModel.DataAnnotations;
namespace Validation.Models;

public class AddUserDataModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}