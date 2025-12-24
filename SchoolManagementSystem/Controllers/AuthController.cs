using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.DTOs.Auth;
using SchoolManagementSystem.Application.Interfaces;
using SchoolManagementSystem.Infrastructure.Constants;
using System.Security.Authentication;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, IValidator<RegisterRequestDTO> createValidator) : ControllerBase
    {
        //[Authorize(Roles = $"{UserRoles.Admin}")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            var validationResult = await createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            try
            {
                return Ok(await authService.RegisterAsync(request));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            try
            {
                return Ok(await authService.LoginAsync(request));
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDTO request)
        {
            try
            {
                return Ok(await authService.RefreshTokenAsync(request));
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }    
}
