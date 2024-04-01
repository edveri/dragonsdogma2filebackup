namespace DragonsDogma2FileBackupWorker.Models;

public class DirectoryStorage : IDirectoryStorage
{
    public string SteamAccountDirectory { get; set; } = string.Empty;
    public string SteamSaveFileDirectory { get; set; } = string.Empty;
    public string SteamAccountId { get; set; } = string.Empty;
    public string SteamRootPath { get; set; } = string.Empty;
    public IEnumerable<ValidationResult> Validate(ValidationContext? validationContext)
    {
        if (string.IsNullOrWhiteSpace(SteamAccountDirectory))
        {
            yield return new ValidationResult("Steam account directory is not set", new[] { nameof(SteamAccountDirectory) });
        }
        
        if (string.IsNullOrWhiteSpace(SteamSaveFileDirectory))
        {
            yield return new ValidationResult("Steam save file directory is not set", new[] { nameof(SteamSaveFileDirectory) });
        }
        
        if (string.IsNullOrWhiteSpace(SteamAccountId))
        {
            yield return new ValidationResult("Steam account id is not set", new[] { nameof(SteamAccountId) });
        }
        
        if (string.IsNullOrWhiteSpace(SteamRootPath))
        {
            yield return new ValidationResult("Steam root path is not set", new[] { nameof(SteamRootPath) });
        }
    }
}