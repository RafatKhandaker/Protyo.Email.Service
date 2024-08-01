using Microsoft.Extensions.Configuration;
using Protyo.Utilities.Models.configuration;
using System.IO;

namespace Protyo.Utilities.Services
{
    public class StripeOptionConfiguration
    {
        private static readonly StripeOptionConfiguration StripeOptionConfig = new StripeOptionConfiguration();

        private IConfigurationRoot Configuration;

        public StripeOptions StripeOptions { get; set; }

        public static StripeOptionConfiguration GetInstance() => StripeOptionConfig;
        private StripeOptionConfiguration()
        {
            StripeOptions = new StripeOptions();

            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                            .AddEnvironmentVariables()
                                                                .Build();

            Configuration.GetSection("StripeConfigurationSettings").Bind(StripeOptions);

        }

    }
}
