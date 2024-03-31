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
host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping.Register(() =>
{
    Log.Logger.Information("*** Stopping host. Enter any key to exit. ***");
});

try
{
    Log.Logger.Information("*** Starting host ***");
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
    Console.Read();
}