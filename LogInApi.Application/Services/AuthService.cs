using LogInApi.Application.DTOs;
using LogInApi.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogInApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LogInApi.Application.Services;

public class AuthService : IAuthService
{
private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task CreateUserAsync(RegisterUserDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Usuário já cadastrado!");
        
        var user = new ApplicationUser
        {
            UserName = dto.Name,
            Email = dto.Email,
            EmailConfirmed = true,
            PhoneNumber = dto.Phone
        };
        
        var result = await _userManager.CreateAsync(user, dto.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ",
                result.Errors.Select(e => e.Description));

            throw new ApplicationException(errors);
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos");

        var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!validPassword)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos");

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        var token = GenerateJwt(user, roles, claims);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<int>("Jwt:ExpiresInMinutes")
            )
        };
    }

    private string GenerateJwt(
        ApplicationUser user,
        IList<string> roles,
        IList<Claim> claims)
    {
        var jwtClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        jwtClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        jwtClaims.AddRange(claims);

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: jwtClaims,
            expires: DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<int>("Jwt:ExpiresInMinutes")
            ),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}