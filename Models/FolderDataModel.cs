using System.ComponentModel.DataAnnotations;
namespace Validation.Models;
public class FolderDataModel
{
    [Required]
    [StringLength(maximumLength: 50)]
    public string Name { get; set; } = null!;
}