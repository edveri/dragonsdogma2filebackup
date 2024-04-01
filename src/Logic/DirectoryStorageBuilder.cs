using DragonsDogma2FileBackupWorker.Models.Abstract;

namespace DragonsDogma2FileBackupWorker.Logic;

public class DirectoryStorageBuilder(ISteamPathInitializer steamPathInitializer, IDirectoryStorage directoryStorage) : IDirectoryStorageBuilder
{
    private readonly ISteamPathInitializer _steamPathInitializer = steamPathInitializer ?? throw new ArgumentNullException(nameof(steamPathInitializer));
    private readonly IDirectoryStorage _directoryStorage =
        directoryStorage ?? throw new ArgumentNullException(nameof(directoryStorage));
    
    public void BuildDirectoryStorage()
    {
        _steamPathInitializer.InitializeSteamDirectories();
        var validationErrors = _directoryStorage.Validate(null!);
        
        if (validationErrors.Any())
        {
            throw new ApplicationException(string.Join(", ", _directoryStorage.Validate(null!)));
        }
    }
}