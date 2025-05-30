﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Quickly
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
                    Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
                    {
#if ANDROID
                        handler.PlatformView.BackgroundTintList =
            Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
                    });
                    Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
                    {
#if ANDROID
                        handler.PlatformView.BackgroundTintList =
            Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
                    });
                })
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
    		builder.Logging.AddDebug();
    		builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            builder.Services.AddSingleton<ItemAddingPageModel>();
            builder.Services.AddSingleton<ProfilePageModel>();
            //builder.Services.AddSingleton<TryingPage2PageModel>();
            //builder.Services.AddTransient<TryingPage2>();

            builder.Services.AddTransient<ItemDetailsPageModel>();
            builder.Services.AddTransient<ItemDetailsPage>();
            builder.Services.AddTransient<MainPageModel>();

            builder.Services.AddTransientWithShellRoute<ItemDetailsPage, ItemAddingPageModel>("ItemDetailsPage");




            return builder.Build();
        }
    }
}
