namespace DragonsDogma2FileBackupWorker.Logic.IO.Abstract;

public interface IIoWrapper
{
    Task WriteAllLinesAsync(string path, IEnumerable<string> content);
    Task WriteAllTextAsync(string path, string contents);
    Task<string[]> ReadAllLinesAsync(string path);
    void Copy(string sourceFileName, string destFileName);
    public string[] GetDirectories(string path);
    void CreateDirectory(string path);
    bool DirectoryExists(string path);
    void CopyDirectory(string sourceDirName, string destDirName);
    string CombinePath(params string[] paths);
    bool ProcessExists(string processName);
    bool IsWindowsOs();
    string GetRegistryKeyValue(string keyName);
    IDirectoryInfoWrapper GetDirectoryInfo(string path);
}