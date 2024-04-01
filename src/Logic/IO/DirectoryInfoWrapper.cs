namespace DragonsDogma2FileBackupWorker.Logic.IO;

public class DirectoryInfoWrapper(DirectoryInfo directoryInfo) : IDirectoryInfoWrapper
{
    private readonly DirectoryInfo _directoryInfo = directoryInfo ?? throw new ArgumentNullException(nameof(directoryInfo));

    public string Name => _directoryInfo.Name;
    public DateTime CreationTime => _directoryInfo.CreationTime;
    public void Delete(bool recursive) => _directoryInfo.Delete(recursive);
}