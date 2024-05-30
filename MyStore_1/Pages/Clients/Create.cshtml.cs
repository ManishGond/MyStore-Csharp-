using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore_1.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string? name = Request.Form["name"];
            string? email = Request.Form["email"];
            string? phone = Request.Form["phone"];
            string? address = Request.Form["address"];

            clientInfo.name = name ?? string.Empty;
            clientInfo.email = email ?? string.Empty;
            clientInfo.phone = phone ?? string.Empty;
            clientInfo.address = address ?? string.Empty;

            if (string.IsNullOrWhiteSpace(clientInfo.name) ||
                string.IsNullOrWhiteSpace(clientInfo.email) ||
                string.IsNullOrWhiteSpace(clientInfo.phone) ||
                string.IsNullOrWhiteSpace(clientInfo.address))
            {
                errorMessage = "All the fields are required";
                return Page();
            }

            //to add it to the database
            try
            {
                String connectionString = "Data Source=.\\SQL220;Initial Catalog=mystore;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " +
                                "(name, email, phone, address) VALUES " +
                                "(@name, @email, @phone, @address);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
                clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
                successMessage = "New Client Added Successfully!";
                return RedirectToPage("/Clients/Index", new { successMessage = successMessage });
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }

            
        }

    }
}
