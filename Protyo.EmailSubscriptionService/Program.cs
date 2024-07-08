using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.EmailSubscriptionService.Jobs;
using Protyo.EmailSubscriptionService.Jobs.Contract;
using Protyo.EmailSubscriptionService.Services;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Contracts.Configuration;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;
using System.Configuration;

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
                    services.AddSingleton<ISubscriberJob, SubscriberJob>();
                    services.AddSingleton(typeof(GoogleSheetsHelper));
                    services.AddSingleton(typeof(ItemsMapper));

                }).ConfigureLogging(loggingBuilder => {
                    // Windows Event Viewer Configuration
                    /*loggingBuilder.AddDebug().AddEventLog(eventLogSettings => {
                          eventLogSettings.SourceName = "Protyo.EmailSubscriptionService";
                          eventLogSettings.LogName = "Application";
                        });*/
                    loggingBuilder
                        .AddDebug()
                        .AddConsole()
                        .SetMinimumLevel(LogLevel.Information);
                });
    }
}
