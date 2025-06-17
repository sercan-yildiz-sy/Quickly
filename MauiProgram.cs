using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Quickly
{
    /// <summary>
    /// Configures and builds the .NET MAUI application, including services, fonts, handlers, and toolkits.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Creates and configures the MAUI app.
        /// </summary>
        /// <returns>The configured <see cref="MauiApp"/> instance.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                // Register the main application class
                .UseMauiApp<App>()
                // Register the .NET MAUI Community Toolkit
                .UseMauiCommunityToolkit()
                // Register the Syncfusion MAUI Toolkit
                .ConfigureSyncfusionToolkit()
                // Configure custom handlers for platform-specific control appearance
                .ConfigureMauiHandlers(handlers =>
                {
                    // Remove background tint from Picker controls on Android for a cleaner look
                    Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
                    {
#if ANDROID
                            handler.PlatformView.BackgroundTintList =
                                Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
                    });
                    // Remove background tint from Entry controls on Android for a cleaner look
                    Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
                    {
#if ANDROID
                            handler.PlatformView.BackgroundTintList =
                                Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
                    });
                })
                // Register custom fonts for use throughout the app
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Roboto-ExtraBold.ttf", "RobotoExtraBold");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto");
                    fonts.AddFont("LibreBaskerville-Bold.ttf", "LibreBaskerville-Bold");
                    fonts.AddFont("LibreBaskerville-Regular.ttf", "LibreBaskerville-Regular");
                    fonts.AddFont("LibreBaskerville-Italic.ttf", "LibreBaskerville-Italic");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
                // Enable debug logging in development builds
                builder.Logging.AddDebug();
                builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            // Register view models and pages for dependency injection
            builder.Services.AddSingleton<ItemAddingPageModel>();
            builder.Services.AddSingleton<ProfilePageModel>();
            //builder.Services.AddSingleton<TryingPage2PageModel>();
            //builder.Services.AddTransient<TryingPage2>();

            builder.Services.AddTransient<ItemDetailsPageModel>();
            builder.Services.AddTransient<ItemDetailsPage>();
            builder.Services.AddTransient<MainPageModel>();

            // Register ItemDetailsPage with a shell route and its required view model
            builder.Services.AddTransientWithShellRoute<ItemDetailsPage, ItemAddingPageModel>("ItemDetailsPage");

            return builder.Build();
        }
    }
}
