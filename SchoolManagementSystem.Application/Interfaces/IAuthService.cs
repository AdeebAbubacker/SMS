using SchoolManagementSystem.Application.DTOs.Auth;
using SchoolManagementSystem.Application.Responses.Auth;

namespace SchoolManagementSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequestDTO request);
        Task<AuthResponse> LoginAsync(LoginRequestDTO request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequestDTO request);
    }
}