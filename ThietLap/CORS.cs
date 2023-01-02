namespace Api.ThietLap;

public static partial class ThietLap
{
	/// <summary>
	/// </summary>
	/// <param name="services"></param>
	public static void ThietLapCors(this IServiceCollection services)
	{
		services.AddCors(options =>
						 {
							 options.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyHeader().WithMethods("DELETE", "PATCH", "GET", "POST"));
						 });
	}
}