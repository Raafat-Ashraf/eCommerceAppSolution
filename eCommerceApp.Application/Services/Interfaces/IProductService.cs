using eCommerceApp.Application.Abstractions;
using eCommerceApp.Application.Dtos;
using eCommerceApp.Application.Dtos.Product;

namespace eCommerceApp.Application.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<Result<ProductResponse>> GetByIdAsync(Guid id);

    Task<Result> CreateAsync(CreateProduct product);
    Task<Result> UpdateAsync(UpdateProduct product);
    Task<Result> DeleteAsync(Guid id);
}
