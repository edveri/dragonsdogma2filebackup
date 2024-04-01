using DragonsDogma2FileBackupWorker.Models.Abstract;

namespace DragonsDogma2FileBackupWorker.Logic;

public class SteamLaunchOptionsEditor(IVdfFileHelper vdfFileHelper, IDirectoryStorage directoryStorage, IIoWrapper ioWrapper) : ISteamLaunchOptionsEditor
{
    private readonly IVdfFileHelper _vdfFileHelper = vdfFileHelper ?? throw new ArgumentNullException(nameof(vdfFileHelper));
    private readonly IDirectoryStorage _directoryStorage = directoryStorage ?? throw new ArgumentNullException(nameof(directoryStorage));
    private readonly IIoWrapper _ioWrapper = ioWrapper ?? throw new ArgumentNullException(nameof(ioWrapper));

    public async Task SetSteamLaunchOptionsAsync()
    {
        //Get the path to Steam's vdf config file. This file contains the launch options for the game. 
        var steamConfigFileDirectory = _ioWrapper.CombinePath(_directoryStorage.SteamAccountDirectory, Constants.ConfigDirectoryName);
        var steamConfigFileFullPath = _ioWrapper.CombinePath(steamConfigFileDirectory, Constants.LocalConfigFileName);
        
        BackupExistingConfigFile(steamConfigFileFullPath, steamConfigFileDirectory);
        
        var launchOptionsString = Constants.LaunchOptionsText.Replace(Constants.LaunchOptionsDirectoryKey,
            _ioWrapper.CombinePath(
                _directoryStorage.SteamRootPath, 
                Constants.SteamAppsDirectory, 
                Constants.BatchFileName)
                .Replace("\\", "\\\\"));
        
        var steamConfigFileContent = (await _ioWrapper.ReadAllLinesAsync(steamConfigFileFullPath)).ToList();
        var steamConfigFileData = _vdfFileHelper.GetStartAndEndIndexOfSection(steamConfigFileContent);

        //Update steam vdf config file. If launch options for DD2 are already present, they will be updated. If not, they will be added.
        _vdfFileHelper.UpdateSteamLaunchConfig(steamConfigFileData, steamConfigFileContent, launchOptionsString);

        await _ioWrapper.WriteAllLinesAsync(steamConfigFileFullPath, steamConfigFileContent);
    }
    
    private void BackupExistingConfigFile(string configFilePath, string configDirectory) =>
        _ioWrapper.Copy(configFilePath, _ioWrapper.CombinePath(configDirectory, $"{Constants.LocalConfigFileName}_backup{DateTime.Now:hh-mm-ss}"));
}