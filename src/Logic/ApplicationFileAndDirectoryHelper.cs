namespace DragonsDogma2FileBackupWorker.Logic;

public class ApplicationFileAndDirectoryHelper(
    IDirectoryStorage directoryStorage,
    IIoWrapper ioWrapper,
    IOptions<CopyOptions> copyOptions,
    ILogger<ApplicationFileAndDirectoryHelper> logger)
    : IApplicationFileAndDirectoryHelper
{
    private readonly IDirectoryStorage _directoryStorage = directoryStorage ?? throw new ArgumentNullException(nameof(directoryStorage));
    private readonly IIoWrapper _ioWrapper = ioWrapper ?? throw new ArgumentNullException(nameof(ioWrapper));
    private readonly CopyOptions _copyOptions = copyOptions.Value ?? throw new ArgumentNullException(nameof(copyOptions));
    private readonly ILogger<ApplicationFileAndDirectoryHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    
    public async Task CreateBackupDirAndBatchFileAsync()
    {
        if(string.IsNullOrWhiteSpace(_directoryStorage.SteamRootPath))
        {
            throw new ArgumentException("Steam root path is not set.");
        }

        CreateDestinationDirectory();

        var batchFileContent = Constants.BatchFileContent.Replace(Constants.BatchFileWorkingDirectoryKey, AppDomain.CurrentDomain.BaseDirectory);
        _logger.LogInformation("Writing batch file content {Content}", batchFileContent);
        await _ioWrapper.WriteAllTextAsync(Path.Combine(_directoryStorage.SteamRootPath, Constants.SteamAppsDirectory, Constants.BatchFileName), batchFileContent);
        _logger.LogInformation("Batch file created");
    }

    private void CreateDestinationDirectory()
    {
        if (_ioWrapper.DirectoryExists(_copyOptions.DestinationDirectory))
        {
            return;
        }
        _logger.LogInformation("Backup Directory created: {Directory}", _copyOptions.DestinationDirectory);
        _logger.LogInformation("Creating directory: {Directory}", _copyOptions.DestinationDirectory);
        _ioWrapper.CreateDirectory(_copyOptions.DestinationDirectory);
    }
}