using System.Reflection;

namespace Api.ThietLap;

public static partial class ThietLap
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static void ThietLapSwagger(this IServiceCollection services)
    {
		services.AddSwaggerDocument();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}