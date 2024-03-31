namespace DragonsDogma2FileBackupWorker.Extensions;
public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddLogic(services);
        AddOptions(services,configuration);
        AddSerilog(services);
        AddWorker(services);
    }

    private static void AddLogic(this IServiceCollection services)
    {
        services.AddTransient<ICopySaveFiles, CopySaveFiles>();
        services.AddTransient<ISteamPathInitializer, SteamPathInitializer>();
        services.AddTransient<IApplicationFileAndDirectoryHelper, ApplicationFileAndDirectoryHelper>();
        services.AddTransient<ILaunchOptionsEditor, LaunchOptionsEditor>();
        services.AddTransient<IIoWrapper, IoWrapper>();
        services.AddTransient<IVdfFileHelper, VdfFileHelper>();
        services.AddSingleton<IDirectoryStorage, DirectoryStorage>();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<CopyOptions>(configuration.GetSection(nameof(CopyOptions)));
    
    private static void AddSerilog(this IServiceCollection services) =>
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });
    
    private static void AddWorker(this IServiceCollection services) => 
        services.AddHostedService<Worker>();
}