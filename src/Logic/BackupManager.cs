namespace DragonsDogma2FileBackupWorker.Logic;

public class BackupManager(IDirectoryStorage directoryStorage, IIoWrapper ioWrapper, IOptions<CopyOptions> copyOptions, ILogger<BackupManager> logger) : IBackupManager
{
    private readonly IDirectoryStorage _directoryStorage = directoryStorage ?? throw new ArgumentNullException(nameof(directoryStorage));
    private readonly IIoWrapper _ioWrapper = ioWrapper ?? throw new ArgumentNullException(nameof(ioWrapper));
    private readonly CopyOptions _copyOptions = copyOptions.Value ?? throw new ArgumentNullException(nameof(copyOptions));
    private readonly ILogger<BackupManager> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task ExecuteBackupsAsync()
    {
        var optionsValidationResults = _copyOptions.Validate(null).ToList();

        if (optionsValidationResults.Any())
        {
            throw new ArgumentException("Invalid options", string.Join(Environment.NewLine, optionsValidationResults));
        }

        if (string.IsNullOrWhiteSpace(_directoryStorage.SteamSaveFileDirectory))
        {
            throw new ArgumentException("Steam save file directory is not set", nameof(_directoryStorage.SteamSaveFileDirectory));
        }
        
        while (_ioWrapper.ProcessExists(Constants.DragonsDogma2ProcessName))
        {
            var steamSaveFileDirectory = _directoryStorage.SteamSaveFileDirectory;
            _logger.LogInformation("Copying files from: {SourceDirectory} to {DestinationDirectory}", steamSaveFileDirectory, _copyOptions.DestinationDirectory);
            
            _ioWrapper.CopyDirectory(steamSaveFileDirectory, _ioWrapper.CombinePath(_copyOptions.DestinationDirectory, $"Backup_{DateTime.Now:yyyy-MM-dd HH-mm-ss}"));
            _logger.LogInformation("Files copied at: {Time}", DateTimeOffset.Now); 
            DeleteBackups();
            await Task.Delay(TimeSpan.FromSeconds(_copyOptions.WaitTimeInSeconds));
        }
    }

    private void DeleteBackups()
    {
        var existingBackups = _ioWrapper.GetDirectories(_copyOptions.DestinationDirectory)
            .Select(dir => _ioWrapper.GetDirectoryInfo(dir))
            .Where(dir => dir.Name.StartsWith("Backup_"))
            .OrderBy(dirInfo => dirInfo.CreationTime)
            .ToList();
        
        if( _copyOptions.MaximumNumberOfBackups > 0 && existingBackups.Count > _copyOptions.MaximumNumberOfBackups)
        {
            var backupsToDelete = existingBackups.Take(existingBackups.Count - _copyOptions.MaximumNumberOfBackups);
            foreach (var backup in backupsToDelete)
            {
                _logger.LogInformation("Deleting backup: {Backup}", backup.Name);
                backup.Delete(true);
            }
        }
    }
}