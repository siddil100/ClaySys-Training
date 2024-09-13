using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MovieTicketBooking.Models;
namespace MovieTicketBooking.Repositories
{
    public class HomeRepository
    {
        private readonly string _connectionString;

        public HomeRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        /// <summary>
        /// Used to submit the enquiry form
        /// </summary>
        /// <param name="contactUs"></param>
        /// <exception cref="Exception"></exception>
        public void SubmitEnquiry(ContactUs contactUs)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPI_Enquiry", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@firstName", contactUs.FirstName);
                        command.Parameters.AddWithValue("@lastName", contactUs.LastName);
                        command.Parameters.AddWithValue("@Email", contactUs.Email);
                        command.Parameters.AddWithValue("@Message", contactUs.Message);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception("An error occurred while submitting the enquiry.", ex);
            }
        }
    }
}
