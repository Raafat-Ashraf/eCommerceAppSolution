using System.Reflection;
using eCommerceApp.Application.Mapping;
using eCommerceApp.Application.Services.Implementations;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationService = eCommerceApp.Application.Services.Implementations.Authentication.AuthenticationService;
using IAuthenticationService = eCommerceApp.Application.Services.Interfaces.Authentication.IAuthenticationService;

namespace eCommerceApp.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        // Add services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();


        // Add AutoMapper
        services.AddAutoMapper(typeof(MappingConfig));


        // Add fluent validation
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

        services.AddScoped<IValidationService, ValidationService>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
