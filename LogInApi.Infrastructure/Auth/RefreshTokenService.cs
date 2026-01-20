using LogInApi.Domain.Entities;
using LogInApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogInApi.Infrastructure.Auth;

public class RefreshTokenService 
{
    /*private readonly AppDbContext _context;

    public RefreshTokenService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }*/
}