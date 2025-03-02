using Authentication.IServices;
using Authentication.Services;
using BackendServices.IServices;
using BackendServices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Models;
using TailoringCompany.Helpers;
using TailoringCompany.Services;
using TailoringCompany.ViewModels;

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
        // Load Firebase settings from embedded resources
        var assembly = typeof(App).Assembly;
        using var stream = assembly.GetManifestResourceStream("TailoringCompany.firebase-config.json");
        if (stream == null)
        {
            throw new FileNotFoundException("Firebase configuration file not found.");
        }
        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
        builder.Configuration.AddConfiguration(config);
        // Register Firebase settings and services
        builder.Services.Configure<FirebaseSettings>(builder.Configuration.GetSection(nameof(FirebaseSettings)));
        builder.Services.AddSingleton<IAuthService, FirebaseAuthService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.RegisterPagesAndViewModels();

#if DEBUG
        HttpClientHandler insecureHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
        };
        builder.Services.AddSingleton(sp => new HttpClient(insecureHandler));
#else
    builder.Services.AddSingleton<HttpClient>();
#endif

        // Register services
        builder.Services.AddSingleton<IMaterialsPriceUpdateService, MaterialsPriceUpdateService>();
        builder.Services.AddSingleton<IMaterialsService, MaterialsService>();
        builder.Services.AddSingleton<IOrdersService, OrdersService>();
        builder.Services.AddSingleton<IProductsService, ProductsService>();
        builder.Services.AddSingleton<IUserService, UserService>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
