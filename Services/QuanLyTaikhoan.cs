using Api.Areas.Edu.Contexts;
using Api.Models;
using Api.PhuTro;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaiKhoan=Api.Areas.Edu.Models.TaiKhoan;

namespace Api.Services;

public interface IQuanLyTaiKhoan
{
	Task<string> DangNhapAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default);
	Task<string> DangNhapAsync(string userName, string matKhau, CancellationToken cancellationToken = default);
	Task DangKyAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default);
	Task DangKyAsync(TaiKhoan taiKhoan, string matKhau, CancellationToken cancellationToken = default);
	void DangXuat(string token);
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
	private readonly ITokenDangXuatService _tokenDangXuat;

	public QuanLyTaiKhoan(AppDbContext context, ITokenService tokenService, IConfiguration configuration, ITokenDangXuatService tokenDangXuat)
	{
		_context = context;
		_tokenService = tokenService;
		_configuration = configuration;
		_tokenDangXuat = tokenDangXuat;
	}

	public async Task<string> DangNhapAsync(Guid idTaiKhoan, string matKhau, CancellationToken cancellationToken = default)
	{
		if (await KiemTraId(idTaiKhoan, cancellationToken))
			if (await XacThucAsync(idTaiKhoan, matKhau, cancellationToken))
			{
				List<Claim> claims = await LayClaimAsync(idTaiKhoan, cancellationToken);
				return _tokenService.TaoTokenAsync(claims);
			}
		return string.Empty;
	}

	public async Task<string> DangNhapAsync(string userName, string matKhau, CancellationToken cancellationToken = default)
	{
		if (await KiemTraUsername(userName, cancellationToken))
			if (await XacThucAsync(userName, matKhau, cancellationToken))
			{
				List<Claim> claims = await LayClaimAsync(userName, cancellationToken);
				return _tokenService.TaoTokenAsync(claims);
			}
		return string.Empty;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="taiKhoan"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="Exception"></exception>
	public async Task DangKyAsync(TaiKhoan taiKhoan, CancellationToken cancellationToken = default)
	{
		if (_context.TaiKhoan.Any(x => x.Username.Equals(taiKhoan.Username)))
			throw new Exception("Tài khoản đã tồn tại");
		_context.Add(taiKhoan);
		try
		{
			await _context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			throw new Exception("Cập nhật database thất bại");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="taiKhoan"></param>
	/// <param name="matKhau"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="Exception"></exception>
	public async Task DangKyAsync(TaiKhoan taiKhoan, string matKhau, CancellationToken cancellationToken = default)
	{
		taiKhoan.MatKhau = MatKhau.MaHoaMatKhau(matKhau);
		await DangKyAsync(taiKhoan, cancellationToken);
	}

	public void DangXuat(string token)
	{
		ClaimsPrincipal claimsPrincipal = _tokenService.GiaiMaToken(token);
		TokenDangXuat tokenDangXuat = new()
									  {
										  Token = token,
										  Exp = TimeSpan.FromSeconds(Double.Parse(claimsPrincipal.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Exp))!.Value))
									  };
		_tokenDangXuat.ThemToken(tokenDangXuat);
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