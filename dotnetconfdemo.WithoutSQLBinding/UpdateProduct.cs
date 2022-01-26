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
    public static class UpdateProduct
    {
        [FunctionName("UpdateProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "product/{id:int}")] HttpRequest req,
            int id,
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
                    var query = @"Update Product Set ProductName = @ProductName, ProductDescription = @ProductDescription, ProductPrice = @ProductPrice, ProductQuantity = @ProductQuantity Where ProductId = @ProductId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductId", id);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@ProductDescription",product.ProductDescription);
                    command.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
                    command.Parameters.AddWithValue("@ProductQuantity", product.ProductQuantity);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
            }
            return new OkResult();

        }
    }
}
