using SmartShop.Models;
using SmartShop.Models.Request;
using System.Threading.Tasks;

namespace SmartShop.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterRequest request);
        Task<User> GetUserAsync(string token);
        Task<AuthResponse> LogIn(LoginRequest request);
        Task<bool> LogOut();
    }
}
