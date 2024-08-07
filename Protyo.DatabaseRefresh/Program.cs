using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protyo.DatabaseRefresh.Jobs;
using Protyo.DatabaseRefresh.Jobs.Protyo.DatabaseRefresh.Jobs;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Contracts.Configuration;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;
using System.Net.Http;

namespace Protyo.DatabaseRefresh
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddHostedService<Worker>();
                            services.AddSingleton<IConfigurationSetting, ConfigSetting>();
                            services.AddSingleton<IDynamoService, DynamoService>();
                            services.AddSingleton<IMongoService<GrantDataObject>, MongoService<GrantDataObject>>();
                            services.AddSingleton<IMongoService<UserDataObject>, MongoService<UserDataObject>>();
                            services.AddSingleton<IHttpService, HttpService>();
                            services.AddSingleton(typeof(HttpClient));
                            services.AddSingleton(typeof(GrantAPI_DynamoDB_SyncJob));
                            services.AddSingleton(typeof(GrantAPI_GSheetDB_SyncJob));
                            services.AddSingleton(typeof(GrantAPI_MongoDB_SyncJob));
                            services.AddSingleton(typeof(StringCompressionHelper));

                        }).ConfigureLogging(loggingBuilder => {
                            /*   loggingBuilder.AddDebug().AddEventLog(eventLogSettings => {
                                       eventLogSettings.SourceName = "Protyo.DatabaseRefresh";
                                       eventLogSettings.LogName = "Application";
                               });*/
                            loggingBuilder
                                 .AddDebug()
                                 .AddConsole()
                                 .SetMinimumLevel(LogLevel.Information);
                        });
        
    }
}
