using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Protyo.EmailSubscriptionService.Configuration;
using Protyo.EmailSubscriptionService.Configuration.Contracts;
using Protyo.EmailSubscriptionService.Services;
using Protyo.EmailSubscriptionService.Services.Contracts;


namespace Protyo.EmailSubscriptionService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IConfigurationSetting, ConfigSetting>();
                    services.AddSingleton<IEmailService, EmailService>();
                    services.AddSingleton(typeof(GoogleSheetsHelper));
                });
    }
}
