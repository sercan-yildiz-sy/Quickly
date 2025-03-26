using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Quicky
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

            builder.Services.AddSingleton<ProjectRepository>();
            builder.Services.AddSingleton<TaskRepository>();
            builder.Services.AddSingleton<CategoryRepository>();
            builder.Services.AddSingleton<TagRepository>();
            builder.Services.AddSingleton<SeedDataService>();
            builder.Services.AddSingleton<ModalErrorHandler>();
            builder.Services.AddSingleton<MainPageModel>();
            builder.Services.AddSingleton<ItemAddingPageModel>();
            builder.Services.AddSingleton<ManageMetaPageModel>();

            //builder.Services.AddSingleton<TryingPage2PageModel>();
            //builder.Services.AddTransient<TryingPage2>();

            builder.Services.AddTransient<TryingPage>();
            builder.Services.AddTransient<TryingPageModel>();

            builder.Services.AddTransientWithShellRoute<TryingPage2, TryingPage2PageModel>("TryingPage2");



            builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
            builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");

            return builder.Build();
        }
    }
}
