using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.DTOs.Auth;
using SchoolManagementSystem.Application.Interfaces;
using SchoolManagementSystem.Application.Responses.Auth;
using SchoolManagementSystem.Domain.Context;
using SchoolManagementSystem.Domain.Interfaces;
using SchoolManagementSystem.Domain.Models;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace SchoolManagementSystem.Infrastructure.Services
{
    public class AuthService(ApplicationDbContext dbContext, ITokenService tokenService, IMapper mapper) : IAuthService
    {
        
        public async Task<AuthResponse> RegisterAsync(RegisterRequestDTO request)
        {
            if (await dbContext.Users.AnyAsync(x => x.Email == request.Email))
                throw new KeyNotFoundException("Email already exists");

            CreatePasswordHash(request.Password, out var hash, out var salt);

            var user = mapper.Map<User>(request);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return await GenerateTokensAsync(user); // Need to replace with email confirmation 
        }

        public async Task<AuthResponse> LoginAsync(LoginRequestDTO request)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
            if (user == null) throw new InvalidCredentialException("Invalid credentials");

            if (!VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new InvalidCredentialException("Invalid credentials");

            return await GenerateTokensAsync(user);
        }

        private async Task<AuthResponse> GenerateTokensAsync(User user)
        {
            var accessToken = tokenService.CreateJwtToken(user);

            var existingToken = await dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            RefreshToken refreshToken;
            if (existingToken != null && existingToken.Expires > DateTime.UtcNow)
            {
                refreshToken = existingToken;
                refreshToken.Expires = existingToken.Expires;
            }
            else
            {
                refreshToken = tokenService.CreateRefreshToken();

                refreshToken.UserId = user.Id;

                await dbContext.RefreshTokens.AddAsync(refreshToken);
                await dbContext.SaveChangesAsync();
            }
            return mapper.Map<AuthResponse>((user, accessToken, refreshToken.Token, refreshToken.Expires.ToString("o")));
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {
            var storedToken = await dbContext.RefreshTokens
                .Include(r => r.User)
                .SingleOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (storedToken == null || !storedToken.IsActive)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (storedToken != null && storedToken.Expires < DateTime.UtcNow)
            {
                storedToken.Revoked = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
            }
            return await GenerateTokensAsync(storedToken!.User);
        }

        #region Password Helpers
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }
        #endregion
    }
}