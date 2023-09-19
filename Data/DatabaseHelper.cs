using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InsERT_Demo_HS.Data
{
    public class ConfigurationManager
    {
        public IConfiguration Configuration { get; }

        public ConfigurationManager()
        {
            // Load the appsettings.json file from the application's base path
            Configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
