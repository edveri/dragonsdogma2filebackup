using DragonsDogma2FileBackupWorker.Models.Abstract;

namespace DragonsDogma2FileBackupWorker.UnitTests.Logic;

[TestFixture]
public class SteamLaunchOptionsEditorTests
{
    private Mock<IVdfFileHelper> _mockVdfFileReader = null!;
    private Mock<IDirectoryStorage> _mockDirectoryStorage = null!;
    private Mock<IIoWrapper> _mockIoWrapper = null!;
    
    private const string SteamAccountDirectory = "SteamAccountDirectory";
    private const string SteamSaveFileDirectory = "SteamSaveFileDirectory";
    private const string SteamAccountId = "SteamAccountId";
    private const string SteamRootPath = @"C:\Dir\test";
    
    private SteamLaunchOptionsEditor _steamLaunchOptionsEditor = null!;

    [SetUp]
    public void SetUp()
    {
        _mockVdfFileReader = new Mock<IVdfFileHelper>();
        _mockDirectoryStorage = new Mock<IDirectoryStorage>();
        _mockIoWrapper = new Mock<IIoWrapper>();
        
        _mockDirectoryStorage.Setup(m => m.SteamAccountDirectory).Returns(SteamAccountDirectory);
        _mockDirectoryStorage.Setup(m => m.SteamSaveFileDirectory).Returns(SteamSaveFileDirectory);
        _mockDirectoryStorage.Setup(m => m.SteamAccountId).Returns(SteamAccountId);
        _mockDirectoryStorage.Setup(m => m.SteamRootPath).Returns(SteamRootPath);
        
        _steamLaunchOptionsEditor = new SteamLaunchOptionsEditor(_mockVdfFileReader.Object, _mockDirectoryStorage.Object, _mockIoWrapper.Object);
    }
    
    [Test]
    public async Task SetSteamLaunchOptionsAsync_ExistingSteamConfigFileBackedUp()
    {
        //Arrange
        var expectedSteamConfigDir = Path.Combine(SteamAccountDirectory, Constants.ConfigDirectoryName);
        var expectedSteamConfigFile = Path.Combine(expectedSteamConfigDir, Constants.LocalConfigFileName);
        var expectedBackupName = $"{Constants.LocalConfigFileName}_backup{DateTime.Now:hh-mm-ss}";
        
        _mockIoWrapper.Setup(x => x.CombinePath(SteamAccountDirectory, Constants.ConfigDirectoryName))
            .Returns(expectedSteamConfigDir)
            .Verifiable();
        
        _mockIoWrapper.Setup(x => x.CombinePath(expectedSteamConfigDir, Constants.LocalConfigFileName))
            .Returns(expectedSteamConfigFile)
            .Verifiable();

        _mockIoWrapper.Setup(m => m.CombinePath(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("dummy");
        
        _mockIoWrapper.Setup(m => m.CombinePath(expectedSteamConfigDir, It.Is<string>(s => s.StartsWith($"{Constants.LocalConfigFileName}_backup"))))
            .Returns("dummy");
        
        //Act
        await _steamLaunchOptionsEditor.SetSteamLaunchOptionsAsync();
        
        //Assert
        _mockIoWrapper.Verify(m => m.Copy(expectedSteamConfigFile, expectedBackupName), Times.Once);
    }
    
    [Test]
    public async Task SetSteamLaunchOptionsAsync_VerifyUpdateSteamLaunchConfigCall()
    {
        //Arrange
        var expectedSteamConfigDir = Path.Combine(SteamAccountDirectory, Constants.ConfigDirectoryName);
        var expectedSteamConfigFile = Path.Combine(expectedSteamConfigDir, Constants.LocalConfigFileName);
        var expectedBackupName = $"{Constants.LocalConfigFileName}_backup{DateTime.Now:hh-mm-ss}";
        
        _mockIoWrapper.Setup(x => x.CombinePath(SteamAccountDirectory, Constants.ConfigDirectoryName))
            .Returns(expectedSteamConfigDir)
            .Verifiable();
        
        _mockIoWrapper.Setup(x => x.CombinePath(expectedSteamConfigDir, Constants.LocalConfigFileName))
            .Returns(expectedSteamConfigFile)
            .Verifiable();

        _mockIoWrapper.Setup(m => m.CombinePath(SteamRootPath, Constants.SteamAppsDirectory, Constants.BatchFileName))
            .Returns("dummy");
        
        
        
        //Act
        await _steamLaunchOptionsEditor.SetSteamLaunchOptionsAsync();
        
        //Assert
        _mockIoWrapper.Verify(m => m.WriteAllLinesAsync(expectedSteamConfigFile, It.IsAny<IEnumerable<string>>()), Times.Once);
    }
}