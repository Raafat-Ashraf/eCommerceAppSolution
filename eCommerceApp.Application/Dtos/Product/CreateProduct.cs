namespace eCommerceApp.Application.Dtos.Product;

public class CreateProduct : ProductBase
{
    public Guid CategoryId { get; set; }
}
