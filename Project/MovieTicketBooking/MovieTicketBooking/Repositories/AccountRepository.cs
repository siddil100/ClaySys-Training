using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using MovieTicketBooking.Models;
using System.Configuration;
using System.Data;
using BCrypt.Net;
namespace MovieTicketBooking.Repositories
{
    public class AccountRepository
    {
        private readonly string _connectionString;


        public AccountRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        /// <summary>
        /// Used to Register a new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userDetails"></param>
        /// <exception cref="Exception"></exception>
        public void RegisterUser(User user, UserDetails userDetails)
        {
            SqlConnection conn = null;
            try
            {
                // Hash the password using BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                conn = new SqlConnection(_connectionString);
                using (SqlCommand cmd = new SqlCommand("SPI_RegisterUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string role = "user";

                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@firstName", userDetails.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", userDetails.LastName);
                    cmd.Parameters.AddWithValue("@Dob", userDetails.Dob);
                    cmd.Parameters.AddWithValue("@Gender", userDetails.Gender);
                    cmd.Parameters.AddWithValue("@phoneNumber", userDetails.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", userDetails.Address);
                    cmd.Parameters.AddWithValue("@stateId", userDetails.StateId);
                    cmd.Parameters.AddWithValue("cityId", userDetails.CityId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }



        /// <summary>
        /// Used for login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>A user if email and password matches</returns>
        public User AuthenticateUser(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Authenticate_User", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isActive = (bool)reader["isActive"];
                            if (!isActive)
                            {
                                return new User
                                {
                                    IsActive = false
                                };
                            }

                            string hashedPassword = (string)reader["password"];

                            // Verify the password against the hashed password
                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                            {
                                int userId = (int)reader["user_id"];
                                string role = (string)reader["role"];
                                string emailAddress = (string)reader["email"];

                                // Close the first reader before running the next query
                                reader.Close();

                                // Fetch firstname and lastname
                                using (SqlCommand cmdDetails = new SqlCommand("SPS_UserDetails_Session", conn))
                                {
                                    cmdDetails.CommandType = CommandType.StoredProcedure;
                                    cmdDetails.Parameters.AddWithValue("@UserId", userId);

                                    using (SqlDataReader detailsReader = cmdDetails.ExecuteReader())
                                    {
                                        if (detailsReader.Read())
                                        {
                                            string firstName = (string)detailsReader["first_name"];
                                            string lastName = (string)detailsReader["last_name"];

                                            // Set the first name and last name in the session
                                            HttpContext.Current.Session["Firstname"] = firstName;
                                            HttpContext.Current.Session["Lastname"] = lastName;
                                        }
                                    }
                                }
                                return new User
                                {
                                    UserId = userId,
                                    Email = emailAddress,
                                    Role = role,
                                    IsActive = true
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Used to get cities in edit profile
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetCities(int stateId)
    {
        var cities = new List<SelectListItem>();

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SPS_GetCities_Ajax", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StateId", stateId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cities.Add(new SelectListItem
                            {
                                Value = reader["city_id"].ToString(),
                                Text = reader["city_name"].ToString()
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
        return cities;
    }
        /// <summary>
        /// Used to check whether email is already registered
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True or false</returns>
        /// <exception cref="Exception"></exception>
        public bool CheckEmailExists(string email)
        {
            bool emailExists = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CheckEmailExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        emailExists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception("An error occurred while checking the email.", ex);
            }
            return emailExists;
        }
        /// <summary>
        /// Used to get states when signing up
        /// </summary>
        /// <returns>A set of states</returns>
        public List<SelectListItem> GetStates()
        {
            var states = new List<SelectListItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPS_GetStates", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                states.Add(new SelectListItem
                                {
                                    Value = reader["state_id"].ToString(),
                                    Text = reader["state_name"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return states;
        }
    }
}