#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

#endregion

namespace Api.Settings;

/// <summary>
/// </summary>
public static partial class Services
{
	public static void AddJsonPatch(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllers(options => options.InputFormatters.Insert(0, GetJsonPatchInputFormatter()));
	}

	private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
	{
		ServiceProvider builder = new ServiceCollection()
								  .AddLogging()
								  .AddMvc()
								  .AddNewtonsoftJson()
								  .Services.BuildServiceProvider();

		return builder
			   .GetRequiredService<IOptions<MvcOptions>>()
			   .Value
			   .InputFormatters
			   .OfType<NewtonsoftJsonPatchInputFormatter>()
			   .First();
	}
}