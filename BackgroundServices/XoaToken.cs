using Api.Services;

namespace Client.BackgroundServices;

public sealed class XoaTokenBackgroundService : BackgroundService
{
	private readonly ILogger<XoaTokenBackgroundService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly ITokenDangXuatService _tokenDangXuat;

	public XoaTokenBackgroundService(
		IServiceProvider serviceProvider,
		ILogger<XoaTokenBackgroundService> logger,
		ITokenDangXuatService tokenDangXuat)
	{
		(_serviceProvider, _logger, _tokenDangXuat) = (serviceProvider, logger, tokenDangXuat);
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation($"{nameof(XoaTokenBackgroundService)} is running.");
		_tokenDangXuat.SyncKhiGoi = true;
		await DoWorkAsync(cancellationToken);
	}

	private async Task DoWorkAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation($"{nameof(XoaTokenBackgroundService)} is working.");
		while (!cancellationToken.IsCancellationRequested)
		{
			_logger.LogInformation("Tiến hành tìm và xóa token cũ");
			await _tokenDangXuat.XoaTuDongAsync(cancellationToken);
			await Task.Delay(5_000, cancellationToken);
		}
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation($"{nameof(XoaTokenBackgroundService)} is stopping.");
		_tokenDangXuat.LuuDatabase();
		await base.StopAsync(cancellationToken);
	}
}