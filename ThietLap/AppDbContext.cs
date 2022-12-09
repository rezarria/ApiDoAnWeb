using Api.Areas.Edu.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Api.ThietLap;

/// <summary>
/// </summary>
public static partial class ThietLap
{
    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    public static void ThemAppDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            string connectionString = builder.Configuration.GetConnectionString("AppDb_Mysql") ?? throw new Exception();
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }
}