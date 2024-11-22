using eCommerceApp.Application.Dtos.Product;

namespace eCommerceApp.Application.Dtos.Category;

public class CategoryResponse : CategoryBase
{
    public Guid Id { get; set; }
    public ICollection<ProductResponse> Products { get; set; } = [];
}
