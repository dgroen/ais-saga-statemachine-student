using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using RegisterStudent.Consumers;
using RegisterStudent.Models;
using RegisterStudent.Services;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                // find the shared folder in the parent folder
                var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Shared");

                config
                    .AddJsonFile(Path.Combine($"{sharedFolder}", "SharedSettings.json"), optional: true)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

                config.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}