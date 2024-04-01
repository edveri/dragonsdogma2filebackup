namespace DragonsDogma2FileBackupWorker.UnitTests.Logic;

[TestFixture]
public class DirectoryAndSettingsFacadeTests
{
    private Mock<IDirectoryStorageBuilder> _directoryStorageBuilderMock = null!;
    private Mock<ISteamLaunchOptionsEditor> _steamLaunchOptionsEditorMock = null!;
    private Mock<IApplicationFileAndDirectoryHelper> _applicationFileAndDirectoryHelperMock = null!;
    
    private DirectoryAndSettingsFacade _directoryAndSettingsFacade = null!;
    
    [SetUp]
    public void SetUp()
    {
        _directoryStorageBuilderMock = new Mock<IDirectoryStorageBuilder>();
        _steamLaunchOptionsEditorMock = new Mock<ISteamLaunchOptionsEditor>();
        _applicationFileAndDirectoryHelperMock = new Mock<IApplicationFileAndDirectoryHelper>();
        
        _directoryAndSettingsFacade = new DirectoryAndSettingsFacade(_directoryStorageBuilderMock.Object, _steamLaunchOptionsEditorMock.Object, _applicationFileAndDirectoryHelperMock.Object);
    }
    
    [Test]
    public async Task InitializeAndSetSteamDirectoriesAsync_CallsBuildDirectoryStorage()
    {
        //Arrange
        //Act
        await _directoryAndSettingsFacade.InitializeAndSetSteamDirectoriesAsync();
        
        //Assert
        _directoryStorageBuilderMock.Verify(m => m.BuildDirectoryStorage(), Times.Once);
    }
    
    [Test]
    public async Task InitializeAndSetSteamDirectoriesAsync_CallsSetSteamLaunchOptionsAsync()
    {
        //Arrange
        //Act
        await _directoryAndSettingsFacade.InitializeAndSetSteamDirectoriesAsync();
        
        //Assert
        _steamLaunchOptionsEditorMock.Verify(m => m.SetSteamLaunchOptionsAsync(), Times.Once);
    }
    
    [Test]
    public async Task InitializeAndSetSteamDirectoriesAsync_CallsCreateBackupDirAndBatchFileAsync()
    {
        //Arrange
        //Act
        await _directoryAndSettingsFacade.InitializeAndSetSteamDirectoriesAsync();
        
        //Assert
        _applicationFileAndDirectoryHelperMock.Verify(m => m.CreateBackupDirAndBatchFileAsync(), Times.Once);
    }
}