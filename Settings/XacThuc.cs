#region

using Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RezUtility.Services;
using System.Text;

#endregion

namespace Api.Settings;

/// <summary>
/// </summary>
public static partial class Services
{
	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	public static WebApplicationBuilder AddXacThuc(this WebApplicationBuilder builder)
	{
		IServiceCollection services = builder.Services;
		ConfigurationManager configuration = builder.Configuration;

		services.AddAuthorization()
				.AddTokenService()
				.AddTokenDangXuat()
				.AddQuanLyTaiKhoan()
				.AddAuthentication()
				.AddJwtBearer(options =>
							  {
								  string issuer = configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer không tồn tại");
								  string key = configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key không tồn tại");
								  options.TokenValidationParameters = new TokenValidationParameters
																	  {
																		  ValidateIssuer = true,
																		  ValidateAudience = true,
																		  ValidateLifetime = true,
																		  ValidateIssuerSigningKey = true,
																		  ValidIssuer = issuer,
																		  ValidAudience = issuer,
																		  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
																	  };
							  });

		services.Configure<AuthenticationOptions>(options =>
												  {
													  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
													  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
													  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
												  });

		services.Configure<IdentityOptions>(options =>
										    {
											    options.Password.RequireDigit = false;
											    options.Password.RequireLowercase = false;
											    options.Password.RequireNonAlphanumeric = false;
											    options.Password.RequireUppercase = false;
											    options.Password.RequiredLength = 6;
											    options.Password.RequiredUniqueChars = 0;
											    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
											    options.Lockout.MaxFailedAccessAttempts = 5;
											    options.Lockout.AllowedForNewUsers = true;
											    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
											    options.User.RequireUniqueEmail = false;
										    });
		services.ConfigureApplicationCookie(options =>
										    {
											    // options.Cookie.HttpOnly = true;  
											    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
										    });
		services.Configure<SecurityStampValidatorOptions>(options =>
														  {
															  options.ValidationInterval = TimeSpan.FromSeconds(5);
														  });
		services.Configure<TokenServiceOptions>(options =>
											    {
												    options.ExpiryDurationMinutes = 30;
												    options.Key = configuration["Jwt:Key"] ?? throw new Exception();
												    options.Issuer = configuration["Jwt:Issuer"] ?? throw new Exception();
											    });
		return builder;
	}
}