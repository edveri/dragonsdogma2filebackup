namespace DragonsDogma2FileBackupWorker.UnitTests.Logic;

[TestFixture]
public class DirectoryStorageBuilderTests
{
    private Mock<ISteamPathInitializer> _steamPathInitializerMock = null!;
    private Mock<IDirectoryStorage> _directoryStorageMock = null!;
    
    private DirectoryStorageBuilder _directoryStorageBuilder = null!;
    
    [SetUp]
    public void SetUp()
    {
        _steamPathInitializerMock = new Mock<ISteamPathInitializer>();
        _directoryStorageMock = new Mock<IDirectoryStorage>();
        
        _directoryStorageBuilder = new DirectoryStorageBuilder(_steamPathInitializerMock.Object, _directoryStorageMock.Object);
    }
    
    [Test]
    public void BuildDirectoryStorage_CallsInitializeSteamDirectories()
    {
        //Arrange
        _directoryStorageMock.Setup(m => m.Validate(null!))
            .Returns(new List<ValidationResult>());
        
        //Act
        _directoryStorageBuilder.BuildDirectoryStorage();
        
        //Assert
        _steamPathInitializerMock.Verify(m => m.InitializeSteamDirectories(), Times.Once);
    }
    
    [Test]
    public void BuildDirectoryStorage_WhenValidationErrorsExist_ThrowsApplicationException()
    {
        //Arrange
        var validationErrors = new List<ValidationResult> { new("error"), new("error2") };
        
        _directoryStorageMock.Setup(m => m.Validate(null!))
            .Returns(validationErrors);
        
        //Act
        var result = Assert.Throws<ApplicationException>(() => _directoryStorageBuilder.BuildDirectoryStorage());
        
        //Assert
        Assert.That(result!.Message, Is.EqualTo("error, error2"));
    }
}