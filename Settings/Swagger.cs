#region

using System.Reflection;

#endregion

namespace Api.Settings;

public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		services.AddSwaggerDocument();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
							   {
								   string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
								   options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
							   });
		return builder;
	}
}