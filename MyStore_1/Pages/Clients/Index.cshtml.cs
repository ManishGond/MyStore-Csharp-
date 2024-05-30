using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace MyStore_1.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<ClientInfo> ListClients { get; set; } = new List<ClientInfo>();

        public string? SuccessMessage { get; set; }

        public void OnGet(string successMessage)
        {
            try
            {
                SuccessMessage = successMessage;
                string connectionString = "Data Source=.\\SQL220;Initial Catalog=mystore;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    _logger.LogInformation("Database connection opened successfully.");
                    string sql = "SELECT * FROM clients";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo
                                {
                                    id = "" + reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    phone = reader.GetString(3),
                                    address = reader.GetString(4),
                                    createdAt = reader.GetDateTime(5)
                                };

                                ListClients.Add(clientInfo);
                            }
                        }
                    }
                    _logger.LogInformation($"Number of clients retrieved: {ListClients.Count}");
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Exception: {sqlEx.Message}");
                _logger.LogError(sqlEx.StackTrace);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError($"Argument Exception: {argEx.Message}");
                _logger.LogError(argEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"General Exception: {ex.Message}");
                _logger.LogError(ex.StackTrace);
            }
        }
    }

    public class ClientInfo
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public DateTime createdAt { get; set; }
    }
}
