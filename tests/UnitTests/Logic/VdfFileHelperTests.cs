namespace DragonsDogma2FileBackupWorker.UnitTests.Logic;

[TestFixture]
public class VdfFileHelperTests
{
    private VdfFileHelper _vdfFileHelper = null!;
    
    [SetUp]
    public void SetUp()
    {
        _vdfFileHelper = new VdfFileHelper();
    }
    
    [Test]
    public void GetStartAndEndIndexOfSection_WhenLaunchOptionsFound_ReturnsCorrectData()
    {
        // Arrange
        var configFileContentLines = new List<string> { "", Constants.DragonsDogma2Id, Constants.StartBracket.ToString(), Constants.LocalConfigEulaString, "", Constants.LaunchOptionsSectionRoot, "}", ""}
            .Concat(Enumerable.Range(1, Constants.MaxNumberOfConfigFileLinesToSearch).Select(i => i.ToString())).ToList();

        // Act
        var result = _vdfFileHelper.GetStartAndEndIndexOfSection(configFileContentLines);

        // Assert
        Assert.That(result.StartIndex, Is.EqualTo(1));
        Assert.That(result.LaunchOptionsExists, Is.True);
        Assert.That(result.LaunchOptionsIndex, Is.EqualTo(5));
    }

    [TestCaseSource(nameof(TestCases))]
    public void GetStartAndEndIndexOfSection_WhenLaunchOptionsNotFound_ReturnsCorrectData(IList<string> configFileContentLines, SteamConfigFileInfo steamConfigFileInfo)
    {
        // Act
        var result = _vdfFileHelper.GetStartAndEndIndexOfSection(configFileContentLines);

        // Assert
        Assert.That(result.StartIndex, Is.EqualTo(steamConfigFileInfo.StartIndex));
        Assert.That(result.LaunchOptionsExists, Is.EqualTo(steamConfigFileInfo.LaunchOptionsExists));
        Assert.That(result.LaunchOptionsIndex, Is.EqualTo(steamConfigFileInfo.LaunchOptionsIndex));
    }

    [Test]
    public void UpdateSteamLaunchConfig_WhenLaunchOptionDoesNotExist_InsertsConfigCorrectly()
    {
        // Arrange
        var configFileData = new SteamConfigFileInfo(3, false, -1);
        var configFileLines = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();
        var launchOptionsString = "this is a test string";

        // Act
        _vdfFileHelper.UpdateSteamLaunchConfig(configFileData, configFileLines, launchOptionsString);

        // Assert
        Assert.That(configFileLines[5], Is.EqualTo(launchOptionsString));
    }

    private static readonly object[] TestCases =
    [
        new object[] 
        { 
            new List<string>{ "", Constants.DragonsDogma2Id, Constants.StartBracket.ToString(), Constants.LocalConfigEulaString}
                .Concat(Enumerable.Range(1, Constants.MaxNumberOfConfigFileLinesToSearch).Select(i => i.ToString())).ToList(), 
            new SteamConfigFileInfo(1, false, -1)
        },
        new object[] 
        { 
            new List<string>{ "", "", "", Constants.DragonsDogma2Id, Constants.StartBracket.ToString(), "", "", "", Constants.LocalConfigPlaytimeString,}
                .Concat(Enumerable.Range(1, Constants.MaxNumberOfConfigFileLinesToSearch).Select(i => i.ToString())).ToList(), 
            new SteamConfigFileInfo(3, false, -1)
        },
        new object[] 
        { 
            new List<string>{ "", "", "", "", "", Constants.DragonsDogma2Id, Constants.StartBracket.ToString() , "", "", "", Constants.LocalConfigPlaytimeString, Constants.LaunchOptionsSectionRoot}
                .Concat(Enumerable.Range(1, Constants.MaxNumberOfConfigFileLinesToSearch).Select(i => i.ToString())).ToList(), 
            new SteamConfigFileInfo(5, true, 11)
        }
    ];
    
    [Test]
    public void GetStartAndEndIndexOfSection_WhenDragonsDogmaSectionNotFound_ThrowsException()
    {
        // Arrange
        var configFileContentLines = Enumerable.Range(1, 100).Select(i => i.ToString()).ToList();
        
        // Act
        var result = Assert.Throws<ApplicationException>(() => _vdfFileHelper.GetStartAndEndIndexOfSection(configFileContentLines));
        
        // Assert
        Assert.That(result!.Message, Is.EqualTo("Could not find the start of the Dragons Dogma 2 section in the Steam config file."));
    }
    
    [Test]
    public void UpdateSteamLaunchConfig_WhenLaunchOptionsExist_UpdatesConfigCorrectly()
    {
        // Arrange
        var configFileData = new SteamConfigFileInfo(3, true, 5);
        var configFileLines = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();
        var launchOptionsString = "this is a test string";

        // Act
        _vdfFileHelper.UpdateSteamLaunchConfig(configFileData, configFileLines, launchOptionsString);

        // Assert
        Assert.That(configFileLines[5], Is.EqualTo(launchOptionsString));
    }
    
    [Test]
    public void UpdateSteamLaunchConfig_WhenLaunchOptionsDoesNotExist_UpdatesConfigCorrectly()
    {
        // Arrange
        var configFileData = new SteamConfigFileInfo(3, false, 5);
        var configFileLines = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();
        var launchOptionsString = "this is a test string";

        // Act
        _vdfFileHelper.UpdateSteamLaunchConfig(configFileData, configFileLines, launchOptionsString);

        // Assert
        Assert.That(configFileLines[5], Is.EqualTo(launchOptionsString));
    }
}