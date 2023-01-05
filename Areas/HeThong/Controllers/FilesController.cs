using Api.Models.ElFinder;
using Api.Models.ElFinder.Responses;
using Api.Services;
using Duende.IdentityServer.Extensions;
using elFinder.Net.AspNetCore.Extensions;
using elFinder.Net.AspNetCore.Helper;
using elFinder.Net.Core;
using elFinder.Net.Core.Models.Command;
using elFinder.Net.Core.Models.Response;
using elFinder.Net.Core.Models.Result;
using elFinder.Net.Core.Services.Drawing;
using elFinder.Net.Drivers.FileSystem.Extensions;
using elFinder.Net.Drivers.FileSystem.Services;
using elFinder.Net.Plugins.FileSystemQuotaManagement;
using elFinder.Net.Plugins.FileSystemQuotaManagement.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using Timer=System.Timers.Timer;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
public class FilesController : Controller
{
	private const int HeartbeatInterval = 2000;
	private static readonly ConcurrentDictionary<Guid, UploadPulseModel> UploadStatus = new();
	private readonly IConnector _connector;
	private readonly IDriver _driver;
	private readonly IElFinderUtilityService _elFinderUtilityService;
	private readonly ILogger _logger;
	private readonly IPictureEditor _pictureEditor;
	private readonly IStorageManager _storageManager;
	private readonly IThumbnailBackgroundGenerator _thumbnailGenerator;
	private readonly IVideoEditor _videoEditor;

	public FilesController(IConnector connector,
						   IDriver driver,
						   IElFinderUtilityService elFinderUtilityService,
						   IStorageManager storageManager,
						   ILogger<FilesController> logger,
						   IVideoEditor videoEditor,
						   IPictureEditor pictureEditor,
						   IThumbnailBackgroundGenerator thumbnailGenerator)
	{
		_connector = connector;
		_driver = driver;
		_storageManager = storageManager;
		_elFinderUtilityService = elFinderUtilityService;
		_logger = logger;
		_videoEditor = videoEditor;
		_pictureEditor = pictureEditor;
		_thumbnailGenerator = thumbnailGenerator;
	}

