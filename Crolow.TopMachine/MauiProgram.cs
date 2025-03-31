using Blazored.LocalStorage;
using Crolow.TopMachine.Core;
using Crolow.TopMachine.Data;
using Crolow.TopMachine.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Radzen;

namespace Crolow.TopMachine
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            EnsureFiles.CopyAssetsToAppDataDirectoryAsync();

            //string appDataPath = FileSystem.AppDataDirectory;
            //Console.WriteLine(appDataPath);

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddRadzenComponents();
            builder.Services.AddBlazoredLocalStorageAsSingleton();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            CrolowPixConfiguration.ConfigureServices(builder.Services);
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
