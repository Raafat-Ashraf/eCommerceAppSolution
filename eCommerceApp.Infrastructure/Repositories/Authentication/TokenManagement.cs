using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class TokenManagement(
    ApplicationDbContext dbContext,
    IConfiguration configuration,
    UserManager<AppUser> userManager) : ITokenManagement
{
    public async Task<int> AddRefreshToken(string userId, string refreshToken)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return 0;

        await dbContext.RefreshTokens.AddAsync(new RefreshToken { UserId = userId, Token = refreshToken });
        return await dbContext.SaveChangesAsync();

        /*user.RefreshTokens.Add(new RefreshToken { UserId = userId, Token = refreshToken });
        await userManager.UpdateAsync(user);*/

        return 1;
    }

    public string GenerateToken(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: cred
        );


        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public string GetRefreshToken()
    {
        const int byteSize = 64;
        var randomBytes = new byte[byteSize];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        string token = Convert.ToBase64String(randomBytes);
        var x = WebUtility.UrlEncode(token);
        return x;
    }


    public List<Claim> GetUserClaimsFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        return jwtToken != null ? jwtToken.Claims.ToList() : [];
    }

    public async Task<bool> ValidateRefreshToken(string refreshToken)
    {
        var user = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

        return user is not null;
    }

    public async Task<string> GetUserIdByRefreshToken(string refreshToken)
        => (await dbContext.RefreshTokens.FirstOrDefaultAsync(_ => _.Token == refreshToken))?.UserId;

    public async Task<int> UpdateRefreshToken(string userId, string refreshToken)
    {
        var tokens = await dbContext.RefreshTokens.ToListAsync();
        var user = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        if (user is null)
            return -1;

        user.Token = refreshToken;
        return await dbContext.SaveChangesAsync();
    }
}
