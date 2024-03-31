namespace DragonsDogma2FileBackupWorker.Logic;

public class VdfFileHelper : IVdfFileHelper
{
    public LocalConfigFileData GetStartAndEndIndexOfSection(IList<string> lines)
    {
        const int maxNumberOfLinesToSearch = 20;

        var startIndexOfSection = -1;
        var sectionsFound = false;
        var launchOptionsFound = false;
        var launchOptionsIndex = -1;

        for (var i = 0; i < lines.Count - 1; i++)
        {
            if (lines[i].Contains(Constants.DragonsDogma2Id) && lines[i + 1].Contains("{") && lines[i + 2].Contains("2054970_eula"))
            {
                sectionsFound = true;
                startIndexOfSection = i;

                // Check if LaunchOptions already exists. We're assuming that it should be withing 20 lines of the start of the section
                for (var j = startIndexOfSection;  j < i+ maxNumberOfLinesToSearch; j++)
                {
                    if(lines[j].Contains("LaunchOptions") && lines[j+1].Contains("}"))
                    {
                        launchOptionsFound = true;
                        launchOptionsIndex = j;
                        break;
                    }
                }
            }

            if (sectionsFound)
            {
                break;
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