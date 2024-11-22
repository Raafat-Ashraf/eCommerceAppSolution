using eCommerceApp.Application.Abstractions;
using eCommerceApp.Application.Dtos;
using eCommerceApp.Application.Dtos.Category;

namespace eCommerceApp.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllAsync();
    Task<Result<CategoryResponse>> GetByIdAsync(Guid id);

    Task<Result> CreateAsync(CreateCategory Category);
    Task<Result> UpdateAsync(UpdateCategory Category);
    Task<Result> DeleteAsync(Guid id);
}
