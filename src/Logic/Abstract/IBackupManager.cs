namespace DragonsDogma2FileBackupWorker.Logic.Abstract;

public interface IBackupManager
{
    Task ExecuteBackupsAsync();
}