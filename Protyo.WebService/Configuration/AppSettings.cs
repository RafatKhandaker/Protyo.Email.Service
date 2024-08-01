using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Protyo.WebService.Configuration
{
    public static class AppSettings
    {
        public static readonly string ApiSuccessRedirectUri = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                .AddEnvironmentVariables()
                                                                    .Build()
                                                                        .GetSection("ApiSuccessRedirectUri")
                                                                           .Value;

        public static readonly string ApiCanceledRedirectUri = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                .AddEnvironmentVariables()
                                                                    .Build()
                                                                        .GetSection("ApiCanceledRedirectUri")
                                                                           .Value;
    }
}
