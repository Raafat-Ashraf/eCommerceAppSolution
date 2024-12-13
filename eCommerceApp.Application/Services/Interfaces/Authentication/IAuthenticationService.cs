using eCommerceApp.Application.Abstractions;
using eCommerceApp.Application.Dtos.Identity;

namespace eCommerceApp.Application.Services.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<Result> CreateUserAsync(CreateUser user);
    Task<Result<LoginResponse>> LoginAsync(LoginUser user);
    Task<Result<LoginResponse>> ReviveTokenAsync(string refreshToken);
}
