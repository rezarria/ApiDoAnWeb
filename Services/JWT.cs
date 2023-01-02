using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public interface ITokenService
{
	string TaoTokenAsync(string key, string issuer, ICollection<Claim>? themVao = null);
	bool KiemTraToken(string key, string issuer, string token);
}

public class TokenService : ITokenService
{
	private const double ExpiryDurationMinutes = 30;


	public TokenService()
	{
	}

	public string TaoTokenAsync(string key, string issuer, ICollection<Claim>? themVao = null)
	{
		List<Claim> claims = new();
		claims.Add(new Claim("role1", "true"));
		if (themVao != null)
			claims.AddRange(themVao);
		SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
		SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
		JwtSecurityToken tokenDescriptor = new(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(ExpiryDurationMinutes), signingCredentials: credentials);
		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
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
}