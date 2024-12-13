using eCommerceApp.Application.Dtos.Category;
using Microsoft.AspNetCore.Authorization;

namespace eCommerceApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await categoryService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await categoryService.GetByIdAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateCategory product)
    {
        var result = await categoryService.CreateAsync(product);

        return result.IsSuccess ? Created() : result.ToProblem();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCategory product)
    {
        var result = await categoryService.UpdateAsync(product);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await categoryService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
