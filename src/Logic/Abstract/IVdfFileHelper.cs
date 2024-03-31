namespace DragonsDogma2FileBackupWorker.Logic.Abstract;

public interface IVdfFileHelper
{
    LocalConfigFileData GetStartAndEndIndexOfSection(IList<string> lines);
    
    public void UpdateSteamLaunchConfig(LocalConfigFileData configFileData, IList<string> configFileLines, string launchOptionsString);
}