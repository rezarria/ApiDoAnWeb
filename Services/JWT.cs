using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public interface ITokenService
{
	Task<string> TaoTokenAsync(string key, string issuer, Guid idTaiKhoan, CancellationToken cancellationToken);
	Task<string> TaoTokenAsync(string key, string issuer, string username, CancellationToken cancellationToken);

	bool KiemTraToken(string key, string issuer, string token);
}

public class TokenService : ITokenService
{
	private const double ExpiryDurationMinutes = 30;

	private readonly IQuanLyTaiKhoan _quanLyTaiKhoan;

	public TokenService(IQuanLyTaiKhoan quanLyTaiKhoan)
	{
		_quanLyTaiKhoan = quanLyTaiKhoan;
	}

	public async Task<string> TaoTokenAsync(string key, string issuer, string username, CancellationToken cancellationToken = default)
	{
		List<Claim> claims = new();

		claims.AddRange(await _quanLyTaiKhoan.LayClaimAsync(username, cancellationToken));

		return CreateTaoTokenAsync(key, issuer, claims);
	}

	public async Task<string> TaoTokenAsync(string key, string issuer, Guid idTaiKhoan, CancellationToken cancellationToken = default)
	{
		List<Claim> claims = new();

		claims.AddRange(await _quanLyTaiKhoan.LayClaimAsync(idTaiKhoan, cancellationToken));

		return CreateTaoTokenAsync(key, issuer, claims);
	}

	public bool KiemTraToken(string key, string issuer, string token)
	{
		byte[] mySecret = Encoding.UTF8.GetBytes(key);
		SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(mySecret);
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
		try
		{
			tokenHandler.ValidateToken(token,
									   new TokenValidationParameters
									   {
										   ValidateIssuerSigningKey = true,
										   ValidateIssuer = true,
										   ValidateAudience = true,
										   ValidIssuer = issuer,
										   ValidAudience = issuer,
										   IssuerSigningKey = mySecurityKey
									   },
									   out SecurityToken _);
		}
		catch
		{
			return false;
		}
		return true;
	}

	private static string CreateTaoTokenAsync(string key, string issuer, List<Claim> claims)
	{
		claims.Add(new Claim("role1", "true"));

		SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
		SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
		JwtSecurityToken tokenDescriptor = new(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(ExpiryDurationMinutes), signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
	}
}