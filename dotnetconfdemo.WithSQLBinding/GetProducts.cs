using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using dotnetconfdemo.Shared;

namespace dotnetconfdemo.WithSQLBinding
{
    public static class GetProducts
    {
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product")] HttpRequest req,
             [Sql("SELECT * FROM dbo.Product", CommandType = System.Data.CommandType.Text, 
            ConnectionStringSetting = "ConnectionString")] IEnumerable<Product> products,
            ILogger log)
        {
            return new OkObjectResult(products);
        }
    }
}
