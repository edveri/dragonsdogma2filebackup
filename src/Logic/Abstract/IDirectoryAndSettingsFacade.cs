namespace DragonsDogma2FileBackupWorker.Logic.Abstract;

public interface IDirectoryAndSettingsFacade
{
    Task InitializeAndSetSteamDirectoriesAsync();
}