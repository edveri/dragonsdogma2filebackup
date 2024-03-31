namespace DragonsDogma2FileBackupWorker.Logic.Abstract;

public interface IApplicationFileAndDirectoryHelper
{
    Task CreateBackupDirAndBatchFileAsync();
}