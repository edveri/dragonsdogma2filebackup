namespace DragonsDogma2FileBackupWorker.Logic;

public class DirectoryAndSettingsFacade(
    IDirectoryStorageBuilder directoryStorageBuilder,
    ISteamLaunchOptionsEditor steamLaunchOptionsEditor,
    IApplicationFileAndDirectoryHelper applicationFileAndDirectoryHelper) : IDirectoryAndSettingsFacade
{
    private readonly IDirectoryStorageBuilder _directoryStorageBuilder = directoryStorageBuilder ?? throw new ArgumentNullException(nameof(directoryStorageBuilder));
    private readonly ISteamLaunchOptionsEditor _steamLaunchOptionsEditor = steamLaunchOptionsEditor ?? throw new ArgumentNullException(nameof(steamLaunchOptionsEditor));
    private readonly IApplicationFileAndDirectoryHelper _applicationFileAndDirectoryHelper = applicationFileAndDirectoryHelper ?? throw new ArgumentNullException(nameof(applicationFileAndDirectoryHelper));
    
    public async Task InitializeAndSetSteamDirectoriesAsync()
    {
        _directoryStorageBuilder.BuildDirectoryStorage();
        await _steamLaunchOptionsEditor.SetSteamLaunchOptionsAsync();
        await _applicationFileAndDirectoryHelper.CreateBackupDirAndBatchFileAsync();
    }
}