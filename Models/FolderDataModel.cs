using System.ComponentModel.DataAnnotations;
namespace Validation.Models;
public class FolderDataModel
{
    [Required]
    [StringLength(maximumLength: 256)]
    public string? Name { get; set; }
}