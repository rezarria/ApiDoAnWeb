namespace Api.Settings;

public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		services.AddCors(options =>
						 {
							 options.AddDefaultPolicy(x => x.AllowAnyOrigin()
														    .WithOrigins("https://localhost:5001")
														    .WithMethods("DELETE", "PATCH", "GET", "POST")
														    .AllowCredentials()
														    .WithHeaders("x-requested-with")
													 );
						 });
		return builder;
	}
}