using CW1Preyanshu.Components.Model;
using CW1Preyanshu.Models;
using Microsoft.Extensions.Logging;

namespace CW1Preyanshu
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Register services for dependency injection
            builder.Services.AddSingleton<UserService>();        // Register UserService
            builder.Services.AddSingleton<CurrencyService>();    // Register CurrencyService
              // Register AdminService

            builder.Services.AddMauiBlazorWebView(); // Add MAUI Blazor support

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools(); // Add developer tools in debug mode
            builder.Logging.AddDebug(); // Add debug logging
#endif

            return builder.Build(); // Build and return the app
        }
    }
}
