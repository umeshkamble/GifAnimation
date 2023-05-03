using Microsoft.Extensions.Logging;
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
			 .ConfigureMauiHandlers(handlers =>
             		{
#if IOS
                 handlers.AddCompatibilityRenderer(typeof(GIFImage), typeof(Microsoft.Maui.Controls.Compatibility.Platform.iOS.ImageRenderer));
#endif
             		});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
