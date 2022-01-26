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

namespace dotnetconfdemo.WithoutSQLBinding
{
    public static class GetProductById
    {
        [FunctionName("GetProductById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/{id:int}")] HttpRequest req, 
            int id,
            ILogger log)
        {
            log.LogInformation($"Product Id to be processed is {id}");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("localdb")))
                {
                    connection.Open();
                    var query = @"SELECT * FROM Product WHERE ProductId = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
            }
            if (dt.Rows.Count == 0)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(dt);
        }
    }
}
