using Api.ElFinder.Settings;
using Api.Services;
using elFinder.Net.AspNetCore.Extensions;
using elFinder.Net.Core.Extensions;
using elFinder.Net.Core.Plugins;
using elFinder.Net.Core.Services.Drawing;
using elFinder.Net.Drivers.FileSystem.Extensions;
using elFinder.Net.Drivers.FileSystem.Services;
using elFinder.Net.Plugins.FileSystemQuotaManagement.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace Api.Settings;

public static class ElFinder
{
	public static WebApplicationBuilder AddElFinder(this WebApplicationBuilder builder)
	{
		IWebHostEnvironment env = builder.Environment;
		IServiceCollection services = builder.Services;
		ConfigurationManager configuration = builder.Configuration;

		PluginCollection pluginCollection = new();
		string tempPath = configuration["Settings:ElFinder:TempFileFolderPath"] ?? throw new Exception();
		string storagePath = configuration["Settings:ElFinder:StorageFolderPath"] ?? throw new Exception();
		string urlRoot = configuration["Settings:ElFinder:Url"] ?? throw new Exception();

		tempPath = Path.GetFullPath(tempPath);
		storagePath = Path.GetFullPath(storagePath);

		TaoThuMucNeoKhongTonTai(tempPath);
		TaoThuMucNeoKhongTonTai(storagePath);


		services.AddElFinderAspNetCore()
				.AddFileSystemDriver(typeof(ApplicationFileSystemDriver), tempFileCleanerConfig: opt => opt.ScanFolders.Add(tempPath, TempFileCleanerOptions.DefaultUnmanagedLifeTime))
				.AddFileSystemQuotaManagement(pluginCollection, fileSystemDriverImplType: typeof(ApplicationFileSystemDriver))
				.AddElFinderLogging(pluginCollection)
				.AddElFinderPlugins(pluginCollection)
				.AddSingleton<IVideoEditor, AppVideoEditor>()
				.AddResponseCompression(options =>
									    {
										    options.EnableForHttps = true;
										    options.Providers.Add<GzipCompressionProvider>();
									    })
				.AddSingleton<IElFinderUtilityService, ElFinderUtilityService>()
				.Configure<ElFinderUtilityServiceOptions>(options =>
														  {
															  options.WebRootPath = env.WebRootPath;
															  options.StoragePath = storagePath;
															  options.TempPath = tempPath;
															  options.Url = urlRoot;
														  })
				.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
		return builder;
	}
	private static void TaoThuMucNeoKhongTonTai(string tempPath)
	{
		if (!Directory.Exists(tempPath))
			Directory.CreateDirectory(tempPath);
	}
}