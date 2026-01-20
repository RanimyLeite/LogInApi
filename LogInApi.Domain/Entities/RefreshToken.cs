using System.ComponentModel.DataAnnotations;

namespace LogInApi.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool Revoked { get; set; }

    // FK
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}