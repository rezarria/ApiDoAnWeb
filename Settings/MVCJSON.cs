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
	/// <param name="services"></param>
	public static void AddControllerETC(this IServiceCollection services)
	{
		services.AddControllers().AddNewtonsoftJson(x =>
												    {
													    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
													    x.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
													    x.SerializerSettings.ConstructorHandling = ConstructorHandling.Default;
													    x.SerializerSettings.Formatting = Formatting.Indented;
													    x.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Reuse;
												    });
	}
}