using WebSpider_bqg.Pages;
using Serilog.Events;
using Serilog;

namespace WebSpider_bqg;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAntDesign();
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Host.UseSerilog();
        
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
        }
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/runLogs/runLogs-.txt", 
                shared: true,
                retainedFileCountLimit: 7,
                rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .WriteTo.File(
                "logs/errorLogs/errorLogs-.txt", 
                shared: true, 
                retainedFileCountLimit: 7,
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Error, 
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
        app.Run();
    }
}
