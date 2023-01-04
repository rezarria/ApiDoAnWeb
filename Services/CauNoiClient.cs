using Api.Contexts;

namespace Api.Services;

public class CauNoiClientService
{
	private readonly IServiceScope _serviceScope;
	private readonly CauHinhDbContext _hinhDbContext;
	private readonly ILogger _logger;


	public CauNoiClientService(IServiceProvider serviceProvider)
	{
		_serviceScope = serviceProvider.CreateScope();
		_hinhDbContext = _serviceScope.ServiceProvider.GetRequiredService<CauHinhDbContext>();
		_logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<CauNoiClientService>>();
	}

	public Task KiemTra(CancellationToken cancellationToken)
	{
		_logger.LogInformation("✔");
		return Task.CompletedTask;
	}
}
