using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.Dtos.Product;

public class ProductBase
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [DataType(DataType.Currency)] public decimal Price { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int Quantity { get; set; }
}
