#region

using Newtonsoft.Json;

#endregion

namespace Api.Settings;

/// <summary>
/// </summary>
public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	public static WebApplicationBuilder AddControllerEtc(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		services.AddControllers().AddNewtonsoftJson(x =>
												    {
													    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
													    x.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
													    x.SerializerSettings.ConstructorHandling = ConstructorHandling.Default;
													    x.SerializerSettings.Formatting = Formatting.Indented;
													    x.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Reuse;
												    });
		return builder;
	}
}