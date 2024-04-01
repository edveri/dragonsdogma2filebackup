namespace DragonsDogma2FileBackupWorker.Logic.Abstract;

public interface IVdfFileHelper
{
    SteamConfigFileInfo GetStartAndEndIndexOfSection(IList<string> fileContentLines);
    
    public void UpdateSteamLaunchConfig(SteamConfigFileInfo configFileInfo, IList<string> configFileLines, string launchOptionsString);
}