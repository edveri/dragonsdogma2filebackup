namespace DragonsDogma2FileBackupWorker.Logic;

public class LaunchOptionsEditor(IVdfFileHelper vdfFileHelper, IDirectoryStorage directoryStorage, IIoWrapper ioWrapper) : ILaunchOptionsEditor
{
    private readonly IVdfFileHelper _vdfFileHelper = vdfFileHelper ?? throw new ArgumentNullException(nameof(vdfFileHelper));
    private readonly IDirectoryStorage _directoryStorage = directoryStorage ?? throw new ArgumentNullException(nameof(directoryStorage));
    private readonly IIoWrapper _ioWrapper = ioWrapper ?? throw new ArgumentNullException(nameof(ioWrapper));

    public async Task SetSteamLaunchOptionsAsync()
    {
        ValidateSteamAccountDirectory();

        var configDirectory = Path.Combine(_directoryStorage.SteamAccountDirectory, Constants.ConfigDirectoryName);
        var configFilePath = Path.Combine(configDirectory, Constants.LocalConfigFileName);

        BackupConfigFile(configFilePath, configDirectory);

        var launchOptionsString = Constants.LaunchOptionsText.Replace(Constants.LaunchOptionsDirectoryKey,
            Path.Combine(_directoryStorage.SteamRootPath, Constants.SteamAppsDirectory, Constants.BatchFileName));

        var configFileLines = (await _ioWrapper.ReadAllLinesAsync(configFilePath)).ToList();
        var configFileData = _vdfFileHelper.GetStartAndEndIndexOfSection(configFileLines);

        _vdfFileHelper.UpdateSteamLaunchConfig(configFileData, configFileLines, launchOptionsString);

        await _ioWrapper.WriteAllLinesAsync(configFilePath, configFileLines);
    }
    
    private void BackupConfigFile(string configFilePath, string configDirectory) =>
        _ioWrapper.Copy(configFilePath, Path.Combine(configDirectory, $"{Constants.LocalConfigFileName}_backup{DateTime.Now:hh-mm-ss}"));

    private void ValidateSteamAccountDirectory()
    {
        if (string.IsNullOrWhiteSpace(_directoryStorage.SteamAccountDirectory))
        {
            throw new ArgumentException("Steam account directory is not set.");
        }
    }
}