namespace DragonsDogma2FileBackupWorker.Models;

public class DirectoryStorage : IDirectoryStorage
{
    public string SteamAccountDirectory { get; set; } = string.Empty;
    public string SteamSaveFileDirectory { get; set; } = string.Empty;
    public string SteamAccountId { get; set; } = string.Empty;
    public string SteamRootPath { get; set; } = string.Empty;
}