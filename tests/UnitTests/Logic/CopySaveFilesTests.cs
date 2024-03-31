using DragonsDogma2FileBackupWorker;
using DragonsDogma2FileBackupWorker.Logic;
using DragonsDogma2FileBackupWorker.Logic.Abstract;
using DragonsDogma2FileBackupWorker.Logic.IO.Abstract;

namespace UnitTests.Logic;

[TestFixture]
public class CopySaveFilesTests
{
    private Mock<IDirectoryStorage> _mockDirectoryStorage = null!;
    private Mock<IIoWrapper> _mockIoWrapper = null!;
    private Mock<IOptions<CopyOptions>> _mockCopyOptions = null!;
    private Mock<ILogger<CopySaveFiles>> _mockLogger = null!;
    
    [SetUp]
    public void SetUp()
    {
        _mockDirectoryStorage = new Mock<IDirectoryStorage>();
        _mockIoWrapper = new Mock<IIoWrapper>();
        _mockCopyOptions = new Mock<IOptions<CopyOptions>>();
        _mockLogger = new Mock<ILogger<CopySaveFiles>>();
    }
    
    [TestCase("")]
    [TestCase(" ")]
    public void CopyFilesAsync_WhenDestinationDirectoryIsNotSet_ThrowsArgumentException(string destinationDirectory)
    {
        //Arrange
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = destinationDirectory });
        var copySaveFiles = new CopySaveFiles(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
       
        //Act //Assert
        Assert.ThrowsAsync<ArgumentException>( () => copySaveFiles.CopyFilesAsync());
    }
    
    [Test]
    public void CopyFilesAsync_WhenWaitTimeInSecondsLessThanZero_ThrowsArgumentException()
    {
        //Arrange
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = @"C:\temp\backup", WaitTimeInSeconds = -1 });
        var copySaveFiles = new CopySaveFiles(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        //Act //Assert
        Assert.ThrowsAsync<ArgumentException>( () => copySaveFiles.CopyFilesAsync());
    }
    
    [TestCase("")]
    [TestCase(" ")]
    public void CopyFilesAsync_WhenSteamSaveFileDirectoryIsNotSet_ThrowsArgumentException_(string saveFileDirectory)
    {
        // Arrange
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = @"C:\temp\backup", WaitTimeInSeconds = 1 });
        _mockDirectoryStorage.Setup(m => m.SteamSaveFileDirectory).Returns(saveFileDirectory);
        var copySaveFiles = new CopySaveFiles(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        //Act //Assert
        Assert.ThrowsAsync<ArgumentException>( () => copySaveFiles.CopyFilesAsync());
    }
    
    [Test]
    public async Task CopyFilesAsync_WhenDragonsDogma2Running_CopyFiles()
    {
        // Arrange
        var steamSaveDirectory = "sourceDirectory";
        var destinationDirectory = @"C:\temp\backup";
;
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = destinationDirectory, WaitTimeInSeconds = 1});
        _mockDirectoryStorage.Setup(ds => ds.SteamSaveFileDirectory).Returns(steamSaveDirectory)
            .Verifiable();
        _mockIoWrapper.Setup(io => io.CombinePath(destinationDirectory, It.IsAny<string>())).Returns("combinedPath");
        _mockIoWrapper.SetupSequence(io => io.ProcessExists(Constants.DraginsDogma2ProcessName))
            .Returns(true)
            .Returns(true)
            .Returns(false);
        
        var copySaveFiles = new CopySaveFiles(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        // Act
        await copySaveFiles.CopyFilesAsync();

        // Assert
        _mockDirectoryStorage.Verify();
        _mockIoWrapper.Verify(io => io.CopyDirectory(steamSaveDirectory, _mockIoWrapper.Object.CombinePath(destinationDirectory, It.IsAny<string>())), Times.Exactly(2));
    }
}