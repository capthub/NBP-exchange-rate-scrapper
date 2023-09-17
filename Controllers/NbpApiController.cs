using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InsERT_Demo_HS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InsERT_Demo_HS.Controllers
{
    [ApiController] //Enable automatic model data validation
    [Route("NbpApi")]
    public class NbpApiController : Controller
    {
        private readonly IHttpClientFactory myhttpClientFactory;
        public NbpApiController(IHttpClientFactory httpClientFactory)

        {
            myhttpClientFactory = httpClientFactory;
        }
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")]
        [HttpGet("GetApiData")]
        public async Task<IActionResult> GetApiData()
        {
            try
            {
                string table = "a"; //a – tabela kursów średnich walut obcych;
                string url = $"http://api.nbp.pl/api/exchangerates/tables/{table}/"; //Aktualnie obowiązująca tabela kursów typu {table}
                var client = myhttpClientFactory.CreateClient(); //create an httpclient instance
                var response = await client.GetAsync(url); //make the request to the NBP Api
                if (response.IsSuccessStatusCode)
                {
                        var data = await response.Content.ReadAsStringAsync(); //return response
                        var exchangeData = JsonSerializer.Deserialize<List<ExchangeModel>>(data); //Deserialise the data into my model
                        return PartialView("_Exchange", exchangeData);
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
