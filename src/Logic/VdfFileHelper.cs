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
            if (lines[i].Contains(Constants.DragonsDogma2Id, StringComparison.OrdinalIgnoreCase) && lines[i + 1].Contains("{", StringComparison.OrdinalIgnoreCase))
            {
                startIndexOfSection = i;

                // Check if LaunchOptions already exists. We're assuming that it should be withing 20 lines of the start of the section
                for (var j = startIndexOfSection;  j < i+ maxNumberOfLinesToSearch; j++)
                {
                    if(lines[j].Contains("LaunchOptions", StringComparison.OrdinalIgnoreCase))
                    {
                        launchOptionsFound = true;
                        launchOptionsIndex = j;
                        break;
                    }
                }

                // No need to continue the loop once the section is found
                if (launchOptionsFound || lines.Skip(i).Take(maxNumberOfLinesToSearch).Any(x => x.Contains("2054970_eula") || x.Contains("Playtime")))
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
            configFileLines.Insert(configFileData.StartIndex+2, launchOptionsString);
        }
    }
}