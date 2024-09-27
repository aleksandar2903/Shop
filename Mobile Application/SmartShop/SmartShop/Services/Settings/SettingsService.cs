using SmartShop.Services.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsService))]
namespace SmartShop.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        #region Setting Constants

        private const string AccessToken = "access_token";
        private readonly string AccessTokenDefault = string.Empty;
        #endregion

        #region Settings Properties

        public string AuthAccessToken
        {
            get => Preferences.Get(AccessToken, AccessTokenDefault);
            set => Preferences.Set(AccessToken, value);
        }

        #endregion
    }
}
