using InsERT_Demo_HS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace InsERT_Demo_HS.Controllers
{
    public class DataController : Controller
    {
        private readonly IConfiguration config;
        public DataController(IConfiguration configuration)
        {
            config = configuration; //get access to configuration data

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
        }
        public IActionResult PostData()
        {
            if (TempData["dataToSend"] is string data) //check if the data is stored in TempData
            {
                var postData = JsonSerializer.Deserialize<List<ExchangeModel>>(data);
                string connectionString = config.GetConnectionString("AzureDatabase"); // my connection string securely stored in appsettings.json
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("TRUNCATE TABLE dbo.ExchangeData", conn);
                        cmd.ExecuteNonQuery(); // Clear existing table data

                        foreach (var i in postData[0].rates)
                        {
                            using (SqlCommand cmd2 = new SqlCommand("INSERT INTO dbo.ExchangeData (Currency, Code, Mid) VALUES (@Currency,@Code,@Mid)", conn))
                            {
                                cmd2.Parameters.AddWithValue("@Currency", i.currency);
                                cmd2.Parameters.AddWithValue("@Code", i.code);
                                cmd2.Parameters.AddWithValue("@Mid", i.mid);

                                cmd2.ExecuteNonQuery(); //Insert Data Row
                            }
                        }
                        conn.Close();
                    }
                    return Ok("Insert successful");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error inserting data");
                }
            }
            else
            {
                return StatusCode(404, "Error fetching data");
            }
        }

    }
}
