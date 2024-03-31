using DragonsDogma2FileBackupWorker;
using DragonsDogma2FileBackupWorker.Logic;
using DragonsDogma2FileBackupWorker.Models;

namespace UnitTests.Logic;

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
        var configFileContentLines = new List<string> { "", Constants.DragonsDogma2Id, "{",  "2054970_eula", "", "LaunchOptions", "}", ""};

        // Act
        var result = _vdfFileHelper.GetStartAndEndIndexOfSection(configFileContentLines);

        // Assert
        Assert.That(result.StartIndex, Is.EqualTo(1));
        Assert.That(result.LaunchOptionsExists, Is.True);
        Assert.That(result.LaunchOptionsIndex, Is.EqualTo(5));
    }
    
    [Test]
    public void GetStartAndEndIndexOfSection_WhenLaunchOptionsNotFound_ReturnsCorrectData()
    {
        // Arrange
        var configFileContentLines = new List<string> { "", Constants.DragonsDogma2Id, "{",  "2054970_eula", "", "", "", "", "", "","", "", "", "", "", "", "","", "", "", ""};

        // Act
        var result = _vdfFileHelper.GetStartAndEndIndexOfSection(configFileContentLines);

        // Assert
        Assert.That(result.StartIndex, Is.EqualTo(1));
        Assert.That(result.LaunchOptionsExists, Is.False);
        Assert.That(result.LaunchOptionsIndex, Is.EqualTo(-1));
    }

    [Test]
    public void UpdateSteamLaunchConfig_WhenLaunchOptionsDoesNotExist_UpdatesConfigCorrectly()
    {
        // Arrange
        var configFileData = new LocalConfigFileData(3, false, 5);
        var configFileLines = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();
        var launchOptionsString = "this is a test string";

        // Act
        _vdfFileHelper.UpdateSteamLaunchConfig(configFileData, configFileLines, launchOptionsString);

        // Assert
        Assert.That(configFileLines[5], Is.EqualTo(launchOptionsString));
    }
    
    [Test]
    public void UpdateSteamLaunchConfig_WhenLaunchOptionsExist_UpdatesConfigCorrectly()
    {
        // Arrange
        var configFileData = new LocalConfigFileData(3, true, 5);
        var configFileLines = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();
        var launchOptionsString = "this is a test string";

        // Act
        _vdfFileHelper.UpdateSteamLaunchConfig(configFileData, configFileLines, launchOptionsString);

        // Assert
        Assert.That(configFileLines[5], Is.EqualTo(launchOptionsString));
    }
}