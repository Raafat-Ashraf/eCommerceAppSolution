using System.ComponentModel.DataAnnotations;
using eCommerceApp.Application.Dtos.Category;

namespace eCommerceApp.Application.Dtos.Product;

public class ProductResponse : ProductBase
{
    [Required]
    public Guid Id { get; set; }

    public CategoryResponse Category { get; set; } = new();
}
