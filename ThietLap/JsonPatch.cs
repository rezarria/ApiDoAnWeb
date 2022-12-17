#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

#endregion

namespace Api.ThietLap;

/// <summary>
/// </summary>
public static partial class ThietLap
{
	public static void ThemJsonPatch(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllers(options => options.InputFormatters.Insert(0, GetJsonPatchInputFormatter()));
	}

	public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
	{
		var builder = new ServiceCollection()
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