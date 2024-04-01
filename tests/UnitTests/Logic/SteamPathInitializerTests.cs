namespace DragonsDogma2FileBackupWorker.UnitTests.Logic;

[TestFixture]
public class SteamPathInitializerTests
{
    private IDirectoryStorage _directoryStorage = null!;
    private Mock<IIoWrapper> _mockIoWrapper = null!;
    
    private SteamPathInitializer _steamPathInitializer = null!;
    
    [SetUp]
    public void SetUp()
    {
        _directoryStorage = new DirectoryStorage();
        _mockIoWrapper = new Mock<IIoWrapper>();

        _steamPathInitializer = new SteamPathInitializer(_directoryStorage, _mockIoWrapper.Object);
    }
    
    [Test]
    public void InitializeSteamPathsAsync_WhenOsIsNotWindows_ThrowsPlatformNotSupportedException()
    {
        //Arrange
        _mockIoWrapper.Setup(m => m.IsWindowsOs()).Returns(false);
        
        //Act
        var result = Assert.Throws<PlatformNotSupportedException>(() => _steamPathInitializer.InitializeSteamDirectories());
        
        //Assert
        Assert.That(result!.Message, Is.EqualTo("This application only supports Windows"));
    }
    
    [Test]
    public void InitializeSteamPathsAsync_WhenSteamPathIsNotSet_ThrowsDirectoryNotFoundException()
    {
        //Arrange
        _mockIoWrapper.Setup(m => m.IsWindowsOs()).Returns(true);
        _mockIoWrapper.Setup(m => m.GetRegistryKeyValue(It.IsAny<string>())).Returns(string.Empty);
        
        //Act
        var result = Assert.Throws<DirectoryNotFoundException>(() => _steamPathInitializer.InitializeSteamDirectories());
        
        //Assert
        Assert.That(result!.Message, Is.EqualTo("Could not find Steam path in registry"));
    }
    
    [Test]
    public void InitializeSteamPathsAsync_WhenSteamPathIsSetAndDragonsDogma2FolderExists_SetsSteamPathsCorrectly()
    {
        //Arrange
        var steamPath = "steamPath";
        var steamUserDataPath = "steamUserDataPath";
        var steamAccountDirectory = "25470";
        var steamSaveFileDirectory = "steamSaveFileDirectory";

        _mockIoWrapper.Setup(m => m.IsWindowsOs()).Returns(true);
        _mockIoWrapper.Setup(m => m.GetRegistryKeyValue(It.IsAny<string>())).Returns(steamPath);
        _mockIoWrapper.Setup(m => m.CombinePath(steamPath, Constants.SteamUserDataDirectory)).Returns(steamUserDataPath);
        _mockIoWrapper.Setup(m => m.GetDirectories(steamUserDataPath)).Returns(new[] { steamAccountDirectory });
        _mockIoWrapper.Setup(m => m.GetDirectories(steamAccountDirectory)).Returns(new[] { $"{Constants.DragonsDogma2Id}" });
        _mockIoWrapper.Setup(m => m.CombinePath(steamAccountDirectory, Constants.DragonsDogma2Id)).Returns(steamSaveFileDirectory);

        //Act
        _steamPathInitializer.InitializeSteamDirectories();

        //Assert
        Assert.That(_directoryStorage.SteamAccountDirectory, Is.EqualTo(steamAccountDirectory));
        Assert.That(_directoryStorage.SteamAccountId, Is.EqualTo(steamAccountDirectory));
        Assert.That(_directoryStorage.SteamSaveFileDirectory, Is.EqualTo(steamSaveFileDirectory));
    }
    
    [Test]
    public void InitializeSteamPathsAsync_WhenSteamPathIsSetAndDragonsDogma2FolderDoesNotExist_ThrowsDirectoryNotFoundException()
    {
        //Arrange
        var steamPath = "steamPath";
        var steamUserDataPath = "steamUserDataPath";
        var steamAccountDirectory = "25470";

        _mockIoWrapper.Setup(m => m.IsWindowsOs()).Returns(true);
        _mockIoWrapper.Setup(m => m.GetRegistryKeyValue(It.IsAny<string>())).Returns(steamPath);
        _mockIoWrapper.Setup(m => m.CombinePath(steamPath, Constants.SteamUserDataDirectory)).Returns(steamUserDataPath);
        _mockIoWrapper.Setup(m => m.GetDirectories(steamUserDataPath)).Returns([steamAccountDirectory]);
        _mockIoWrapper.Setup(m => m.GetDirectories(steamAccountDirectory)).Returns(Array.Empty<string>());

        //Act
        var result = Assert.Throws<DirectoryNotFoundException>(() => _steamPathInitializer.InitializeSteamDirectories());

        //Assert
        Assert.That(result!.Message, Is.EqualTo("Could not find Dragons Dogma 2 save path in Steam directory"));
    }
}