
using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Domain.Interfaces
{
    public interface ITokenService
    {
        string CreateJwtToken(User user);
        RefreshToken CreateRefreshToken();
    }
}