#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace GifAnimation;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCompatibility()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureGifAnimation();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static MauiAppBuilder ConfigureGifAnimation(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
#if IOS ||MACCATALYST
                Microsoft.Maui.Handlers.ImageHandler.Mapper.AppendToMapping("gifimage", async (handler, view) =>
                {
                    if (view is GIFImage)
                    {
                        var imageStream = await ConvertImageSourceToStreamAsync(((GIFImage)view).ImageName);
                        var nsdata = await GetDataAsync(imageStream);
                        var image = AnimatedImageView.GetAnimatedImageView(nsdata,handler.PlatformView);
                    }
                });
#endif

        });
        return builder;
    }
#if IOS || MACCATALYST
        async static Task<NSData> GetDataAsync(Stream stream)
        {
            return await Task.Run(() => { return NSData.FromStream(stream); });
        }
#endif

    public static async Task<Stream> ConvertImageSourceToStreamAsync(string imageName)
    {
        return await FileSystem.OpenAppPackageFileAsync(imageName);
    }
}
