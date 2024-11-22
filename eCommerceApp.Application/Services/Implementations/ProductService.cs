using AutoMapper;
using eCommerceApp.Application.Abstractions;
using eCommerceApp.Application.Dtos.Product;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace eCommerceApp.Application.Services.Implementations;

public class ProductService(
    IGenericRepository<Product> productRepository,
    IGenericRepository<Category> categoryRepository,
    IMapper mapper) : IProductService
{
    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await productRepository.GetAllAsync();

        return mapper.Map<IEnumerable<ProductResponse>>(products);
    }

    public async Task<Result<ProductResponse>> GetByIdAsync(Guid id)
    {
        if (await productRepository.GetByIdAsync(id) is not { } product)
            return Result.Failure<ProductResponse>(new Error("product.NotFound",
                "No product was found with the given ID", StatusCodes.Status404NotFound));

        var productMapped = mapper.Map<ProductResponse>(product);

        return Result.Success(productMapped);
    }

    public async Task<Result> CreateAsync(CreateProduct product)
    {
        if (await categoryRepository.GetByIdAsync(product.CategoryId) is not { } category)
            return Result.Failure<ProductResponse>(new Error("Category.NotFound",
                " No Category was found with the given ID", StatusCodes.Status404NotFound));

        var productMapped = mapper.Map<Product>(product);

        await productRepository.AddAsync(productMapped);

        return Result.Success("Product created successfully");
    }

    public async Task<Result> UpdateAsync(UpdateProduct request)
    {
        if (await categoryRepository.GetByIdAsync(request.CategoryId) is not { } category)
            return Result.Failure<ProductResponse>(new Error("Category.NotFound",
                "No Category was found with the given ID", StatusCodes.Status404NotFound));

        if (await productRepository.GetByIdAsync(request.Id) is not { } product)
            return Result.Failure<ProductResponse>(new Error("product.NotFound",
                "No product was found with the given ID", StatusCodes.Status404NotFound));

        mapper.Map(request, product);
        await productRepository.UpdateAsync(product);

        return Result.Success("product updated successfully");
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        if (await productRepository.GetByIdAsync(id) is not { } product)
            return Result.Failure<ProductResponse>(new Error("Category.NotFound",
                "No Category was found with the given ID", StatusCodes.Status404NotFound));

        await productRepository.DeleteAsync(product);

        return Result.Success("Product deleted successfully");
    }
}