	private UploadPulseModel CurrentUploadStatus
	{
		get
		{
			if (!Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value, out Guid id))
				id = Guid.Empty;
			lock (UploadStatus)
			{
				return UploadStatus.GetOrAdd(id, valueFactory: _ => new UploadPulseModel
																    {
																	    UploadedFiles = new List<string>()
																    });
			}
		}
	}

	[Route("connector")]
	public async Task<IActionResult> Connector()
	{
		CancellationTokenSource? ccTokenSource = ConnectorHelper.RegisterCcTokenSource(HttpContext);
		(IVolume volume, long quota) = await SetupConnectorAsync(ccTokenSource.Token);
		ConnectorCommand? cmd = ConnectorHelper.ParseCommand(Request);
		ConnectorResult? conResult = await _connector.ProcessAsync(cmd, ccTokenSource);
		CustomizeResponse(conResult, volume, quota);
		IActionResult? actionResult = conResult.ToActionResult(HttpContext);
		return actionResult;
	}

	[Route("thumb/{target}")]
	public async Task<IActionResult> Thumb(string target)
	{
		await SetupConnectorAsync(HttpContext.RequestAborted);
		ImageWithMimeType? thumb = await _connector.GetThumbAsync(target, HttpContext.RequestAborted);
		IActionResult? actionResult = ConnectorHelper.GetThumbResult(thumb);
		return actionResult;
	}

	[Route("storage/{**path}")]
	public async Task<IActionResult> GetFile(string path)
	{
		await SetupConnectorAsync(HttpContext.RequestAborted);
		string fullPath = _elFinderUtilityService.MapStoragePath(path);
		return await this.GetPhysicalFileAsync(_connector, fullPath, HttpContext.RequestAborted);
	}

	[HttpPost("upload-pulse")]
	public IActionResult PulseUpload()
	{
		UpdatePulseStatus();
		return NoContent();
	}

	private void CustomizeResponse(ConnectorResult connectorResult, IVolume volume, long quota)
	{
		(long storage, bool _) = _storageManager.GetOrCreateDirectoryStorage(volume.RootDirectory, createFunc: dir => volume.Driver.CreateDirectory(dir, volume).GetPhysicalStorageUsageAsync(HttpContext.RequestAborted));
		if (quota != long.MaxValue)
			connectorResult.Response = connectorResult.Response switch
									   {
										   InitResponse initResp => new ApplicationInitResponse(initResp)
																    {
																	    Quota = quota,
																	    Usage = storage
																    },
										   OpenResponse openResp => new ApplicationOpenResponse(openResp)
																    {
																	    Quota = quota,
																	    Usage = storage
																    },
										   _ => connectorResult.Response
									   };
	}

	private async Task<(IVolume volume, long quota)> SetupConnectorAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		Volume volume = await SetupVolume(cancellationToken);
		long quota = await SetupQuota(volume, cancellationToken);
		// await SetupEvent(cancellationToken);
		// await SetupAcessControlManagement(volume, cancellationToken);
		await Task.WhenAll(SetupEvent(cancellationToken), SetupAcessControlManagement(volume, cancellationToken));
		_driver.SetupBackgroundThumbnailGenerator(_thumbnailGenerator, _pictureEditor, _videoEditor, cancellationToken: cancellationToken);
		return (volume, quota);
	}

	private async Task<Volume> SetupVolume(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		Volume volume = _elFinderUtilityService.GetVolume(_driver, User);
		_connector.AddVolume(volume);
		await _driver.SetupVolumeAsync(volume, cancellationToken);
		return volume;
	}

	private static Task SetupAcessControlManagement(Volume volume, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		return Task.CompletedTask;
	}

	private Task<long> SetupQuota(Volume volume, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		QuotaOptions quotaOptions = new();
		long quota = _elFinderUtilityService.GetQuota(User);
		quotaOptions.Enabled = User.IsAuthenticated();
		_connector.PluginManager.Features[typeof(QuotaOptions)] = quotaOptions;
		if (quotaOptions.Enabled)
			quotaOptions.Quotas[volume.VolumeId] = new VolumeQuota
												   {
													   VolumeId = volume.VolumeId,
													   MaxStorageSizeInMb = quota / Math.Pow(1024, 2)
												   };
		return Task.FromResult(quota);
	}

	private Task SetupEvent(CancellationToken cancellationToken)
	{
		if (!Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value, out Guid userId))
			userId = Guid.Empty;

		_driver.OnBeforeUpload.Add((_, _, _, _, _) =>
								   {
									   UpdatePulseStatus();
									   return Task.CompletedTask;
								   });
		_driver.OnAfterUpload.Add((file, destFile, _, _, isChunking) =>
								  {
									  if (!isChunking)
									  {
										  _logger.LogDebug("Tải xong: {FullName}", destFile?.FullName);
										  CurrentUploadStatus.UploadedFiles.Add(file.Name);
										  StartUploadDoneChecking(userId);
									  }

									  return Task.CompletedTask;
								  });
		_driver.OnAfterChunkMerged.Add((file, _) =>
									   {
										   _logger.LogDebug("Bắt đầu tải lên: {FullName}", file.FullName ?? string.Empty);
										   CurrentUploadStatus.UploadedFiles.Add(file.Name);
										   StartUploadDoneChecking(userId);
										   return Task.CompletedTask;
									   });
		cancellationToken.ThrowIfCancellationRequested();
		return Task.CompletedTask;
	}
	private void UpdatePulseStatus()
	{
		CurrentUploadStatus.LastPulse = DateTimeOffset.UtcNow;
	}
	private void StartUploadDoneChecking(Guid id)
	{
		UploadPulseModel status = CurrentUploadStatus;
		if (status.Timer is not null)
			return;
		lock (status)
		{
			status.Timer = new Timer(HeartbeatInterval);
			status.Timer.Elapsed += (o, e) =>
									{
										TimeSpan timeSpan = DateTimeOffset.UtcNow - status.LastPulse;
										if (timeSpan.TotalMilliseconds > HeartbeatInterval)
										{
											_logger.LogDebug("{UploadedFilesCount} đã tải lên xong", status.UploadedFiles.Count);
											UploadStatus.Remove(id, out _);
											status.Timer.Stop();
										}
									};
			status.Timer.Start();
		}
	}
}