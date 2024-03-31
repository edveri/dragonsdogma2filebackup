namespace DragonsDogma2FileBackupWorker;

public static class Constants
{
    public const string Steam32BitRegistryKeyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";
    public const string Steam64BitRegistryKeyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam";
    public const string DragonsDogma2Id = "2054970";
    public const string SteamUserDataDirectory = "userdata";
    public const string BatchFileWorkingDirectoryKey = "{WorkingDirectory}";
    public const string SteamAppsDirectory = @"steamapps\common\Dragons Dogma 2";
    public const string BatchFileName = "launch.bat";
    public static readonly string BatchFileContent = $"""
                                                      @echo off
                                                      start DD2.exe
                                                      pushd {BatchFileWorkingDirectoryKey}
                                                      start {BatchFileWorkingDirectoryKey}FileBackupWorker.exe"
                                                      exit
                                                      """;

    public const string ConfigDirectoryName = "config";
    public const string LocalConfigFileName = "localconfig.vdf";
    public const string LocalConfigEulaString = "2054970_eula";
    public const string LocalConfigPlaytimeString = "Playtime";
    public const string LaunchOptionsDirectoryKey = "{LaunchOptionsDirectoryKey}";
    public const string LaunchOptionsText = $@"						""LaunchOptions""		""\""{LaunchOptionsDirectoryKey}\"" %COMMAND%""";
    public const string DragonsDogma2ProcessName = "DD2";
    public const string LaunchOptionsSectionRoot = "LaunchOptions";
    public const char StartBracket = '{';
}