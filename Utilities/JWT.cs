using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Api.Utilities;

public static class TokenUtility
{
	public static string TaoTokenAsync(string key, string issuer, double expiryDurationMinutes, ICollection<Claim>? themVao = null)
	{
		List<Claim> claims = new();
		claims.Add(new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()));
		if (themVao != null)
			claims.AddRange(themVao);
		SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
		SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
		JwtSecurityToken tokenDescriptor = new(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(expiryDurationMinutes), signingCredentials: credentials);
		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
	}

	public static bool KiemTraToken(string key, string issuer, string token)
	{
		try
		{
			_ = GiaiMaToken(key, issuer, token);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static ClaimsPrincipal GiaiMaToken(string key, string issuer, string token)
	{
		byte[] mySecret = Encoding.UTF8.GetBytes(key);
		SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(mySecret);
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.ValidateToken(token,
										  new TokenValidationParameters
										  {
											  ValidateIssuerSigningKey = true,
											  ValidateIssuer = true,
											  ValidateAudience = true,
											  ValidIssuer = issuer,
											  ValidateLifetime = false,
											  ValidAudience = issuer,
											  IssuerSigningKey = mySecurityKey
										  }, out SecurityToken _);
	}
}