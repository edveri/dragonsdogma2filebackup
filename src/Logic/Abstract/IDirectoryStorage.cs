namespace DragonsDogma2FileBackupWorker.Logic.Abstract;

public interface IDirectoryStorage : IValidatableObject
{ 
    string SteamAccountDirectory { get; set; }
    string SteamSaveFileDirectory { get; set; }
    string SteamAccountId { get; set; }
    string SteamRootPath { get; set; }
}