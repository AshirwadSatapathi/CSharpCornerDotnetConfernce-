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

namespace dotnetconfdemo.WithSQLBinding
{
    public static class CreateProduct
    {
        [FunctionName("CreateProduct")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product")] HttpRequest req,
            [Sql("dbo.Product", ConnectionStringSetting = "ConnectionString")] out Product product,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Product>(requestBody);

            product = data;


            return new CreatedResult($"/api/product", product);
        }
    }
}
