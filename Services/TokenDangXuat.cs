using Api.Contexts;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Api.Services;

public interface ITokenDangXuatService
{
	int SoLuongToiDa { get; set; }
	bool SyncKhiGoi { get; set; }
	void ThemToken(TokenDangXuat token);
	bool KiemTra(string token);
	Task XoaTuDongAsync(CancellationToken cancellationToken = default);
	void LuuDatabase();
}

public class TokenDangXuatService : ITokenDangXuatService
{
	private readonly ConcurrentStack<TokenDangXuat> _danhSachBoNhoTrong;
	private readonly object _danhSachBoNhoTrongLock = new();
	private readonly ConcurrentStack<TokenDangXuat> _danhSachTokenTrongDatabase;
	private readonly ILogger _logger;
	private readonly IServiceProvider _serviceProvider;

	public TokenDangXuatService(IServiceProvider serviceProvider, ILogger<TokenDangXuatService> logger)
	{
		_serviceProvider = serviceProvider;
		using IServiceScope scope = _serviceProvider.CreateScope();
		XacThucContext context = scope.ServiceProvider.GetRequiredService<XacThucContext>();
		_danhSachBoNhoTrong = new ConcurrentStack<TokenDangXuat>();
		_danhSachTokenTrongDatabase = new ConcurrentStack<TokenDangXuat>(context.TokenDangXuat.AsNoTracking());
		_logger = logger;
	}
	public void LuuDatabase()
	{
		_logger.LogInformation("Lưu token từ ram vào database");
		using IServiceScope scope = _serviceProvider.CreateScope();
		XacThucContext context = scope.ServiceProvider.GetRequiredService<XacThucContext>();
		lock (_danhSachBoNhoTrongLock)
		{
			context.AddRange(_danhSachBoNhoTrong);
			context.SaveChanges();
		}
	}
	public int SoLuongToiDa { get; set; } = 1000;

	public bool KiemTra(string token)
	{
		_logger.LogInformation("Kiểm tra token");
		lock (_danhSachBoNhoTrongLock)
		{
			return _danhSachBoNhoTrong.Any(x => x.Token.Equals(token)) ||
				   _danhSachTokenTrongDatabase.Any(x => x.Token.Equals(token));
		}
	}

	public bool SyncKhiGoi { get; set; } = false;

	public void ThemToken(TokenDangXuat token)
	{
		lock (_danhSachBoNhoTrongLock)
		{
			if (!_danhSachBoNhoTrong.Any(x => x.Token.SequenceEqual(token.Token)))
			{
				_logger.LogInformation("Thêm token vào hàng chờ |" + token.Token);
				_danhSachBoNhoTrong.Push(token);
			}
		}
	}

	public async Task XoaTuDongAsync(CancellationToken cancellationToken = default)
	{
		TokenDangXuat? token;
		lock (_danhSachBoNhoTrongLock)
		{
			while (_danhSachBoNhoTrong.TryPeek(out token))
			{
				if (token.Exp > DateTime.Now.Subtract(DateTime.UnixEpoch))
					break;
				_danhSachBoNhoTrong.TryPop(out _);
				_logger.LogInformation("Xóa token đăng xuất trong bộ nhớ");
			}
		}

		List<TokenDangXuat> danhSachXoa = new();
		while (_danhSachTokenTrongDatabase.TryPeek(out token))
		{
			if (token.Exp > DateTime.Now.Subtract(DateTime.UnixEpoch))
				break;
			danhSachXoa.Add(token);
		}

		if (danhSachXoa.Any())
		{
			_logger.LogInformation("Xóa token đăng xuất trong database");
			using IServiceScope scope = _serviceProvider.CreateScope();
			XacThucContext context = scope.ServiceProvider.GetRequiredService<XacThucContext>();
			context.RemoveRange(danhSachXoa);
			await context.SaveChangesAsync(cancellationToken);
		}
	}
}