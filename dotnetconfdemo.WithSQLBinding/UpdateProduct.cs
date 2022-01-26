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
    public static class UpdateProduct
    {
        [FunctionName("UpdateProduct")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "product/{id:int}")] HttpRequest req,
            int id,
            [Sql("dbo.Product", ConnectionStringSetting = "ConnectionString")] out Product product,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject<Product>(requestBody);

            product = data;

            return new NoContentResult();
        }
    }
}
