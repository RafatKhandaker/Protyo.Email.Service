using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.EmailSubscriptionService.Services;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Contracts.Configuration;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;


namespace Protyo.WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConfigurationSetting, ConfigSetting>();
                    services.AddSingleton<IDynamoService, DynamoService>();
                    services.AddSingleton(typeof(GoogleSheetsHelper));
                    services.AddScoped(typeof(ObjectExtensionHelper));

                });
    }
}
