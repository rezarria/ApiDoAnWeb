#region

using Api.Areas.Edu.Contexts;
using Api.Contexts;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Api.Settings;

/// <summary>
/// </summary>
public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	public static void DbContext(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		ConfigurationManager configuration = builder.Configuration;

		services.AddDbContext<AppDbContext>(options =>
										    {
											    switch (configuration["Settings:AppDb"] ?? throw new Exception("Chưa chỉnh appsettings.json"))
											    {
												    case "MySql":
													    string connectionString = configuration.GetConnectionString("AppDb_Mysql") ?? throw new Exception();
													    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
													    break;
												    case "SqlServer":
													    options.UseSqlServer(configuration.GetConnectionString("AppDb_SqlServer"));
													    break;
											    }
										    });
		services.AddDbContext<XacThucDbContext>(options => options.UseSqlite(configuration.GetConnectionString("XacThuc")));
		services.AddDbContext<CauHinhDbContext>(options => options.UseSqlite(configuration.GetConnectionString("CauHinh")));
	}
}