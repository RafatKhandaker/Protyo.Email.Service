using Microsoft.Extensions.Configuration;
using Protyo.Utilities.Models.cors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services
{
    public class CorsConfiguration
    {
        private static readonly CorsConfiguration CorsConfig = new CorsConfiguration();

        private IConfigurationRoot Configuration;

        public CorsConfigurationSettings CorsConfigurationSettings { get; set; }

        public static CorsConfiguration GetInstance() => CorsConfig;
        private CorsConfiguration()
        {
            CorsConfigurationSettings = new CorsConfigurationSettings();

            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                            .AddEnvironmentVariables()
                                                                .Build();

            Configuration.GetSection("CorsConfigurationSettings").Bind(CorsConfigurationSettings);

        }
    }
}
