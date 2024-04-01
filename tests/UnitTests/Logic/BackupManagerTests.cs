using System.Collections;
using DragonsDogma2FileBackupWorker.Logic.IO;

namespace DragonsDogma2FileBackupWorker.UnitTests.Logic;

[TestFixture]
public class BackupManagerTests
{
    private Mock<IDirectoryStorage> _mockDirectoryStorage = null!;
    private Mock<IIoWrapper> _mockIoWrapper = null!;
    private Mock<IOptions<CopyOptions>> _mockCopyOptions = null!;
    private Mock<ILogger<BackupManager>> _mockLogger = null!;
    
    [SetUp]
    public void SetUp()
    {
        _mockDirectoryStorage = new Mock<IDirectoryStorage>();
        _mockIoWrapper = new Mock<IIoWrapper>();
        _mockCopyOptions = new Mock<IOptions<CopyOptions>>();
        _mockLogger = new Mock<ILogger<BackupManager>>();
    }
    
    [TestCase("")]
    [TestCase(" ")]
    public void CreateBackupsAsync_WhenDestinationDirectoryIsNotSet_ThrowsArgumentException(string destinationDirectory)
    {
        //Arrange
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = destinationDirectory });
        var backupManager = new BackupManager(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
       
        //Act //Assert
        Assert.ThrowsAsync<ArgumentException>( () => backupManager.ExecuteBackupsAsync());
    }
    
    [Test]
    public void CreateBackupsAsync_WhenWaitTimeInSecondsLessThanZero_ThrowsArgumentException()
    {
        //Arrange
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = @"C:\temp\backup", WaitTimeInSeconds = -1 });
        var backupManager = new BackupManager(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        //Act //Assert
        Assert.ThrowsAsync<ArgumentException>( () => backupManager.ExecuteBackupsAsync());
    }
    
    [TestCase("")]
    [TestCase(" ")]
    public void CreateBackupsAsync_WhenSteamSaveFileDirectoryIsNotSet_ThrowsArgumentException_(string saveFileDirectory)
    {
        // Arrange
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = @"C:\temp\backup", WaitTimeInSeconds = 1 });
        _mockDirectoryStorage.Setup(m => m.SteamSaveFileDirectory).Returns(saveFileDirectory);
        var backupManager = new BackupManager(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        //Act //Assert
        Assert.ThrowsAsync<ArgumentException>( () => backupManager.ExecuteBackupsAsync());
    }
    
    [Test]
    public async Task CreateBackupsAsync_WhenDragonsDogma2Running_CopyFiles()
    {
        // Arrange
        var steamSaveDirectory = "sourceDirectory";
        var destinationDirectory = @"C:\temp\backup";
;
        _mockCopyOptions.Setup(m => m.Value).Returns(new CopyOptions { DestinationDirectory = destinationDirectory, WaitTimeInSeconds = 1});
        _mockDirectoryStorage.Setup(ds => ds.SteamSaveFileDirectory).Returns(steamSaveDirectory)
            .Verifiable();
        _mockIoWrapper.Setup(io => io.CombinePath(destinationDirectory, It.IsAny<string>())).Returns("combinedPath");
        _mockIoWrapper.SetupSequence(io => io.ProcessExists(Constants.DragonsDogma2ProcessName))
            .Returns(true)
            .Returns(true)
            .Returns(false);
        
        var backupManager = new BackupManager(_mockDirectoryStorage.Object, _mockIoWrapper.Object, _mockCopyOptions.Object, _mockLogger.Object);
        
        // Act
        await backupManager.ExecuteBackupsAsync();

        // Assert
        _mockDirectoryStorage.Verify();
        _mockIoWrapper.Verify(io => io.CopyDirectory(steamSaveDirectory, _mockIoWrapper.Object.CombinePath(destinationDirectory, It.IsAny<string>())), Times.Exactly(2));
    }

    [Test]
    public async Task CreateBackupsAsync_WhenNumberOfBackupsAboveMax_DeleteBackups()
    {
        // Arrange
        var expectedRemainingBackups =
            new[]
            {
                "Backup_2024-01-01 12-59-00", "Backup_2024-01-01 12-53-00", "Backup_2024-01-01 12-45-00",
                "Backup_2024-01-01 12-54-00", "Backup_2024-01-01 12-32-00"
            };
        var destinationDirectory = @"C:\temp\backup";
    
        // _mockIoWrapper.Setup(m => m.GetDirectories(_))
    }

    //We want to delete all backups but the 5 latest. 
    IEnumerable<IDirectoryInfoWrapper> GetDirectoryInfoWrappers()
    {
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 2, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 2, 0));
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 12, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 12, 0));
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 15, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 15, 0));
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 59, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 59, 0)); //keep
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 31, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 31, 0));
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 11, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 11, 0));
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 53, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 53, 0)); //keep
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 9, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 9, 0));
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 1, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 1, 0));
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 45, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 45, 0)); //keep
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 54, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 54, 0)); //keep
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 32, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 32, 0)); //keep
        yield return Mock.Of<IDirectoryInfoWrapper>(dir =>
            dir.Name == $"Backup_{new DateTime(2024, 1, 1, 12, 31, 0):yyyy-MM-dd HH-mm-ss}" &&
            dir.CreationTime == new DateTime(2024, 1, 1, 12, 31, 0));
    }
}