using Microsoft.Extensions.Configuration;
using ProgClientOIDC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProgClientOIDC.GetConf
{
    public static class GetConfSettings
    {
        public static ConfSettings GetSettings()
        {
            IConfiguration configuration;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath($@"{Directory.GetCurrentDirectory()}\..\..\..\")
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            return configuration.Get<ConfSettings>();
        }
    }
}
