using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InsERT_Demo_HS.Data;
using InsERT_Demo_HS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InsERT_Demo_HS.Controllers
{
    [ApiController] //Enable automatic model data validation
    [Route("NbpApi")] //Base route for the controller
    public class NbpApiController : Controller
    {
        private readonly IHttpClientFactory myhttpClientFactory;
        public NbpApiController(IHttpClientFactory httpClientFactory)

        {
            myhttpClientFactory = httpClientFactory;
        }

        [HttpGet("GetApiData")]
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> GetApiData()
        {
            try
            {
                string table = "a"; //a – tabela kursów średnich walut obcych;
                string url = $"http://api.nbp.pl/api/exchangerates/tables/{table}/"; //Aktualnie obowiązująca tabela kursów typu {table}
                var client = myhttpClientFactory.CreateClient(); //create a httpclient instance using the factory
                var response = await client.GetAsync(url); //make the request to the NBP Api
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync(); //return response
                    var exchangeData = JsonSerializer.Deserialize<List<ExchangeModel>>(data); //Deserialise the data into my model
                    TempData["dataToSend"] = data; //Store the data in TempData
                    return PartialView("_Exchange", exchangeData); //Display the result
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(); //404 response
                    }
                    else
                    {
                        return BadRequest("GET Request failed"); //400 response for other errors
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); //log the exception
                return StatusCode(500, "Internal Server Error"); //500 response for internal server errors
            }
        }
    }
}
