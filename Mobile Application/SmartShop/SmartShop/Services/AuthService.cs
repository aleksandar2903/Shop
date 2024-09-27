using Newtonsoft.Json;
using SmartShop.Models;
using SmartShop.Models.Request;
using SmartShop.Services;
using SmartShop.Services.RequestProvider;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthService))]
namespace SmartShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly string url = $"{ Config.APIUrl }";
        public AuthService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }
        public async Task<AuthResponse> LogIn(LoginRequest request)
        {
            var token = await _requestProvider.PostAsync<LoginRequest, AuthResponse>($"{url}/login", request).ConfigureAwait(false);

            return token;
        }
        public async Task<User> GetUserAsync(string token)
        {
            var user = await _requestProvider.GetAsync<User>($"{url}/user", token).ConfigureAwait(false);

            return user ?? new User();
        }

        public Task<bool> LogOut()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            var token = await _requestProvider.PostAsync<RegisterRequest, AuthResponse>($"{url}/register", request).ConfigureAwait(false);

            return token;
        }
    }
}
