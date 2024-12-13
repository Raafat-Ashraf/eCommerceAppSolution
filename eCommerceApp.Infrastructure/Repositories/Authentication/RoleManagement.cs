using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class RoleManagement(UserManager<AppUser> userManager) : IRoleManagement
{
    public async Task<string?> GetUserRole(string userEmail)
    {
        if (await userManager.FindByEmailAsync(userEmail) is not { } user)
            return null;

        return (await userManager.GetRolesAsync(user)).FirstOrDefault();
    }

    public async Task<bool> AddUserToRole(AppUser user, string roleName)
        => (await userManager.AddToRoleAsync(user, roleName)).Succeeded;
}
