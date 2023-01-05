using Api.Contexts;

namespace Api.Services;

public class CauNoiClientService
{
	private readonly CauHinhDbContext _CauHinhDbContext;
	private readonly ILogger _logger;


	public CauNoiClientService(IServiceProvider serviceProvider)
	{
		IServiceScope serviceScope = serviceProvider.CreateScope();
		_CauHinhDbContext = serviceScope.ServiceProvider.GetRequiredService<CauHinhDbContext>();
		_logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<CauNoiClientService>>();
	}

	public Task KiemTra(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		_logger.LogInformation("✔");
		return Task.CompletedTask;
	}
}