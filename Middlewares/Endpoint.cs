namespace Api.Middlewares;

public static class EndpointExtensions
{
	public static IApplicationBuilder UseMyEndpoints(this IApplicationBuilder builder)
	{
		builder.UseEndpoints(endpoints => endpoints.MapControllers());
		return builder;
	}
}