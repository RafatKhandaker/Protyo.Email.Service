using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Protyo.DatabaseRefresh.Jobs;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Contracts.Configuration;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Protyo.DatabaseRefresh
{
    public class Program
    {
        public static void Main(string[] args) =>  CreateHostBuilder(args).Build().Run();
        

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IConfigurationSetting, ConfigSetting>();
                    services.AddSingleton<IDynamoService, DynamoService>();
                    services.AddSingleton<IHttpService, HttpService>();
                    services.AddSingleton(typeof(HttpClient));
                    services.AddSingleton(typeof(GrantAPI_DynamoDB_SyncJob));

                });
    }
}
