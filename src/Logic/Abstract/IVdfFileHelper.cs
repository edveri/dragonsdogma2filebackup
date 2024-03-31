using DragonsDogma2FileBackupWorker.Models;

namespace DragonsDogma2FileBackupWorker.Logic.Abstract;

public interface IVdfFileHelper
{
    LocalConfigFileData GetStartAndEndIndexOfSection(IList<string> lines);
    
    public void UpdateSteamLaunchConfig(LocalConfigFileData configFileData, List<string> configFileLines, string launchOptionsString);
}