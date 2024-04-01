namespace DragonsDogma2FileBackupWorker.Logic.IO.Abstract;

public interface IDirectoryInfoWrapper
{
    string Name { get; }
    DateTime CreationTime { get; }
    void Delete(bool recursive);
}