using Api.Areas.Edu.Contexts;
using Api.PhuTro;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaiKhoan=Api.Areas.Edu.Models.TaiKhoan;

namespace Api.Services;

public interface IQuanLyTaiKhoan
{
	Task<string> DangNhapAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default);
	Task<string> DangNhapAsync(string userName, string matKhau, CancellationToken cancellationToken = default);
	Task<bool> KiemTraUsername(string username, CancellationToken cancellationToken = default);
	Task<bool> KiemTraId(Guid id, CancellationToken cancellationToken = default);
	Task<List<Claim>> LayClaimAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default);
	Task<List<Claim>> LayClaimAsync(Guid idTaiKhoan, CancellationToken cancellationToken = default);
	Task<List<Claim>> LayClaimAsync(string username, CancellationToken cancellationToken = default);
	bool XacThuc(TaiKhoan taiKhoan, string matKhau);
	Task<bool> XacThucAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default);
}

public class QuanLyTaiKhoan : IQuanLyTaiKhoan
{
	private readonly IConfiguration _configuration;
	private readonly AppDbContext _context;
	private readonly ITokenService _tokenService;

	public QuanLyTaiKhoan(AppDbContext context, ITokenService tokenService, IConfiguration configuration)
	{
		_context = context;
		_tokenService = tokenService;
		_configuration = configuration;
	}

	public async Task<string> DangNhapAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default)
	{
		if (await KiemTraId(idTaiKhoan, cancellationToken))
			if (await XacThucAsync(idTaiKhoan, matKhau, cancellationToken))
			{
				string key = _configuration["Jwt:Key"] ?? throw new Exception();
				string issuer = _configuration["Jwt:Issuer"] ?? throw new Exception();
				List<Claim> claims = await LayClaimAsync(idTaiKhoan, cancellationToken);
				return _tokenService.TaoTokenAsync(key, issuer, claims);
			}
		return string.Empty;
	}

	public async Task<string> DangNhapAsync(string userName, string matKhau, CancellationToken cancellationToken = default)
	{
		if (await KiemTraUsername(userName, cancellationToken))
			if (await XacThucAsync(userName, matKhau, cancellationToken))
			{
				string key = _configuration["Jwt:Key"] ?? throw new Exception();
				string issuer = _configuration["Jwt:Issuer"] ?? throw new Exception();
				List<Claim> claims = await LayClaimAsync(userName, cancellationToken);
				return _tokenService.TaoTokenAsync(key, issuer, claims);
			}
		return string.Empty;
	}

	public async Task<bool> KiemTraUsername(string username, CancellationToken cancellationToken = default)
	{
		return await _context.TaiKhoan.AnyAsync(predicate: x => x.Username == username, cancellationToken);
	}

	public async Task<bool> KiemTraId(Guid id, CancellationToken cancellationToken = default)
	{
		return await _context.TaiKhoan.AnyAsync(predicate: x => x.Id == id, cancellationToken);
	}

	public async Task<List<Claim>> LayClaimAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default)
	{
		return await LayClaimAsync(taiKhoan.Id, cancellationToken);
	}

	public async Task<List<Claim>> LayClaimAsync(Guid idTaiKhoan, CancellationToken cancellationToken = default)
	{
		return await _context.ClaimTaikhoan
						     .Where(x => x.IdTaiKhoan == idTaiKhoan)
						     .Select(x => new Claim(x.Ten, x.GiaTri ?? "null"))
						     .AsNoTracking()
						     .ToListAsync(cancellationToken);
	}

	public async Task<List<Claim>> LayClaimAsync(string username, CancellationToken cancellationToken = default)
	{
		Guid? id = await _context.TaiKhoan.Where(x => x.Username == username).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
		if (!id.HasValue) return new List<Claim>();
		return await LayClaimAsync(id.Value, cancellationToken);
	}

	public async Task<bool> XacThucAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default)
	{
		byte[]? hashed = await _context.TaiKhoan.Where(x => x.Id == idTaiKhoan).Select(x => x.MatKhau).FirstOrDefaultAsync(cancellationToken);
		return hashed is not null && MatKhau.XacThucMatKhau(hashed, matKhau);
	}

	public bool XacThuc(TaiKhoan taiKhoan, string matKhau)
	{
		if (taiKhoan.MatKhau is null) return false;
		return MatKhau.XacThucMatKhau(taiKhoan.MatKhau, matKhau);
	}

	public async Task<bool> XacThucAsync(string username, string matKhau, CancellationToken cancellationToken = default)
	{
		byte[]? hashed = await _context.TaiKhoan.Where(x => x.Username == username).Select(x => x.MatKhau).FirstOrDefaultAsync(cancellationToken);
		return hashed is not null && MatKhau.XacThucMatKhau(hashed, matKhau);
	}
}