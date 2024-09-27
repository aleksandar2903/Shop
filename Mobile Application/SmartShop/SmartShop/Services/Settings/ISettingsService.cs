using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Services.Settings
{
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }
    }
}
