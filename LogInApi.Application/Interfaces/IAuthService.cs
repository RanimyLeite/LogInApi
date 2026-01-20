using LogInApi.Application.DTOs;

namespace LogInApi.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    
    Task CreateUserAsync(RegisterUserDto dto);
}