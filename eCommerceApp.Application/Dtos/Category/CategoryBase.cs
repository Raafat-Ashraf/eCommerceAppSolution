using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.Dtos.Category;

public class CategoryBase
{
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
    public string Name { get; set; } = null!;
}
