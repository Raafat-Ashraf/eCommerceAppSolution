using System.Security.Claims;
using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class UserManagement(
    IRoleManagement roleManagement,
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext) : IUserManagement
{
    public async Task<bool> CreateUser(AppUser user)
    {
        if (await userManager.FindByEmailAsync(user.Email!) is not null)
            return false;
        var result = await userManager.CreateAsync(user, user.PasswordHash!);

        return result.Succeeded;
    }


    public async Task<bool> LoginUser(AppUser user)
    {
        if (await userManager.FindByEmailAsync(user.Email!) is not{} userFound)
            return false;

        var roleName = await roleManagement.GetUserRole(userFound.Email!);
        if (string.IsNullOrEmpty(roleName))
            return false;

        var x = await userManager.CheckPasswordAsync(userFound, user.PasswordHash!);
        return x;
    }


    public async Task<AppUser?> GetUserByEmail(string email)
        => await userManager.FindByEmailAsync(email);


    public async Task<AppUser?> GetUserById(string id)
        => await userManager.FindByIdAsync(id);


    public async Task<IEnumerable<AppUser>> GetUsers()
        => await dbContext.Users.ToListAsync();


    public async Task<bool> RemoveUserByEmail(string email)
    {
        if (await userManager.FindByEmailAsync(email) is not { } user)
            return false;

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }


    public async Task<List<Claim>> GetUserClaims(string email)
    {
        if (await userManager.FindByEmailAsync(email) is not { } user)
            return [];

        string? roleName = await roleManagement.GetUserRole(user.Email!);

        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, roleName!)
        ];

        return claims;
    }
}
