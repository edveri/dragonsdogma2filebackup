namespace DragonsDogma2FileBackupWorker.Logic;

public class DirectoryAndSettingsFacade(
    IDirectoryStorageBuilder directoryStorageBuilder,
    ILaunchOptionsEditor launchOptionsEditor,
    IApplicationFileAndDirectoryHelper applicationFileAndDirectoryHelper) : IDirectoryAndSettingsFacade
{
    private readonly IDirectoryStorageBuilder _directoryStorageBuilder = directoryStorageBuilder ?? throw new ArgumentNullException(nameof(directoryStorageBuilder));
    private readonly ILaunchOptionsEditor _launchOptionsEditor = launchOptionsEditor ?? throw new ArgumentNullException(nameof(launchOptionsEditor));
    private readonly IApplicationFileAndDirectoryHelper _applicationFileAndDirectoryHelper = applicationFileAndDirectoryHelper ?? throw new ArgumentNullException(nameof(applicationFileAndDirectoryHelper));
    
    public async Task InitializeAndSetSteamDirectoriesAsync()
    {
        _directoryStorageBuilder.BuildDirectoryStorage();
        await _launchOptionsEditor.SetSteamLaunchOptionsAsync();
        await _applicationFileAndDirectoryHelper.CreateBackupDirAndBatchFileAsync();
    }
}