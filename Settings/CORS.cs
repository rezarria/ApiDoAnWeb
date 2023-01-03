namespace Api.Settings;

public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="services"></param>
	public static void AddCors(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		services.AddCors(options =>
						 {
							 options.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyHeader().WithMethods("DELETE", "PATCH", "GET", "POST"));
						 });
	}
}