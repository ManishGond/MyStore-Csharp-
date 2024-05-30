using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace MyStore_1.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public EditModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String? id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=.\\SQL220;Initial Catalog=mystore;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    _logger.LogInformation("Database connection opened successfully.");
                    string sql = "SELECT * FROM clients WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add the parameter for id
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Your existing code to populate clientInfo
                                clientInfo = new ClientInfo
                                {
                                    id = "" + reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    phone = reader.GetString(3),
                                    address = reader.GetString(4),
                                    createdAt = reader.GetDateTime(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }


        public void OnPost() 
        {
            string? id = Request.Query["id"];
            string? name = Request.Form["name"];
            string? email = Request.Form["email"];
            string? phone = Request.Form["phone"];
            string? address = Request.Form["address"];

            clientInfo.id = id ?? string.Empty;
            clientInfo.name = name ?? string.Empty;
            clientInfo.email = email ?? string.Empty;
            clientInfo.phone = phone ?? string.Empty;
            clientInfo.address = address ?? string.Empty;

            if (string.IsNullOrWhiteSpace(clientInfo.name) ||
                string.IsNullOrWhiteSpace(clientInfo.email) ||
                string.IsNullOrWhiteSpace(clientInfo.phone) ||
                string.IsNullOrWhiteSpace(clientInfo.address) ||
                string.IsNullOrWhiteSpace(clientInfo.id))
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=.\\SQL220;Initial Catalog=mystore;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    _logger.LogInformation("Database connection opened successfully.");
                    string sql = "UPDATE clients "+
                                "SET name=@name, email=@email, phone=@phone, address=@address "+
                                "WHERE id=@id ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex) 
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");

        }
    }
}
