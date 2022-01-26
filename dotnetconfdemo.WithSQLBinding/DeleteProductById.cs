using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using dotnetconfdemo.Shared;
using System.Collections.Generic;

namespace dotnetconfdemo.WithSQLBinding
{
    public static class DeleteProductById
    {
        [FunctionName("DeleteProductById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "product/{id:int}")] HttpRequest req,
            int id,ILogger log,
            [Sql("[DeleteProductById]", CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = "@Id={id}", ConnectionStringSetting = "ConnectionString")]  IEnumerable<Product> products)
           
        {
            return new OkObjectResult(products);
        }
    }
}
