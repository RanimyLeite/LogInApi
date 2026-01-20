namespace LogInApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;


public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}