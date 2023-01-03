using Api.Areas.Edu.Contexts;
using Api.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.BackgroundServices;

public class CheckBackgroundService : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<CheckBackgroundService> _logger;
	private readonly IHostApplicationLifetime _hostApplicationLifetimelifeTime;

	public CheckBackgroundService(IServiceProvider serviceProvider, ILogger<CheckBackgroundService> logger, IHostApplicationLifetime hostApplicationLifetimelifeTime)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
		_hostApplicationLifetimelifeTime = hostApplicationLifetimelifeTime;
	}
	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Bắt đầu kiểm tra...");
		using IServiceScope scope = _serviceProvider.CreateScope();
		XacThucContext xacThucContext = scope.ServiceProvider.GetRequiredService<XacThucContext>();
		AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		bool needShutDown = false;

		needShutDown = await KiemTraDatabase(xacThucContext, cancellationToken) ||
					   await KiemTraDatabase(appDbContext, cancellationToken);

		if (needShutDown)
		{
			_logger.LogError("Đóng chương trình....");
			_hostApplicationLifetimelifeTime.StopApplication();
		}
	}
	private async Task<bool> KiemTraDatabase(DbContext context, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Kiểm tra database {0}", context.Database.GetDbConnection().Database);
		bool needShutDown = false;

		if (await context.Database.CanConnectAsync(cancellationToken))
			_logger.LogInformation("Kết nối ổn ✔");
		else
		{
			_logger.LogError("Không thể kết nối ❌\nBắt đầu khởi tạo database...");
			try
			{
				await context.Database.EnsureCreatedAsync(cancellationToken);
				_logger.LogInformation("Tạo database thành công ✔");
			}
			catch (Exception)
			{
				_logger.LogError("Tạo database thất bại ❌");
				needShutDown = true;
			}
		}

		return needShutDown;
	}
}