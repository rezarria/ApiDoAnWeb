namespace Api.ThietLap;

public static partial class ThietLap
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    public static void Cors(this IServiceCollection services)
	{
		services.AddCors(options => { });
	}
}