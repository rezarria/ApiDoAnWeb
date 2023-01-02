using Api.Utilities;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Api.Services;

public interface ITokenService
{
	string TaoTokenAsync(ICollection<Claim>? themVao = null);
	bool KiemTraToken(string token);
	ClaimsPrincipal GiaiMaToken(string token);
}

public class TokenServiceOptions
{
	public double ExpiryDurationMinutes { get; set; }
	public string Key { get; set; } = string.Empty;
	public string Issuer { get; set; } = string.Empty;
}

public class TokenService : ITokenService
{
	private readonly double expiryDurationMinutes;
	private readonly string key;
	private readonly string issuer;


	public TokenService(IOptions<TokenServiceOptions> options)
	{
		expiryDurationMinutes = options.Value.ExpiryDurationMinutes;
		key = options.Value.Key;
		issuer = options.Value.Issuer;
	}

	public string TaoTokenAsync(ICollection<Claim>? themVao = null)
		=> TokenUtility.TaoTokenAsync(key, issuer, expiryDurationMinutes, themVao);

	public bool KiemTraToken(string token)
		=> TokenUtility.KiemTraToken(key, issuer, token);

	public ClaimsPrincipal GiaiMaToken(string token)
		=> TokenUtility.GiaiMaToken(key, issuer, token);
}