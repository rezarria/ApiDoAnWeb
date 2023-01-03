using Api.Areas.Edu.Contexts;
using Api.Contexts;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

namespace Api.Tasks;

public class CheckTask
{
	private readonly IHostApplicationLifetime _hostApplicationLifetimelifeTime;
	private readonly ILogger _logger;
	private readonly IServiceProvider _serviceProvider;

	private CheckTask(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_logger = serviceProvider.GetRequiredService<ILogger<CheckTask>>();
		_hostApplicationLifetimelifeTime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
	}

	public static void Check(IServiceProvider serviceProvider)
	{
		CheckTask checkTask = new(serviceProvider);
		checkTask.ExecuteAsync().Wait();
	}

	public async Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Bắt đầu kiểm tra...");
		using IServiceScope scope = _serviceProvider.CreateScope();
		XacThucDbContext xacThucDbContext = scope.ServiceProvider.GetRequiredService<XacThucDbContext>();
		AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		bool needShutDown;

		needShutDown = await KiemTraDatabase(xacThucDbContext, cancellationToken) ||
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
			_logger.LogError("Không thể kết nối \u274c{NewLine}Bắt đầu khởi tạo database...", Environment.NewLine);
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