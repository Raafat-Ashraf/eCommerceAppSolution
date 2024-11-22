using eCommerceApp.Application.Dtos.Product;

namespace eCommerceApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await productService.GetAllAsync();

        return Ok(products);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await productService.GetByIdAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProduct product)
    {
        var result = await productService.CreateAsync(product);

        return result.IsSuccess ? Created() : result.ToProblem();
    }


    [HttpPut]
    public async Task<IActionResult> Update(UpdateProduct product)
    {
        var result = await productService.UpdateAsync(product);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await productService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
