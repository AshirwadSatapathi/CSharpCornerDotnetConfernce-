using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using dotnetconfdemo.Shared;
using System.Collections.Generic;

namespace dotnetconfdemo.WithoutSQLBinding
{
    public static class GetProducts
    {
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            List<Product> productList = new List<Product>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("localdb")))
                {
                    connection.Open();
                    var query = @"SELECT * FROM Product";
                    SqlCommand command = new SqlCommand(query, connection);
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Product product = new Product()
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = reader["ProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            ProductPrice = (int)reader["ProductPrice"],
                            ProductQuantity = (int)reader["ProductQuantity"]
                        };
                        productList.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
            }
            if (productList.Count > 0)
            {
                return new OkObjectResult(productList);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
