using eCommerceApp.Application.Abstractions;
using FluentValidation;

namespace eCommerceApp.Application.Validations;

public interface IValidationService
{
    Task<Result> ValidateAsync<T>(T model, IValidator<T> validator);
}

public class ValidationService : IValidationService
{
    public async Task<Result> ValidateAsync<T>(T model, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(model);

        if (validationResult.IsValid)
            return Result.Success();

        var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

        var errorMessage = string.Join(", ", errors);

        return Result.Failure(new Error("Validation Error", errorMessage, 400));
    }
}
