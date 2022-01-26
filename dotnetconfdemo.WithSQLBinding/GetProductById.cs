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
    public static class GetProductById
    {
        [FunctionName("GetProductById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/{id:int}")] HttpRequest req,
            int id,
            [Sql("SELECT * FROM dbo.Product WHERE ProductId=@id",CommandType =System.Data.CommandType.Text,
            Parameters ="@Id={id}",ConnectionStringSetting ="ConnectionString")] IEnumerable<Product> products,
            ILogger log)
        {
            //log.LogInformation($"The product id is {id}");

            return new OkObjectResult(products);
        }
    }
}
