using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection;

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

	public Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Bắt đầu kiểm tra...");

		Task<bool>[] tasks = KiemTraToanBoDbContext(cancellationToken);

		bool needShutDown = false;

		if (tasks.Any())
		{
			Task.WhenAll(tasks).Wait(cancellationToken);
			needShutDown = tasks.All(x => x.Result);
		}


		if (!needShutDown)
			return Task.CompletedTask;
		_logger.LogError("Đóng chương trình....");
		_hostApplicationLifetimelifeTime.StopApplication();
		return Task.CompletedTask;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	private Task<bool>[] KiemTraToanBoDbContext(CancellationToken cancellationToken)
	{
		using IServiceScope scope = _serviceProvider.CreateScope();
		PropertyInfo? property = scope.GetType().GetProperty("RootProvider", BindingFlags.NonPublic | BindingFlags.Instance);
		object? rootProvider = property?.GetGetMethod(nonPublic: true)?.Invoke(scope, null);
		FieldInfo? field = rootProvider?.GetType().GetField("_callSiteValidator", BindingFlags.NonPublic | BindingFlags.Instance);
		object? callSiteValidator = field?.GetValue(rootProvider);
		FieldInfo? field2 = callSiteValidator?.GetType().GetField("_scopedServices", BindingFlags.NonPublic | BindingFlags.Instance);
		IDictionary? data = field2?.GetValue(callSiteValidator) as IDictionary;
		ICollection<Type>? types = data?.Keys as ICollection<Type>;
		List<Type> tasks = (types ?? throw new InvalidOperationException())
						   .Where(x => x.BaseType is not null && (typeof(DbContext) == x.BaseType || typeof(IdentityDbContext) == x.BaseType))
						   .ToList();
		Task<bool>[] job = tasks.Select(x => KiemTraDatabase((DbContext)scope.ServiceProvider.GetRequiredService(x), cancellationToken)).ToArray();
		return job;
	}
	private async Task<bool> KiemTraDatabase(DbContext context, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Kiểm tra database {Database}", context.Database.GetDbConnection().Database);
		bool needShutDown = false;

		if (await context.Database.CanConnectAsync(cancellationToken))
			_logger.LogInformation("Kết nối ổn ✔");
		else
		{
			_logger.LogError("Không thể kết nối ❌{NewLine}Bắt đầu khởi tạo database...", Environment.NewLine);
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