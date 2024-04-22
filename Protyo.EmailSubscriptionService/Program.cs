using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.EmailSubscriptionService.Services;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Contracts.Configuration;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;

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
                    services.AddSingleton(typeof(ItemsMapper));

                });
    }
}
