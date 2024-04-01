using DragonsDogma2FileBackupWorker.Models.Abstract;

namespace DragonsDogma2FileBackupWorker.UnitTests.Logic;

[TestFixture]
public class ApplicationFileAndDirectoryHelperTests
{
    private Mock<IDirectoryStorage> _mockDirectoryStorage = null!;
    private Mock<IIoWrapper> _mockIoWrapper = null!;
    private Mock<IOptions<CopyOptions>> _mockCopyOptions = null!;
    private Mock<ILogger<ApplicationFileAndDirectoryHelper>> _mockLogger = null!;
    private ApplicationFileAndDirectoryHelper _applicationFileAndDirectoryHelper = null!;
    
    [SetUp]
    public void SetUp()
    {
        _mockDirectoryStorage = new Mock<IDirectoryStorage>();
        _mockIoWrapper = new Mock<IIoWrapper>();
        _mockCopyOptions = new Mock<IOptions<CopyOptions>>();
        _mockLogger = new Mock<ILogger<ApplicationFileAndDirectoryHelper>>();
    }

    [TestCase("")]
    [TestCase(" ")]
    public void CreateBackupDirAndBatchFileAsync_WhenSteamRootPathIsNullOrWhiteSpace_ThrowsArgumentException(string steamRootPath)
    {
        //Arrange
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = "destinationDir" });
        _mockDirectoryStorage.Setup(ds => ds.SteamRootPath).Returns(steamRootPath);
        
        _applicationFileAndDirectoryHelper = new ApplicationFileAndDirectoryHelper(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        //Act
        var result = Assert.ThrowsAsync<ArgumentException>(() => _applicationFileAndDirectoryHelper.CreateBackupDirAndBatchFileAsync());
        
        //Act
        Assert.That(result!.Message, Is.EqualTo("Steam root path is not set."));
    }
   
    [Test]
    public async Task CreateBackupDirAndBatchFileAsync_WhenDestinationDirectoryExists_DoesNotCreateDirectory()
    {
        //Arrange
        var destinationDirectory = "destinationDirectory";
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = destinationDirectory });
        _mockDirectoryStorage.Setup(ds => ds.SteamRootPath).Returns("steamRootPath");
        _mockIoWrapper.Setup(iw => iw.DirectoryExists(destinationDirectory)).Returns(true);
        
        _applicationFileAndDirectoryHelper = new ApplicationFileAndDirectoryHelper(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        //Act
        await _applicationFileAndDirectoryHelper.CreateBackupDirAndBatchFileAsync();
        
        //Assert
        _mockIoWrapper.Verify(iw => iw.CreateDirectory(destinationDirectory), Times.Never);
    }
    
    [Test]
    public async Task CreateBackupDirAndBatchFileAsync_WhenDestinationDirectoryDoesNotExist_CreatesDirectory()
    {
        //Arrange
        var destinationDirectory = "destinationDirectory";
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = destinationDirectory });
        _mockDirectoryStorage.Setup(ds => ds.SteamRootPath).Returns("steamRootPath");
        _mockIoWrapper.Setup(iw => iw.DirectoryExists(destinationDirectory)).Returns(false);
        
        _applicationFileAndDirectoryHelper = new ApplicationFileAndDirectoryHelper(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        //Act
        await _applicationFileAndDirectoryHelper.CreateBackupDirAndBatchFileAsync();
        
        // Assert
        _mockIoWrapper.Verify(iw => iw.CreateDirectory(destinationDirectory), Times.Once);
    }
}

    
