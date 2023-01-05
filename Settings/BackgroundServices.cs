#region

using Api.BackgroundServices;

#endregion

namespace Api.Settings;

/// <summary>
/// </summary>
public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	public static WebApplicationBuilder AddBackgroundService(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		services.AddHostedService<XoaTokenBackgroundService>();
		return builder;
	}
}