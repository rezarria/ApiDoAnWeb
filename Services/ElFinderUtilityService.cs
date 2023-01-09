using Api.Contexts;
using Api.Models.ElFinder;
using Duende.IdentityServer.Extensions;
using elFinder.Net.Core;
using elFinder.Net.Drivers.FileSystem.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Api.Services;

public class ElFinderUtilityServiceOptions
{
	public string WebRootPath { get; set; } = string.Empty;
	public string StoragePath { get; set; } = string.Empty;
	public string TempPath { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
}

public interface IElFinderUtilityService
{
	string MapStoragePath(string path);
	Volume GetVolume(IDriver driver, ClaimsPrincipal claimsPrincipal);
	Volume GetVolume(IDriver driver);
	long GetQuota(ClaimsPrincipal user);
	Task<string[]> GetVolumeListAsync(ClaimsPrincipal? claimsPrincipal = null, CancellationToken cancellationToken = default);
}

public class ElFinderUtilityService : IElFinderUtilityService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly string _storagePath;
	private readonly string _tempPath;
	private readonly UriBuilder _uriBuilder;

	public ElFinderUtilityService(IOptions<ElFinderUtilityServiceOptions> options,
								  IServiceProvider serviceProvider
	)
	{
		_serviceProvider = serviceProvider;
		_storagePath = options.Value.StoragePath;
		_tempPath = options.Value.TempPath;
		_uriBuilder = new UriBuilder(options.Value.Url);
	}

	private ElFinderDbContext ElFinderDbContext
	{
		get
		{
			IServiceScope scope = _serviceProvider.CreateScope();
			ElFinderDbContext context = scope.ServiceProvider.GetRequiredService<ElFinderDbContext>();
			return context;
		}
	}

	public string MapStoragePath(string path)
	{
		path = path.Replace("~/", "")
				   .TrimStart('/')
				   .Replace('\\', '/');
		return PathHelper.GetFullPath(PathHelper.SafelyCombine(_storagePath, _storagePath, path));
	}

	public Volume GetVolume(IDriver driver, ClaimsPrincipal claimsPrincipal)
	{
		if (!claimsPrincipal.IsAuthenticated()) return GetVolume(driver);

		using ElFinderDbContext context = ElFinderDbContext;
		Guid userId = Guid.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Id")?.Value ?? throw new Exception());
		User user = context.User.FirstOrDefault(x => x.Id == userId) ?? throw new Exception();
		string volumePath = user.VolumePath;
		_uriBuilder.Path = $"/hethong/files/storage/upload/{volumePath}/";
		string storageUrl = _uriBuilder.ToString();
		_uriBuilder.Path = "/hethong/files/thumb/";
		string thumbUrl = _uriBuilder.ToString();
		string rootPath = MapStoragePath(Path.Combine(".", "upload", volumePath));
		string thumbPath = MapStoragePath(Path.Combine(".", "thumb", volumePath));
		Volume volume = new(driver,
						    rootPath,
						    _tempPath,
						    storageUrl,
						    thumbUrl,
						    thumbnailDirectory: thumbPath)
						{
							Name = user.Id.ToString(),
							MaxUploadFiles = 20,
							MaxUploadSizeInMb = 10,
							MaxUploadConnections = 5
						};
		return volume;
	}

	public Volume GetVolume(IDriver driver)
	{
		_uriBuilder.Path = "/hethong/files/storage/upload/test/";
		string storageUrl = _uriBuilder.ToString();
		_uriBuilder.Path = "/hethong/files/thumb/";
		string thumbUrl = _uriBuilder.ToString();
		string rootPath = MapStoragePath(Path.Combine(".", "upload", "test"));
		string thumbPath = MapStoragePath(Path.Combine(".", "thumb", "test"));
		Volume volume = new(driver,
						    rootPath,
						    _tempPath,
						    storageUrl,
						    thumbUrl,
						    thumbnailDirectory: thumbPath)
						{
							Name = "test",
							MaxUploadFiles = 20,
							MaxUploadSizeInMb = 10,
							MaxUploadConnections = 3
						};
		return volume;
	}
	public long GetQuota(ClaimsPrincipal user)
	{
		if (!user.IsAuthenticated()) return 0;
		using ElFinderDbContext context = ElFinderDbContext;
		Guid userId = Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == "Id")?.Value ?? throw new Exception());
		return context.User.FirstOrDefault(x => x.Id == userId)?.QuotaInBytes ?? 0;
	}
	public async Task<string[]> GetVolumeListAsync(ClaimsPrincipal? claimsPrincipal = null, CancellationToken cancellationToken = default)
	{
		if (claimsPrincipal is not null && claimsPrincipal.IsAuthenticated())
		{
			Guid userId = Guid.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Id")?.Value ?? throw new Exception());
			await using ElFinderDbContext context = ElFinderDbContext;
			User? user = await context.User.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
			if (user is not null)
				return new[] { "test", user.VolumePath };
		}

		return new[] { "test" };
	}
}