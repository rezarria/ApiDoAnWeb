using Api.Contexts;

namespace Api.Services;

public class CauNoiClientService
{
	private readonly CauHinhDbContext _CauHinhDbContext;
	private readonly ILogger _logger;
	private readonly IServiceScope _serviceScope;


	public CauNoiClientService(IServiceProvider serviceProvider)
	{
		_serviceScope = serviceProvider.CreateScope();
		_CauHinhDbContext = _serviceScope.ServiceProvider.GetRequiredService<CauHinhDbContext>();
		_logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<CauNoiClientService>>();
	}

	public Task KiemTra(CancellationToken cancellationToken)
	{
		_logger.LogInformation("✔");
		return Task.CompletedTask;
	}
}