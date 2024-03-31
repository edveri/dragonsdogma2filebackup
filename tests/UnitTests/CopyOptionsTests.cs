using DragonsDogma2FileBackupWorker;

namespace UnitTests;

[TestFixture]
public class CopyOptionsTests
{
    private CopyOptions _copyOptions = null!;
    
    [SetUp]
    public void SetUp()
    {
        _copyOptions = new CopyOptions();
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("InvalidPath")]
    public void Validation_DestinationDirectoryNotSet_ReturnsError(string destinationDirectory)
    {
        //Arrange
        _copyOptions.DestinationDirectory = destinationDirectory;
        _copyOptions.WaitTimeInSeconds = 900;
        
        //Act
        var result = _copyOptions.Validate(null);
        
        //Assert
        Assert.That(result.Single().ErrorMessage, Is.EqualTo("Destination directory must be provided."));
    }
    
    [TestCase(-1)]
    [TestCase(-100)]
    public void Validation_WaitTimeIsNegative_ReturnsError(int waitTimeInSeconds)
    {
        //Arrange
        _copyOptions.DestinationDirectory = @"C:\temp\DragonsDogma2Backups";
        _copyOptions.WaitTimeInSeconds = waitTimeInSeconds;
        
        //Act
        var result = _copyOptions.Validate(null);
        
        //Assert
        Assert.That(result.Single().ErrorMessage, Is.EqualTo("Wait time must be greater than or equal to 0."));
    }
}