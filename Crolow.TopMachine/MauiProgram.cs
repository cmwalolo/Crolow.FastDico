using Crolow.TopMachine.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Radzen;

namespace Crolow.TopMachine
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

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddRadzenComponents();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            CreateConfiguration(builder);
            return builder.Build();
        }

        private static void CreateConfiguration(MauiAppBuilder builder)
        {
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var databaseSettings = builder.Configuration
                .GetSection("DatabaseSettings")
                .Get<DatabaseSettings>();

            builder.Services.AddSingleton(databaseSettings);
        }
    }
}
