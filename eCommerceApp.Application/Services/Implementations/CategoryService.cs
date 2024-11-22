using AutoMapper;
using eCommerceApp.Application.Abstractions;
using eCommerceApp.Application.Dtos.Category;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace eCommerceApp.Application.Services.Implementations;

public class CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper) : ICategoryService
{
    public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        return mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<Result<CategoryResponse>> GetByIdAsync(Guid id)
    {
        if (await categoryRepository.GetByIdAsync(id) is not { } category)
            return Result.Failure<CategoryResponse>(new Error("Category.NotFound",
                "No Category was found with the given ID", StatusCodes.Status404NotFound));

        var categoryMapped = mapper.Map<CategoryResponse>(category);

        return Result.Success(categoryMapped);
    }

    public async Task<Result> CreateAsync(CreateCategory category)
    {
        var categoryEntity = mapper.Map<Category>(category);
        await categoryRepository.AddAsync(categoryEntity);

        return Result.Success("Category created successfully");
    }

    public async Task<Result> UpdateAsync(UpdateCategory request)
    {
        if (await categoryRepository.GetByIdAsync(request.Id) is not { } category)
            return Result.Failure<CategoryResponse>(new Error("Category.NotFound",
                "No Category was found with the given ID", StatusCodes.Status404NotFound));

        mapper.Map(category, category);
        await categoryRepository.UpdateAsync(category);

        return Result.Success("Category updated successfully");
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        if (await categoryRepository.GetByIdAsync(id) is not { } category)
            return Result.Failure<CategoryResponse>(new Error("Category.NotFound",
                "No Category was found with the given ID", StatusCodes.Status404NotFound));

        await categoryRepository.DeleteAsync(category);

        return Result.Success("Category deleted successfully");
    }
}
