using Microsoft.AspNetCore.Identity;

namespace LogInApi.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}