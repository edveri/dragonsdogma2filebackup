using DragonsDogma2FileBackupWorker.Extensions;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.RegisterServices(configuration);

var host = builder.Build();

try
{
    Log.Information("Starting host");
    await host.RunAsync();   
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Logger.Information("Stopping host");
    Console.Read();
    await Log.CloseAndFlushAsync();
}