using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using BCrypt.Net;
using System.Diagnostics;
using System.Threading.Tasks;
namespace MovieTicketBooking.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly string dbcon;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
            dbcon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        /// <summary>
        /// Used to update user's details
        /// </summary>
        /// <param name="userDetails"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateUserDetails(UserDetails userDetails)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(_connectionString);
                cmd = new SqlCommand("SPU_UserDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@userId", userDetails.UserId);
                cmd.Parameters.AddWithValue("@firstName", userDetails.FirstName);
                cmd.Parameters.AddWithValue("@lastName", userDetails.LastName);
                cmd.Parameters.AddWithValue("@Address", userDetails.Address);
                cmd.Parameters.AddWithValue("@stateId", userDetails.StateId);
                cmd.Parameters.AddWithValue("@cityId", userDetails.CityId);
                cmd.Parameters.AddWithValue("@Dob", userDetails.Dob);
                cmd.Parameters.AddWithValue("@Gender", userDetails.Gender);
                cmd.Parameters.AddWithValue("@phoneNumber", userDetails.PhoneNumber);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// Used to get userdetails by specific id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserDetails GetUserDetailsById(int userId)
        {
            UserDetails userDetails = null;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(_connectionString);
                cmd = new SqlCommand("SPS_UserDetailsById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userDetails = new UserDetails
                    {
                        UserId = (int)reader["user_id"],
                        FirstName = reader["first_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        Dob = reader["dob"] != DBNull.Value ? (DateTime?)reader["dob"] : null,
                        Gender = reader["gender"].ToString(),
                        PhoneNumber = reader["phone_number"].ToString(),
                        Address = reader["address"].ToString(),
                        StateId = (int)reader["state_id"],
                        CityId = (int)reader["city_id"]
                    };
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return userDetails;
        }

        /// <summary>
        /// Used to load cities based on states
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetCities_Ajax(int stateId)
        {
            var cities = new List<SelectListItem>();

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

            return cities;
        }

        /// <summary>
        /// Used to update password of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdatePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                Debug.WriteLine($"Starting UpdatePassword for UserId: {userId}");
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    Debug.WriteLine("Opening SQL connection...");
                    using (SqlCommand cmd = new SqlCommand("SPU_GetUserPassword", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        conn.Open();
                        Debug.WriteLine("Fetching existing hashed password from the database...");
                        string existingHashedPassword = cmd.ExecuteScalar()?.ToString();
                        if (string.IsNullOrEmpty(existingHashedPassword))
                        {
                            Debug.WriteLine("User not found.");
                            throw new Exception("User not found.");
                        }
                        Debug.WriteLine($"Existing Hashed Password: {existingHashedPassword}");
                        Debug.WriteLine("Verifying the old password...");
                        if (!BCrypt.Net.BCrypt.Verify(oldPassword, existingHashedPassword))
                        {
                            Debug.WriteLine("The old password is incorrect.");
                            throw new UnauthorizedAccessException("The old password is incorrect.");
                        }
                        Debug.WriteLine("Hashing the new password...");
                        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                        Debug.WriteLine($"Newly Hashed Password: {hashedNewPassword}");
                        Debug.WriteLine("Updating the password in the database...");
                        cmd.CommandText = "SPU_UpdatePassword";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@NewPassword", hashedNewPassword);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Debug.WriteLine($"Rows affected: {rowsAffected}");
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new Exception("An error occurred while updating the password.", ex);
            }
        }



        /// <summary>
        /// To get the states 
        /// </summary>
        /// <returns>It will return the state names</returns>
        public List<SelectListItem> GetStates()
        {
            var states = new List<SelectListItem>();


            using (SqlConnection conn = new SqlConnection(dbcon))
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

            return states;
        }
        /// <summary>
        /// To get cities when loading the edit form initially
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns> will return cities names</returns>
        public List<SelectListItem> GetCities(int stateId)
        {
            var cities = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(dbcon))
            {
                using (SqlCommand cmd = new SqlCommand("SPS_GetCities", conn))
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
                                Value = reader["Value"].ToString(),
                                Text = reader["Text"].ToString()
                            });
                        }
                    }
                }
            }

            return cities;
        }

        /// <summary>
        /// Asynchronous method to load movies to userhome 
        /// </summary>
        /// <returns>A list of movies</returns>
        public async Task<List<Movie>> GetMoviesAsync()
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPS_GetMoviesUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            movies.Add(new Movie
                            {
                                MovieId = reader.GetInt32(0),
                                MovieName = reader.GetString(1),
                                Genre = reader.GetString(2),
                                Description = reader.GetString(3),
                                Duration = reader.GetInt32(4),
                                ReleaseDate = reader.GetDateTime(5),
                                Language = reader.GetString(6),
                                MoviePoster = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return movies;
        }

        /// <summary>
        /// Used to retireve showtimes by id of a movie for booking
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public List<ShowTime> GetShowtimesByMovieId(int movieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SPS_GetShowTimesByMovieId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@movieId", movieId);

                connection.Open();
                var reader = command.ExecuteReader();

                var showtimes = new List<ShowTime>();
                while (reader.Read())
                {
                    var showtime = new ShowTime
                    {
                        ShowtimeId = (int)reader["showTimeId"],
                        MovieId = (int)reader["movieId"],
                        ShowDate = (DateTime)reader["showDate"],
                        StartTime = (TimeSpan)reader["startTime"],
                        ScreenNumber = (int)reader["screenNumber"]
                    };
                    showtimes.Add(showtime);
                }
                return showtimes;
            }

        }

        /// <summary>
        /// Used to view details of the movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public Movie GetMovieDetailsById(int movieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SPS_GetMovieDetailsById", connection); // Assume you have a stored procedure
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@movieId", movieId);

                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Movie
                    {
                        MovieId = reader.GetInt32(0),
                        MovieName = reader.GetString(1),
                        Description = reader.GetString(2),
                        Duration = reader.GetInt32(3),
                        Genre = reader.GetString(4),
                        ReleaseDate = reader.GetDateTime(5),
                        Language = reader.GetString(6),
                        MoviePoster = reader.GetString(7),  // Assuming the MoviePoster column holds a base64 string
                        Actor = reader.GetString(8),
                        Actress = reader.GetString(9),
                        Director = reader.GetString(10)
                    };
                }
            }
            return null;
        }
        /// <summary>
        /// Used to get the seats by specific showtime
        /// </summary>
        /// <param name="showtimeId"></param>
        /// <returns>The seats for booking the show</returns>
        public List<Seat> GetAllSeatsByShowtimeId(int showtimeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SPS_GetAllSeatsByShowtimeId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@showtimeId", showtimeId);

                connection.Open();
                var reader = command.ExecuteReader();

                var seats = new List<Seat>();
                while (reader.Read())
                {
                    var seat = new Seat
                    {
                        SeatId = (int)reader["seatId"],
                        RowNumber = (string)reader["rowNumber"],
                        ColumnNumber = (int)reader["columnNumber"],
                        SeatTypeId = (int)reader["seatTypeId"],
                        ScreenNumber = (int)reader["screenNumber"],
                        IsBooked = (bool)reader["isBooked"]
                    };
                    seats.Add(seat);
                }
                return seats;
            }

        }

        /// <summary>
        /// To fetch prices based on seat types
        /// </summary>
        /// <returns></returns>
        public List<SeatType> GetSeatPrices()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var seatPrices = new List<SeatType>();

                using (var command = new SqlCommand("SPS_GetSeatPrices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            seatPrices.Add(new SeatType
                            {
                                SeatTypeId = reader.GetInt32(0),
                                SeatTypeName = reader.GetString(1),
                                Price = reader.GetDecimal(2)
                            });
                        }
                    }
                }

                return seatPrices;
            }
        }
        /// <summary>
        /// Used to book the show
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="showtimeId"></param>
        /// <param name="totalAmount"></param>
        /// <param name="seatIds"></param>
        public void InsertBooking(int userId, int showtimeId, decimal totalAmount, string seatIds)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SPI_InsertBooking", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ShowtimeId", showtimeId);
                command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                command.Parameters.AddWithValue("@PaymentStatus", "Paid");
                command.Parameters.AddWithValue("@CancellationStatus", 0);
                command.Parameters.AddWithValue("@SeatIds", seatIds);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Used to show the info of currently booked tickets
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Booking GetLatestBookingForUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SPS_GetLatestBookingForUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Booking booking = null;
                        List<string> seatDescriptions = new List<string>();

                        if (reader.Read()) // Read booking details
                        {
                            booking = new Booking
                            {
                                BookingId = (int)reader["bookingId"],
                                UserId = (int)reader["userId"],
                                ShowtimeId = (int)reader["showtimeId"],
                                TotalAmount = (decimal)reader["totalAmount"],
                                BookingDate = (DateTime)reader["bookingDate"]
                            };
                        }

                        // Move to the next result set
                        if (reader.NextResult())
                        {
                            while (reader.Read()) // Reading seat details
                            {
                                string rowNumber = reader["rowNumber"].ToString();
                                int columnNumber = (int)reader["columnNumber"];
                                seatDescriptions.Add($"{rowNumber}{columnNumber}");
                            }
                        }

                        // Convert list to a comma-separated string
                        if (booking != null)
                        {
                            booking.SeatDescriptions = string.Join(", ", seatDescriptions);
                        }

                        return booking;
                    }
                }
            }
        }

        /// <summary>
        /// Used to apply filter for movies by language and genre
        /// </summary>
        /// <param name="language"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task<List<Movie>> GetFilteredMoviesAsync(string language, string genre)
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPS_FilterMovies", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@language", language ?? string.Empty);
                    command.Parameters.AddWithValue("@genre", genre ?? string.Empty);

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            movies.Add(new Movie
                            {
                                MovieId = reader.GetInt32(0),
                                MovieName = reader.GetString(1),
                                Genre = reader.GetString(2),
                                Description = reader.GetString(3),
                                Duration = reader.GetInt32(4),
                                ReleaseDate = reader.GetDateTime(5),
                                Language = reader.GetString(6),
                                MoviePoster = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return movies;
        }

        /// <summary>
        /// Used to get user bookings
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of booked movies</returns>
        public IEnumerable<UserBookings> GetUserBookings(int userId)
        {
            var bookings = new List<UserBookings>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SPS_GetUserBookings", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookings.Add(new UserBookings
                        {
                            BookingId = reader.GetInt32(0),
                            MovieName = reader.GetString(1),
                            ShowDate = reader.GetDateTime(2),
                            StartTime = reader.GetTimeSpan(3),
                            SeatNames = reader.GetString(4), 
                            TotalAmount = reader.GetDecimal(5)
                        });
                    }
                }
            }

            return bookings;
        }

    }
}

        
