#region

using System.Reflection;

#endregion

namespace Api.Settings;

public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="services"></param>
	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerDocument();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
							   {
								   string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
								   options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
							   });
	}
}