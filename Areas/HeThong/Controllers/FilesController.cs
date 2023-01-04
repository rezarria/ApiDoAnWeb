using Api.Areas.HeThong.Models;
using elFinder.Net.AspNetCore.Extensions;
using elFinder.Net.AspNetCore.Helper;
using elFinder.Net.Core;
using elFinder.Net.Core.Models.Command;
using elFinder.Net.Core.Models.Response;
using elFinder.Net.Core.Models.Result;
using elFinder.Net.Core.Services.Drawing;
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

	private static readonly ConcurrentDictionary<string, UploadPulseModel> UploadStatus = new();

	private readonly IConnector _connector;
	private readonly IDriver _driver;
	private readonly IPictureEditor _pictureEditor;
	private readonly IStorageManager _storageManager;
	private readonly IThumbnailBackgroundGenerator _thumbnailGenerator;
	private readonly IVideoEditor _videoEditor;

	public FilesController(IConnector connector,
						   IDriver driver,
						   IThumbnailBackgroundGenerator thumbnailGenerator,
						   IPictureEditor pictureEditor,
						   IVideoEditor videoEditor,
						   IStorageManager storageManager)
	{
		_connector = connector;
		_driver = driver;
		_thumbnailGenerator = thumbnailGenerator;
		_pictureEditor = pictureEditor;
		_videoEditor = videoEditor;
		_storageManager = storageManager;
	}

	private UploadPulseModel CurrentUploadStatus => UploadStatus.GetOrAdd(User.Identity?.Name, valueFactory: key => new UploadPulseModel
																											        {
																												        UploadedFiles = new List<string>()
																											        });

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

		var fullPath = Startup.MapStoragePath(path);

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
		(long Storage, bool IsInit) storageCache = _storageManager.GetOrCreateDirectoryStorage(volume.RootDirectory,
																							   createFunc: dir => volume.Driver.CreateDirectory(dir, volume).GetPhysicalStorageUsageAsync(HttpContext.RequestAborted));

		if (connectorResult.Response is InitResponse initResp)
			connectorResult.Response = new ApplicationInitResponse(initResp)
									   {
										   quota = quota,
										   usage = storageCache.Storage
									   };
		else if (connectorResult.Response is OpenResponse openResp)
			connectorResult.Response = new ApplicationOpenResponse(openResp)
									   {
										   quota = quota,
										   usage = storageCache.Storage
									   };
	}

	private async Task<(IVolume Volume, long Quota)> SetupConnectorAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		int userId = int.Parse(User.Identity.Name);
		var user = _dataContext.Users.Single(o => o.Id == userId);
		var volumePath = user.VolumePath;
		var quota = user.QuotaInBytes;

		// Quota management: The 2 line belows is setup once per request
		QuotaOptions quotaOptions = new()
									{ Enabled = true };
		_connector.PluginManager.Features[typeof(QuotaOptions)] = quotaOptions;

		// Volume initialization
		Volume volume = new(_driver,
						    Startup.MapStoragePath($"./upload/{volumePath}"),
						    Startup.TempPath,
						    $"/api/files/storage/upload/{volumePath}/",
						    "/api/files/thumb/",
						    thumbnailDirectory: Startup.MapStoragePath($"./thumb/{volumePath}"))
						{
							Name = "My volume",
							MaxUploadFiles = 20,
							MaxUploadSizeInMb = 10,
							MaxUploadConnections = 3,// 3 upload requests at a time
							//UploadAllow = new[] { "text" },
							UploadDeny = new[] { "image", "text/csv" },
							UploadOrder = new[] { UploadConstraintType.Allow, UploadConstraintType.Deny }
						};

		_connector.AddVolume(volume);
		await _driver.SetupVolumeAsync(volume, cancellationToken);

		// If video thumbnail is used, we may want to run the process in background.
		// The driver package has a built-in support for this.
		//_driver.SetupBackgroundThumbnailGenerator(_thumbnailGenerator, _pictureEditor, _videoEditor, cancellationToken: cancellationToken);

		// Events
		_driver.OnBeforeUpload.Add((file, destFile, formFile, isOverwrite, isChunking) =>
								   {
									   UpdatePulseStatus();
									   return Task.CompletedTask;
								   });

		_driver.OnAfterUpload.Add((file, destFile, formFile, isOverwrite, isChunking) =>
								  {
									  if (!isChunking)
									  {
										  Console.WriteLine($"Uploaded to: {destFile?.FullName}");
										  UploadPulseModel status = CurrentUploadStatus;
										  status.UploadedFiles.Add(file.Name);
										  StartUploadDoneChecking();
									  }

									  return Task.CompletedTask;
								  });

		_driver.OnAfterChunkMerged.Add((file, isOverwrite) =>
									   {
										   Console.WriteLine($"Uploaded to: {file?.FullName}");
										   UploadPulseModel status = CurrentUploadStatus;
										   status.UploadedFiles.Add(file.Name);
										   StartUploadDoneChecking();
										   return Task.CompletedTask;
									   });

		// Quota management: This is set up per volume. Use VolumeId as key.
		// The plugin support quota management on Volume (root) level only. It means that you can not set quota for directories.
		quotaOptions.Quotas[volume.VolumeId] = new VolumeQuota
											   {
												   VolumeId = volume.VolumeId,
												   MaxStorageSizeInMb = quota / Math.Pow(1024, 2)
												   //MaxStorageSizeInKb = quota / 1024,
												   //MaxStorageSize = quota,
											   };

		#region Access Control Management

		string limitedFolder = $"{volume.RootDirectory}{volume.DirectorySeparatorChar}limited";
		string haloFile = $"{volume.RootDirectory}{volume.DirectorySeparatorChar}halo.txt";
		string adminArea = $"{volume.RootDirectory}{volume.DirectorySeparatorChar}admin-area";

		volume.ObjectAttributes = new List<FilteredObjectAttribute>
								  {
									  // You can implement your own logic to modify Physical File attributes to maintain the Access Control attributes
									  // even if the files are moved
									  new()
									  {
										  Expression = "file.attributes.readonly",// Example only
										  ObjectFilter = obj => (int)obj.Attributes != -1 && obj.Attributes.HasFlag(FileAttributes.ReadOnly),
										  Write = false
									  },
									  new()
									  {
										  Expression = "file.attributes.hidden",// Example only
										  ObjectFilter = obj => (int)obj.Attributes != -1 && obj.Attributes.HasFlag(FileAttributes.Hidden),
										  Visible = false
									  },

									  // Recommended: If the parent is limited, then its children should be too.
									  new()
									  {
										  Expression = $"dir.fullname = '{adminArea}'//children",// Example only
										  DirectoryFilter = dir => dir.FullName == adminArea,
										  ObjectFilter = obj => obj.IsChildOfAsync(adminArea).Result,
										  Access = false
									  },

									  // More examples
									  new()
									  {
										  Expression = $"obj.fullname = '{limitedFolder}'",// Example only
										  ObjectFilter = obj => obj.FullName == limitedFolder,
										  Locked = true, Read = false, Write = false
									  },
									  new()
									  {
										  Expression = $"obj.fullname = '{limitedFolder}'",// Example only
										  ObjectFilter = obj => obj.FullName == limitedFolder,
										  Locked = true, Read = false, Write = false
									  },
									  new()
									  {
										  Expression = $"obj.fullname = '{haloFile}'",// Example only
										  ObjectFilter = obj => obj.FullName == haloFile,
										  Locked = true, Write = false
									  },
									  new()
									  {
										  Expression = "file.name.startsWith = 'secrets_'",// Example only
										  FileFilter = file => file.Name.StartsWith("secrets_"),
										  Locked = true, Write = false, Visible = false
									  },
									  new()
									  {
										  Expression = "file.mime = 'somemime-type'",// Example only
										  FileFilter = file => file.MimeType == "somemime-type",
										  Locked = true, Write = false
									  },
									  new()
									  {
										  Expression = "file.ext = 'exe'",// Example only
										  FileFilter = file => file.Extension == ".exe",
										  Write = false, ShowOnly = true, Read = false
									  },
									  new()
									  {
										  Expression = "dir.parent.name = 'locked'",// Example only
										  DirectoryFilter = dir => dir.Parent.Name == "locked",
										  Locked = true
									  },
									  new()
									  {
										  Expression = "dir.name = 'access-denied'",// Example only
										  DirectoryFilter = dir => dir.Name == "access-denied",
										  Access = false
									  }
								  };

		#endregion

		return (volume, quota);
	}

	private void UpdatePulseStatus()
	{
		UploadPulseModel status = CurrentUploadStatus;

		status.LastPulse = DateTimeOffset.UtcNow;
	}

	private void StartUploadDoneChecking()
	{
		string? userId = User.Identity.Name;
		UploadPulseModel status = CurrentUploadStatus;

		lock (status)
		{
			if (status.Timer == null)
			{
				status.Timer = new Timer(HeartbeatInterval);
				status.Timer.Elapsed += (o, e) =>
										{
											TimeSpan timeSpan = DateTimeOffset.UtcNow - status.LastPulse;
											if (timeSpan.TotalMilliseconds > HeartbeatInterval)
											{
												Console.WriteLine($"{status.UploadedFiles.Count()} uploaded.");
												UploadStatus.Remove(userId, out _);
												status.Timer.Stop();
											}
										};
				status.Timer.Start();
			}
		}
	}
}