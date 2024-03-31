namespace DragonsDogma2FileBackupWorker.Logic;

public class SteamPathInitializer(IDirectoryStorage directoryStorage, IIoWrapper ioWrapper) : ISteamPathInitializer
{
    private readonly IDirectoryStorage _directoryStorage = directoryStorage ?? throw new ArgumentNullException(nameof(directoryStorage)); 
    private readonly IIoWrapper _ioWrapper = ioWrapper ?? throw new ArgumentNullException(nameof(ioWrapper));
    
    public void InitializeSteamDirectories()
    {
        foreach (var directory in _ioWrapper.GetDirectories(GetSteamUserDataPath()))
        {
            if (!_ioWrapper.GetDirectories(directory)
                    .Any(dirName => dirName.EndsWith(Constants.DragonsDogma2Id)))
            {
                continue;
            }
            
            _directoryStorage.SteamAccountDirectory = directory;
            _directoryStorage.SteamAccountId = new DirectoryInfo(directory).Name;
            _directoryStorage.SteamSaveFileDirectory = _ioWrapper.CombinePath(directory, Constants.DragonsDogma2Id);
            return;
        }
        throw new DirectoryNotFoundException("Could not find Dragons Dogma 2 save path in Steam directory");
    }
    
    private string GetSteamUserDataPath() => 
        _ioWrapper.CombinePath(GetSetSteamRootPath(), Constants.SteamUserDataDirectory);
    
    private string GetSetSteamRootPath()
    {
        if (!_ioWrapper.IsWindowsOs())
        {
            throw new PlatformNotSupportedException("This application only supports Windows");
        }

        var steamPath = Environment.Is64BitOperatingSystem switch
        {
            true => _ioWrapper.GetRegistryKeyValue(Constants.Steam64BitRegistryKeyPath),
            _ => _ioWrapper.GetRegistryKeyValue(Constants.Steam32BitRegistryKeyPath)
        };
        
        if (string.IsNullOrWhiteSpace(steamPath))
        {
            throw new DirectoryNotFoundException("Could not find Steam path in registry");
        }
        _directoryStorage.SteamRootPath = steamPath;
        return steamPath;
    }
}