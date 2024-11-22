using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public int Quantity { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
