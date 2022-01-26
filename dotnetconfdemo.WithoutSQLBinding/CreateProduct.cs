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
using System.Data.SqlClient;

namespace dotnetconfdemo.WithoutSQLBinding
{
    public static class CreateProduct
    {
        [FunctionName("CreateProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBody);
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("localdb")))
                {
                    connection.Open();
                    if (String.IsNullOrEmpty(product.ProductName) || String.IsNullOrEmpty(product.ProductDescription))
                    {
                        return new BadRequestObjectResult("Bad payload!!!");
                    }
                    else
                    {
                        var query = $"INSERT INTO [Product] (ProductId, ProductName, ProductDescription, ProductPrice, ProductQuantity) VALUES({product.ProductId},'{product.ProductName}', '{product.ProductDescription}' , {product.ProductPrice},{product.ProductQuantity})";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new BadRequestResult();
            }
            return new OkResult();
        }
    }
}
