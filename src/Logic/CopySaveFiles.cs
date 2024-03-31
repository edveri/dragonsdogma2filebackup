using DragonsDogma2FileBackupWorker.Logic.Abstract;
using DragonsDogma2FileBackupWorker.Logic.IO.Abstract;

namespace DragonsDogma2FileBackupWorker.Logic;

public class CopySaveFiles(IDirectoryStorage directoryStorage, IIoWrapper ioWrapper, IOptions<CopyOptions> copyOptions, ILogger<CopySaveFiles> logger) : ICopySaveFiles
{
    private readonly IDirectoryStorage _directoryStorage = directoryStorage ?? throw new ArgumentNullException(nameof(directoryStorage));
    private readonly IIoWrapper _ioWrapper = ioWrapper ?? throw new ArgumentNullException(nameof(ioWrapper));
    private readonly CopyOptions _copyOptions = copyOptions.Value ?? throw new ArgumentNullException(nameof(copyOptions));
    private readonly ILogger<CopySaveFiles> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task CopyFilesAsync()
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
        
        while (_ioWrapper.ProcessExists(Constants.DraginsDogma2ProcessName))
        {
            var steamSaveFileDirectory = _directoryStorage.SteamSaveFileDirectory;
            _logger.LogInformation("Copying files from: {SourceDirectory} to {DestinationDirectory}", steamSaveFileDirectory, _copyOptions.DestinationDirectory);
            
            _ioWrapper.CopyDirectory(steamSaveFileDirectory, _ioWrapper.CombinePath(_copyOptions.DestinationDirectory, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
            _logger.LogInformation("Files copied at: {Time}", DateTimeOffset.Now);
            
            await Task.Delay(TimeSpan.FromSeconds(_copyOptions.WaitTimeInSeconds));
        }
    }
}