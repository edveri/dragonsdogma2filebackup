using DragonsDogma2FileBackupWorker.Logic;
using DragonsDogma2FileBackupWorker.Logic.Abstract;
using DragonsDogma2FileBackupWorker.Logic.IO.Abstract;

namespace UnitTests.Logic;

[TestFixture]
public class LaunchOptionsEditorTests
{
    
    private Mock<IVdfFileHelper> _mockVdfFileReader = null!;
    private Mock<IDirectoryStorage> _mockDirectoryStorage = null!;
    private Mock<IIoWrapper> _mockIoWrapper = null!;
    
    private LaunchOptionsEditor _launchOptionsEditor = null!;

    [SetUp]
    public void SetUp()
    {
        _mockVdfFileReader = new Mock<IVdfFileHelper>();
        _mockDirectoryStorage = new Mock<IDirectoryStorage>();
        _mockIoWrapper = new Mock<IIoWrapper>();

        _launchOptionsEditor = new LaunchOptionsEditor(_mockVdfFileReader.Object, _mockDirectoryStorage.Object, _mockIoWrapper.Object);
    }
    
    [TestCase("")]
    [TestCase(" ")]
    public void SetSteamLaunchOptionsAsync_WhenSteamAccountDirectoryIsNullOrWhiteSpace_ThrowsArgumentException(string steamSourceDirectory)
    {
        // Arrange
        _mockDirectoryStorage.Setup(ds => ds.SteamAccountDirectory).Returns(steamSourceDirectory);

        // Act
        var result = Assert.ThrowsAsync<ArgumentException>(() => _launchOptionsEditor.SetSteamLaunchOptionsAsync());
        
        // Assert
        Assert.That(result!.Message, Is.EqualTo("Steam account directory is not set."));
    }
    
    [Test]
    public async Task SetSteamLaunchOptionsAsync_CallsVdfFileReaderWithCorrectParameters()
    {
        // Arrange
        var steamSourceDirectory = "steamSourceDirectory";
        var steamRootPath = "validPath";
        var expectedConfigFileLines = Enumerable.Range(1, 10).Select(i => i.ToString()).ToArray();

        _mockDirectoryStorage.Setup(ds => ds.SteamAccountDirectory).Returns(steamSourceDirectory);
        _mockDirectoryStorage.Setup(ds => ds.SteamRootPath).Returns(steamRootPath);
        _mockIoWrapper.Setup(io => io.ReadAllLinesAsync(It.IsAny<string>())).ReturnsAsync(expectedConfigFileLines);

        // Act
        await _launchOptionsEditor.SetSteamLaunchOptionsAsync();

        // Assert
        _mockVdfFileReader.Verify(m => m.GetStartAndEndIndexOfSection(expectedConfigFileLines), Times.Once);
    }
}