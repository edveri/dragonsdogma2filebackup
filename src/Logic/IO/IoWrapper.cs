namespace DragonsDogma2FileBackupWorker.Logic.IO;

public class IoWrapper : IIoWrapper
{
    public async Task WriteAllTextAsync(string path, string contents) => 
        await File.WriteAllTextAsync(path, contents);

    public async Task<string[]> ReadAllLinesAsync(string path) => 
        await File.ReadAllLinesAsync(path);
    
    public void Copy(string sourceFileName, string destFileName) => 
        File.Copy(sourceFileName, destFileName);
    
    public string[] GetDirectories(string path) => 
        Directory.GetDirectories(path);
    
    public void CreateDirectory(string path) => 
        Directory.CreateDirectory(path);

    public void CopyDirectory(string sourceDirName, string destDirName) => 
        FileSystem.CopyDirectory(sourceDirName, destDirName);

    public async Task WriteAllLinesAsync(string path, IEnumerable<string> content) => 
        await File.WriteAllLinesAsync(path, content);
    public bool DirectoryExists(string path) => Directory.Exists(path);
    
    public string CombinePath(params string[] paths) 
        => Path.Combine(paths);
    
    public bool ProcessExists(string processName) =>
         Process.GetProcessesByName(processName).Length > 0;
    public bool IsWindowsOs() => 
        OperatingSystem.IsWindows();
    public string GetRegistryKeyValue(string keyName) =>
        Registry.GetValue(keyName, "InstallPath", null)?.ToString() ?? string.Empty;
    public IDirectoryInfoWrapper GetDirectoryInfo(string path) => 
        new DirectoryInfoWrapper(new DirectoryInfo(path));
}