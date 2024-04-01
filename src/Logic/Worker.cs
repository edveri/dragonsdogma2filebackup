namespace DragonsDogma2FileBackupWorker.Logic;

public class Worker(IDirectoryAndSettingsFacade directoryAndSettingsFacade,
    ICopySaveFiles copySaveFiles,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly IDirectoryAndSettingsFacade _directoryAndSettingsFacade = directoryAndSettingsFacade ?? throw new ArgumentNullException(nameof(directoryAndSettingsFacade));
    private readonly ICopySaveFiles _copySaveFiles = copySaveFiles ?? throw new ArgumentNullException(nameof(copySaveFiles));
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
    
    protected override async Task ExecuteAsync(CancellationToken _)
    {
        await _directoryAndSettingsFacade.InitializeAndSetSteamDirectoriesAsync();
        await _copySaveFiles.CopyFilesAsync();
        _hostApplicationLifetime.StopApplication();
    }
}