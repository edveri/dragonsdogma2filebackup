namespace DragonsDogma2FileBackupWorker.Logic;

public class VdfFileHelper : IVdfFileHelper
{
    public LocalConfigFileData GetStartAndEndIndexOfSection(IList<string> lines)
    {
        const int maxNumberOfLinesToSearch = 20;

        var startIndexOfSection = -1;
        var launchOptionsFound = false;
        var launchOptionsIndex = -1;
        
        for (var i = 0; i < lines.Count - 1; i++)
        {
            if (lines[i].Contains(Constants.DragonsDogma2Id) && lines[i + 1].Contains("{"))
            {
                var potentialSectionLines = lines.Skip(i).Take(maxNumberOfLinesToSearch).ToList();
                
                if (potentialSectionLines.Any(x => x.Contains("2054970_eula")) || potentialSectionLines.Any(x => x.Contains("Playtime")))
                {
                    startIndexOfSection = i;

                    // Check if LaunchOptions already exists. We're assuming that it should be withing 20 lines of the start of the section
                    for (var j = startIndexOfSection;  j < i+ maxNumberOfLinesToSearch; j++)
                    {
                        if(lines[j].Contains("LaunchOptions", StringComparison.Ordinal) && lines[j+1].Contains("}", StringComparison.Ordinal))
                        {
                            launchOptionsFound = true;
                            launchOptionsIndex = j;
                            break;
                        }
                    }

                    // No need to continue the loop once the section is found
                    break;
                }
            }
        }
        return new LocalConfigFileData(startIndexOfSection, launchOptionsFound, launchOptionsIndex);
    }

    public void UpdateSteamLaunchConfig(LocalConfigFileData configFileData, List<string> configFileLines,
        string launchOptionsString)
    {
        switch (configFileData.LaunchOptionsExists)
        {
            case false:
                configFileLines.Insert(configFileData.StartIndex+2, launchOptionsString);
                break;
            default:
                configFileLines[configFileData.LaunchOptionsIndex] = launchOptionsString;
                break;
        }
    }
}