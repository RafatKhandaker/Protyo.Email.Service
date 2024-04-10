using Microsoft.Extensions.Configuration;
using Protyo.Utilities.Configuration.Contracts;

namespace Protyo.Utilities.Contracts.Configuration
{
    public class ConfigSetting : IConfigurationSetting
    {

        public IConfiguration appSettings {get; set;}

        public ConfigSetting() => appSettings = new ConfigurationBuilder()
                                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                            .Build();


    }
}
