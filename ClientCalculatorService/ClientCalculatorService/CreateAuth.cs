using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClientCalculatorService
{
    public static class CreateAuth
    {
        public static AuthConfig ReadJsonFromFile(string path)
        {
            IConfiguration configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath($@"{Directory.GetCurrentDirectory()}\..\..\..\")
                .AddJsonFile(path);

            configuration = builder.Build();

            return configuration.Get<AuthConfig>();
        }
    }
}
