using System.Reflection.Metadata.Ecma335;

namespace eCommerceApp.Application.Dtos.Identity;

public record LoginResponse(
    string Token = null!,
    string RefreshToken = null!
);
