namespace DragonsDogma2FileBackupWorker.Logic;

public class Worker(IDirectoryAndSettingsFacade directoryAndSettingsFacade,
    BackupManager backupManager,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly IDirectoryAndSettingsFacade _directoryAndSettingsFacade = directoryAndSettingsFacade ?? throw new ArgumentNullException(nameof(directoryAndSettingsFacade));
    private readonly BackupManager _backupManager = backupManager ?? throw new ArgumentNullException(nameof(backupManager));
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
    
    protected override async Task ExecuteAsync(CancellationToken _)
    {
        await _directoryAndSettingsFacade.InitializeAndSetSteamDirectoriesAsync();
        await _backupManager.ExecuteBackupsAsync();
        _hostApplicationLifetime.StopApplication();
    }
}