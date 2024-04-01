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
            .Returns(expectedBackupName);
        
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
        var configFileContent = new[]{"this", "is", "config", "file"};
        var steamConfigFileInfo = new SteamConfigFileInfo(3, false, -1);

        _mockIoWrapper.Setup(x => x.CombinePath(SteamAccountDirectory, Constants.ConfigDirectoryName))
            .Returns(expectedSteamConfigDir);

        _mockIoWrapper.Setup(x => x.CombinePath(expectedSteamConfigDir, Constants.LocalConfigFileName))
            .Returns(expectedSteamConfigFile);

        _mockIoWrapper.Setup(x => x.CombinePath(SteamRootPath, Constants.SteamAppsDirectory, Constants.BatchFileName))
            .Returns(expectedSteamConfigFile)
            .Verifiable();
        
        _mockIoWrapper.Setup(m => m.ReadAllLinesAsync(expectedSteamConfigFile))
            .ReturnsAsync(configFileContent)
            .Verifiable();

        _mockVdfFileReader.Setup(m => m.GetStartAndEndIndexOfSection(configFileContent))
            .Returns(steamConfigFileInfo)
            .Verifiable();
        
        //Act
        await _steamLaunchOptionsEditor.SetSteamLaunchOptionsAsync();
        
        //Assert
        _mockIoWrapper.Verify();
        _mockVdfFileReader.Verify(m => m.UpdateSteamLaunchConfig(It.IsAny<SteamConfigFileInfo>(), It.IsAny<List<string>>(), It.IsAny<string>()), Times.Once);
    }
    
    [Test]
    public async Task SetSteamLaunchOptionsAsync_VerifyWriteAllLinesAsyncCall()
    {
        //Arrange
        var expectedSteamConfigDir = Path.Combine(SteamAccountDirectory, Constants.ConfigDirectoryName);
        var expectedSteamConfigFile = Path.Combine(expectedSteamConfigDir, Constants.LocalConfigFileName);
        var configFileContent = new[]{"this", "is", "config", "file"};
        var steamConfigFileInfo = new SteamConfigFileInfo(3, false, -1);

        _mockIoWrapper.Setup(x => x.CombinePath(SteamAccountDirectory, Constants.ConfigDirectoryName))
            .Returns(expectedSteamConfigDir);

        _mockIoWrapper.Setup(x => x.CombinePath(expectedSteamConfigDir, Constants.LocalConfigFileName))
            .Returns(expectedSteamConfigFile);

        _mockIoWrapper.Setup(x => x.CombinePath(SteamRootPath, Constants.SteamAppsDirectory, Constants.BatchFileName))
            .Returns(expectedSteamConfigFile);

        _mockIoWrapper.Setup(m => m.ReadAllLinesAsync(expectedSteamConfigFile))
            .ReturnsAsync(configFileContent);

        _mockVdfFileReader.Setup(m => m.GetStartAndEndIndexOfSection(configFileContent))
            .Returns(steamConfigFileInfo);
        
        //Act
        await _steamLaunchOptionsEditor.SetSteamLaunchOptionsAsync();
        
        //Assert
        _mockIoWrapper.Verify(m => m.WriteAllLinesAsync(expectedSteamConfigFile, configFileContent), Times.Once);
    }
}