using DragonsDogma2FileBackupWorker.Logic.Abstract;

namespace DragonsDogma2FileBackupWorker.Logic;

public class DirectoryStorage : IDirectoryStorage
{
    public string SteamAccountDirectory { get; set; } = string.Empty;
    public string SteamSaveFileDirectory { get; set; } = string.Empty;
    public string SteamAccountId { get; set; } = string.Empty;
    public string SteamRootPath { get; set; } = string.Empty;
}