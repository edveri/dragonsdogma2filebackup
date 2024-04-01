namespace DragonsDogma2FileBackupWorker.Logic;

public class VdfFileHelper : IVdfFileHelper
{
    public LocalConfigFileData GetStartAndEndIndexOfSection(IList<string> lines)
    {
        const int maxNumberOfLinesToSearch = 17;

        var startIndexOfSection = -1;
        var dd2SectionFound = false;
        var launchOptionsFound = false;
        var launchOptionsIndex = -1;
        
        for (var i = 0; i < lines.Count - 1; i++)
        {
            if (!IsDragonsDogmaRoot(lines, i))
            {
                continue;
            }
            
            startIndexOfSection = i;
                
            // Check if LaunchOptions already exists. We're assuming that it should be within 17 lines of the start of the section
            for (var j = startIndexOfSection; j < startIndexOfSection + maxNumberOfLinesToSearch; j++)
            {
                if (IsLaunchSettingsLine(lines, j))
                {
                    launchOptionsFound = true;
                    launchOptionsIndex = j;
                    break;
                }

                if (IsDd2Section(lines, j))
                {
                    dd2SectionFound = true;
                }
            }

            // No need to continue the loop once the section is found
            if (launchOptionsFound || dd2SectionFound) 
            {
                break;
            }
        }
        return new LocalConfigFileData(startIndexOfSection, launchOptionsFound, launchOptionsIndex);
    }
    
    public void UpdateSteamLaunchConfig(LocalConfigFileData configFileData, IList<string> configFileLines,
        string launchOptionsString)
    {
        if (configFileData.LaunchOptionsExists)
        {
            configFileLines[configFileData.LaunchOptionsIndex] = launchOptionsString;
        }
        else
        {
            configFileLines.Insert(configFileData.StartIndex + 2, launchOptionsString);
        }
    }
    
    private static bool IsDragonsDogmaRoot(IList<string> lines, int i) => 
        lines[i].Contains(Constants.DragonsDogma2Id, StringComparison.OrdinalIgnoreCase) && lines[i + 1].Contains(Constants.StartBracket, StringComparison.OrdinalIgnoreCase);

    //Check if the section contains an EULA or Playtime string - if so we assume it's the DD2 section
    private static bool IsDd2Section(IList<string> lines, int j) =>
        lines[j].Contains(Constants.LocalConfigEulaString, StringComparison.OrdinalIgnoreCase) ||
        lines[j].Contains(Constants.LocalConfigPlaytimeString, StringComparison.OrdinalIgnoreCase);

    private static bool IsLaunchSettingsLine(IList<string> lines, int j) => 
        lines[j].Contains(Constants.LaunchOptionsSectionRoot, StringComparison.OrdinalIgnoreCase);
}