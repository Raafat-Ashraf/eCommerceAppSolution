namespace eCommerceApp.Application.Dtos.Product;

public class UpdateProduct : ProductBase
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
}
