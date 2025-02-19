using Authentication.IServices;
using Authentication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace TailoringCompany;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Add configuration
        var assembly = typeof(App).Assembly;
        using var stream = assembly.GetManifestResourceStream("TailoringCompany.firebase-config.json");
        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);

        // Configure services
        builder.Services.Configure<FirebaseSettings>(
            builder.Configuration.GetSection(nameof(FirebaseSettings)));
        builder.Services.AddSingleton<IAuthService, FirebaseAuthService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
