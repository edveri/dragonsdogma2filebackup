namespace DragonsDogma2FileBackupWorker.Logic;

public class VdfFileHelper : IVdfFileHelper
{
    public SteamConfigFileInfo GetStartAndEndIndexOfSection(IList<string> fileContentLines)
    {
        var sectionStartIndex = -1;
        var launchOptionsIndex = -1;
        var launchOptionsFound = false;
        
        var lastPossibleLineForSectionStart = fileContentLines.Count - 2 - Constants.MaxNumberOfConfigFileLinesToSearch;
        
        for (var i = 0; i < lastPossibleLineForSectionStart; i++)
        {
            if (!IsDragonsDogmaRoot(fileContentLines[i], fileContentLines[i + 1]))
            {
                continue;
            }
            
            sectionStartIndex = i;
                
            // Check if LaunchOptions already exists. We're assuming that it should be within 17 lines of the start of the section. The number 17 is ... a guess..
            (launchOptionsFound, launchOptionsIndex, var dragonsDogmaSectionFound) = CheckForLaunchOptionsAndDd2Section(fileContentLines, sectionStartIndex);

            // No need to continue the loop once the section is found
            if (launchOptionsFound || dragonsDogmaSectionFound) 
            {
                break;
            }
        }
        
        ThrowExceptionIfStartSectionNotFound(sectionStartIndex);
        
        return new SteamConfigFileInfo(sectionStartIndex, launchOptionsFound, launchOptionsIndex);
    }

    
    public void UpdateSteamLaunchConfig(SteamConfigFileInfo configFileInfo, IList<string> configFileLines,
        string launchOptionsString)
    {
        if (configFileInfo.LaunchOptionsExists)
        {
            configFileLines[configFileInfo.LaunchOptionsIndex] = launchOptionsString;
        }
        else
        {
            configFileLines.Insert(configFileInfo.StartIndex + 2, launchOptionsString);
        }
    }
    
    private static bool IsDragonsDogmaRoot(string currentFileLine, string nextLine) => 
        currentFileLine.Contains(Constants.DragonsDogma2Id, StringComparison.OrdinalIgnoreCase) && nextLine.Contains(Constants.StartBracket, StringComparison.OrdinalIgnoreCase);

    private (bool launchOptionsFound, int foundLaunchOptionsIndex, bool dragonsDogmaSectionFound) CheckForLaunchOptionsAndDd2Section(IList<string> lines, int startIndexOfSection)
    {
        var launchOptionsFound = false;
        var foundLaunchOptionsIndex = -1;
        var dragonsDogmaSectionFound = false;

        for (var j = startIndexOfSection; j < startIndexOfSection + Constants.MaxNumberOfConfigFileLinesToSearch; j++)
        {
            if (IsLaunchSettingsLine(lines[j]))
            {
                launchOptionsFound = true;
                foundLaunchOptionsIndex = j;
                break;
            }

            if (IsDd2Section(lines[j]))
            {
                dragonsDogmaSectionFound = true;
            }
        }
        
        return (launchOptionsFound, foundLaunchOptionsIndex, dragonsDogmaSectionFound);
    }
    
    //Check if the section contains an EULA or Playtime string - if so we assume it's the DD2 section
    private static bool IsDd2Section(string currentFileLine) =>
        currentFileLine.Contains(Constants.LocalConfigEulaString, StringComparison.OrdinalIgnoreCase) ||
        currentFileLine.Contains(Constants.LocalConfigPlaytimeString, StringComparison.OrdinalIgnoreCase);

    private static bool IsLaunchSettingsLine(string currentFileLine) => 
        currentFileLine.Contains(Constants.LaunchOptionsSectionRoot, StringComparison.OrdinalIgnoreCase);
    
    private static void ThrowExceptionIfStartSectionNotFound(int sectionStartIndex)
    {
        if(sectionStartIndex < 0)
        {
            throw new ApplicationException("Could not find the start of the Dragons Dogma 2 section in the Steam config file.");
        }
    }
}