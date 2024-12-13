using AutoMapper;
using eCommerceApp.Application.Abstractions;
using eCommerceApp.Application.Dtos.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using eCommerceApp.Application.Services.Interfaces.Logging;
using eCommerceApp.Application.Validations;
using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using FluentValidation;

namespace eCommerceApp.Application.Services.Implementations.Authentication;

public class AuthenticationService(
    IUserManagement userManagement,
    ITokenManagement tokenManagement,
    IRoleManagement roleManagement,
    IAppLogger<AuthenticationService> logger,
    IMapper mapper,
    IValidator<CreateUser> createUserValidator,
    IValidator<LoginUser> loginUserValidator,
    IValidationService validationService) : IAuthenticationService
{
    public async Task<Result> CreateUserAsync(CreateUser user)
    {
        /*var validationResult = await validationService.ValidateAsync(user, createUserValidator);
        if (!validationResult.IsSuccess)
            return validationResult;*/

        var mappedUser = mapper.Map<AppUser>(user);
        mappedUser.UserName = user.Email;
        mappedUser.PasswordHash = user.Password;

        var result = await userManagement.CreateUser(mappedUser);
        if (!result)
            return Result.Failure(new Error("User Creation Failed", "User Creation Failed", 400));

        var _user = await userManagement.GetUserByEmail(user.Email);
        var users = await userManagement.GetUsers();
        var assignRoleResult = await roleManagement.AddUserToRole(_user!, users.Count() > 1 ? "User" : "Admin");

        if (!assignRoleResult)
        {
            var removeUserResult = await userManagement.RemoveUserByEmail(user.Email);
            if (!removeUserResult)
            {
                logger.LogError(new Exception("User Creation Failed"), "User Creation Failed");
                return Result.Failure(new Error("User Creation Failed", "User Creation Failed", 400));
            }
        }

        return Result.Success();
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginUser user)
    {
        var mappedModel = mapper.Map<AppUser>(user);
        mappedModel.PasswordHash = user.Password;

        var loginResult = await userManagement.LoginUser(mappedModel);
        if (!loginResult)
            return Result.Failure<LoginResponse>(new Error("Login Failed", "Email not found or invalid credentials",
                400));

        var _user = await userManagement.GetUserByEmail(user.Email);
        var claims = await userManagement.GetUserClaims(_user!.Email!);

        var jwtToken = tokenManagement.GenerateToken(claims);
        var refreshToken = tokenManagement.GetRefreshToken();

        var saveTokenResult = 0;

        var userTokenCheck = await tokenManagement.ValidateRefreshToken(refreshToken);
        if (userTokenCheck)
            await tokenManagement.UpdateRefreshToken(_user.Id, refreshToken);
        else
            saveTokenResult = await tokenManagement.AddRefreshToken(_user.Id, refreshToken);

        return saveTokenResult <= 0
            ? Result.Failure<LoginResponse>(new Error("Token Generation Failed", "Token Generation Failed", 400))
            : Result.Success(new LoginResponse(jwtToken, refreshToken));
    }

    public async Task<Result<LoginResponse>> ReviveTokenAsync(string refreshToken)
    {
        var isValidRefreshToken = await tokenManagement.ValidateRefreshToken(refreshToken);
        if (!isValidRefreshToken)
            return Result.Failure<LoginResponse>(new Error("InvalidToken", "Invalid Token", 400));

        var userId = await tokenManagement.GetUserIdByRefreshToken(refreshToken);
        var user = await userManagement.GetUserById(userId);

        var claims = await userManagement.GetUserClaims(user!.Email!);
        var newJwtToken = tokenManagement.GenerateToken(claims);
        var newRefreshToken = tokenManagement.GetRefreshToken();
        await tokenManagement.UpdateRefreshToken(user.Id, newRefreshToken);

        return Result.Success(new LoginResponse(newJwtToken, refreshToken));
    }
}
