using eCommerceApp.Application.Dtos.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;

namespace eCommerceApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUser user)
    {
        var result = await authenticationService.CreateUserAsync(user);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUser user)
    {
        var result = await authenticationService.LoginAsync(user);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("refresh-token/{refreshToken}")]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        var result = await authenticationService.ReviveTokenAsync(refreshToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
