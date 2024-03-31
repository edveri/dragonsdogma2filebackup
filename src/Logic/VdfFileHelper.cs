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
            if (lines[i].Contains(Constants.DragonsDogma2Id, StringComparison.OrdinalIgnoreCase) && lines[i + 1].Contains('{', StringComparison.OrdinalIgnoreCase))
            {
                startIndexOfSection = i;
                
                // Check if LaunchOptions already exists. We're assuming that it should be within 17 lines of the start of the section
                for (var j = startIndexOfSection; j < startIndexOfSection + maxNumberOfLinesToSearch; j++)
                {
                    if (lines[j].Contains("LaunchOptions", StringComparison.OrdinalIgnoreCase))
                    {
                        launchOptionsFound = true;
                        launchOptionsIndex = j;
                        break;
                    }

                    if (lines[j].Contains("2054970_eula", StringComparison.OrdinalIgnoreCase) ||
                        lines[j].Contains("Playtime", StringComparison.OrdinalIgnoreCase))
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
        }
        return new LocalConfigFileData(startIndexOfSection, launchOptionsFound, launchOptionsIndex);
    }

    public void UpdateSteamLaunchConfig(LocalConfigFileData configFileData, List<string> configFileLines,
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
}