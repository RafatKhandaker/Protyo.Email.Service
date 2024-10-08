using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.EmailSubscriptionService.Services;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Contracts.Configuration;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
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
                    services.AddSingleton<IMongoService<GrantDataObject>, MongoService<GrantDataObject>>();
                    services.AddSingleton<IMongoService<UserDataObject>, MongoService<UserDataObject>>();
                    services.AddSingleton<ListCache<GrantDataObject>, ListCache<GrantDataObject>>();
                    services.AddSingleton<Cache<string, FormData>, Cache<string, FormData>>();
                    services.AddSingleton<Cache<long, UserDataObject>, Cache<long, UserDataObject>>();
                    services.AddSingleton(typeof(GoogleSheetsHelper));
                    services.AddScoped(typeof(ObjectExtensionHelper));
                    services.AddScoped(typeof(ItemsMapper));

                });
    }
}
