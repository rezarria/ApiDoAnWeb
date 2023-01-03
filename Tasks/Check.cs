using Api.Areas.Edu.Contexts;
using Api.Contexts;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

namespace Api.Tasks;

public class CheckBackgroundService
{
	private readonly IHostApplicationLifetime _hostApplicationLifetimelifeTime;
	private readonly ILogger _logger;
	private readonly IServiceProvider _serviceProvider;

	public CheckBackgroundService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_logger = serviceProvider.GetRequiredService<ILogger<IServer>>();
		_hostApplicationLifetimelifeTime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
	}

	public static void Check(IServiceProvider serviceProvider)
	{
		CheckBackgroundService checkBackgroundService = new(serviceProvider);
		checkBackgroundService.ExecuteAsync().Wait();
	}

	public async Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Bắt đầu kiểm tra...");
		using IServiceScope scope = _serviceProvider.CreateScope();
		XacThucContext xacThucContext = scope.ServiceProvider.GetRequiredService<XacThucContext>();
		AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		bool needShutDown;

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
		_logger.LogInformation("Kiểm tra database {Database}", context.Database.GetDbConnection().Database);
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