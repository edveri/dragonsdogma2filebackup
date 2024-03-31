using DragonsDogma2FileBackupWorker.Logic.Abstract;

namespace DragonsDogma2FileBackupWorker.Logic;

public class Worker(ISteamPathInitializer steamPathInitializer, 
    IApplicationFileAndDirectoryHelper applicationFileAndDirectoryHelper, 
    ILaunchOptionsEditor launchOptionsEditor, 
    ICopySaveFiles copySaveFiles,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly ISteamPathInitializer _steamPathInitializer = steamPathInitializer ?? throw new ArgumentNullException(nameof(steamPathInitializer));
    private readonly IApplicationFileAndDirectoryHelper _applicationFileAndDirectoryHelper = applicationFileAndDirectoryHelper ?? throw new ArgumentNullException(nameof(applicationFileAndDirectoryHelper));
    private readonly ILaunchOptionsEditor _launchOptionsEditor = launchOptionsEditor ?? throw new ArgumentNullException(nameof(launchOptionsEditor));
    private readonly ICopySaveFiles _copySaveFiles = copySaveFiles ?? throw new ArgumentNullException(nameof(copySaveFiles));
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
    
    protected override async Task ExecuteAsync(CancellationToken _)
    {
        _steamPathInitializer.InitializeSteamDirectories();
        await _launchOptionsEditor.SetSteamLaunchOptionsAsync();
        await _applicationFileAndDirectoryHelper.CreateBackupDirAndBatchFileAsync();
        await _copySaveFiles.CopyFilesAsync();
        _hostApplicationLifetime.StopApplication();
    }
}