using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MovieTicketBooking
{
    public class AdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Used to add a new movie
        /// </summary>
        /// <param name="movie"></param>
        public void AddMovie(Movie movie)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPI_Movie", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@movieName", movie.MovieName);
                    command.Parameters.AddWithValue("@description", movie.Description);
                    command.Parameters.AddWithValue("@duration", movie.Duration);
                    command.Parameters.AddWithValue("@genre", movie.Genre);
                    command.Parameters.AddWithValue("@releaseDate", movie.ReleaseDate);
                    command.Parameters.AddWithValue("@language", movie.Language);
                    command.Parameters.AddWithValue("@moviePoster", movie.MoviePoster); // base64 string
                    command.Parameters.AddWithValue("@actor", movie.Actor);
                    command.Parameters.AddWithValue("@actress", movie.Actress);
                    command.Parameters.AddWithValue("@director", movie.Director);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Used to list added movies
        /// </summary>
        /// <returns></returns>
        public List<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPS_AllMovies", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movies.Add(new Movie
                            {
                                MovieId = reader.GetInt32(reader.GetOrdinal("movieId")),
                                MovieName = reader.GetString(reader.GetOrdinal("movieName")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                Duration = reader.GetInt32(reader.GetOrdinal("duration")),
                                Genre = reader.GetString(reader.GetOrdinal("genre")),
                                ReleaseDate = reader.GetDateTime(reader.GetOrdinal("releaseDate")),
                                Language = reader.GetString(reader.GetOrdinal("language")),
                                MoviePoster = reader.GetString(reader.GetOrdinal("moviePoster")), // base64 string
                                Actor = reader.GetString(reader.GetOrdinal("actor")),
                                Actress = reader.GetString(reader.GetOrdinal("actress")),
                                Director = reader.GetString(reader.GetOrdinal("director"))
                            });
                        }
                    }
                }
            }

            return movies;
        }



        /// <summary>
        /// Used to get a movie by id
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>The details of that movie</returns>
        public Movie GetMovieById(int movieId)
        {
            Movie movie = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPS_MovieById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@movieId", movieId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            movie = new Movie
                            {
                                MovieId = reader.GetInt32(reader.GetOrdinal("movieId")),
                                MovieName = reader.GetString(reader.GetOrdinal("movieName")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                Duration = reader.GetInt32(reader.GetOrdinal("duration")),
                                Genre = reader.GetString(reader.GetOrdinal("genre")),
                                ReleaseDate = reader.GetDateTime(reader.GetOrdinal("releaseDate")),
                                Language = reader.GetString(reader.GetOrdinal("language")),
                                MoviePoster = reader.GetString(reader.GetOrdinal("moviePoster")),
                                Actor = reader.GetString(reader.GetOrdinal("actor")),
                                Actress = reader.GetString(reader.GetOrdinal("actress")),
                                Director = reader.GetString(reader.GetOrdinal("director"))
                            };
                        }
                    }
                }
            }

            return movie;
        }
        /// <summary>
        /// Used to update a movie
        /// </summary>
        /// <param name="movie"></param>
        public void UpdateMovie(Movie movie)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPU_Movie", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@movieId", movie.MovieId);
                    command.Parameters.AddWithValue("@movieName", movie.MovieName);
                    command.Parameters.AddWithValue("@description", movie.Description);
                    command.Parameters.AddWithValue("@duration", movie.Duration);
                    command.Parameters.AddWithValue("@genre", movie.Genre);
                    command.Parameters.AddWithValue("@releaseDate", movie.ReleaseDate);
                    command.Parameters.AddWithValue("@language", movie.Language);
                    command.Parameters.AddWithValue("@moviePoster", (object)movie.MoviePoster ?? DBNull.Value);
                    command.Parameters.AddWithValue("@actor", movie.Actor);
                    command.Parameters.AddWithValue("@actress", movie.Actress);
                    command.Parameters.AddWithValue("@director", movie.Director);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Used to delete a movie
        /// </summary>
        /// <param name="movieId"></param>
        public void DeleteMovie(int movieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPD_Movie", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@movieId", movieId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Used to add a new showtime
        /// </summary>
        /// <param name="showtime"></param>
        public void InsertShowtime(ShowTime showtime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPI_ShowTime", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@movieId", showtime.MovieId);
                    command.Parameters.AddWithValue("@showDate", showtime.ShowDate);
                    command.Parameters.AddWithValue("@startTime", showtime.StartTime);
                    command.Parameters.AddWithValue("@screenNumber", showtime.ScreenNumber);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Used to get all added showtimes
        /// </summary>
        /// <returns></returns>
        public List<ShowTime> GetAllShowtimes()
        {
            var showtimes = new List<ShowTime>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SPS_AllShowTimes", connection);
                command.CommandType = CommandType.StoredProcedure; // Indicating that this is a stored procedure
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        showtimes.Add(new ShowTime
                        {
                            ShowtimeId = (int)reader["ShowtimeId"],
                            ShowDate = (DateTime)reader["ShowDate"],
                            StartTime = (TimeSpan)reader["StartTime"],
                            MovieName = reader["MovieName"].ToString()
                        });
                    }
                }
            }

            return showtimes;
        }


        /// <summary>
        /// Used to get a showtime by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShowTime GetShowtimeById(int id)
        {
            ShowTime showtime = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SPS_GetShowTimesById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@showtimeId", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        showtime = new ShowTime
                        {
                            ShowtimeId = (int)reader["ShowtimeId"],
                            MovieId = (int)reader["MovieId"],
                            ShowDate = (DateTime)reader["ShowDate"],
                            StartTime = (TimeSpan)reader["StartTime"],
                            MovieName = reader["MovieName"].ToString()
                        };
                    }
                }
            }

            return showtime;
        }
        /// <summary>
        /// Used to update a showtime
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateShowtime(ShowTime model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SPU_Showtime", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@showtimeId", model.ShowtimeId);
                command.Parameters.AddWithValue("@movieId", model.MovieId);
                command.Parameters.AddWithValue("@showDate", model.ShowDate);
                command.Parameters.AddWithValue("@startTime", model.StartTime);

                connection.Open();
                var rowsAffected = command.ExecuteNonQuery();

                //To confirm the update was successful
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Used to delete a showtime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteShowtime(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SPD_ShowTime", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@showtimeId", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected > 0;
                }
            }
        }

        /// <summary>
        /// Used to add seat
        /// </summary>
        /// <param name="model"></param>
        public void AddSeat(Seat model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPI_AddSeat", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@rowNumber", model.RowNumber);
                    command.Parameters.AddWithValue("@columnNumber", model.ColumnNumber);
                    command.Parameters.AddWithValue("@seatTypeId", model.SeatTypeId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Used to get seat types for creation of seats
        /// </summary>
        /// <returns></returns>
        public List<SeatType> GetSeatTypes()
        {
            var seatTypes = new List<SeatType>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPS_SeatTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            seatTypes.Add(new SeatType
                            {
                                SeatTypeId = reader.GetInt32(reader.GetOrdinal("seatTypeId")),
                                SeatTypeName = reader.GetString(reader.GetOrdinal("seatType")),
                                Price = reader.GetDecimal(reader.GetOrdinal("price"))
                            });
                        }
                    }
                }
            }

            return seatTypes;
        }


        /// <summary>
        /// Used to get seats to view the layouts for seats
        /// </summary>
        /// <returns></returns>
        public List<Seat> GetSeats()
        {
            var seats = new List<Seat>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPS_GetSeats", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            seats.Add(new Seat
                            {
                                SeatId = reader.GetInt32(reader.GetOrdinal("seatId")),
                                RowNumber = reader.GetString(reader.GetOrdinal("rowNumber")),
                                ColumnNumber = reader.GetInt32(reader.GetOrdinal("columnNumber")),
                                SeatTypeId = reader.GetInt32(reader.GetOrdinal("seatTypeId")),

                            });
                        }
                    }
                }
            }

            return seats;
        }


        /// <summary>
        /// For getting seat info by id for editing purpose
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The details of a seat</returns>
        public Seat GetSeatById(int id)
        {
            Seat seat = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPS_GetSeatById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SeatId", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            seat = new Seat
                            {
                                SeatId = reader.GetInt32(reader.GetOrdinal("SeatId")),
                                RowNumber = reader.GetString(reader.GetOrdinal("RowNumber")),
                                ColumnNumber = reader.GetInt32(reader.GetOrdinal("ColumnNumber")),
                                SeatTypeId = reader.GetInt32(reader.GetOrdinal("SeatTypeId")),

                            };
                        }
                    }
                }
            }

            return seat;
        }

        /// <summary>
        /// Used for updating the seats
        /// </summary>
        /// <param name="updatedSeat"></param>
        public void UpdateSeat(Seat updatedSeat)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SPU_Seat", connection)) 
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SeatId", updatedSeat.SeatId);
                    command.Parameters.AddWithValue("@RowNumber", updatedSeat.RowNumber);
                    command.Parameters.AddWithValue("@ColumnNumber", updatedSeat.ColumnNumber);
                    command.Parameters.AddWithValue("@SeatTypeId", updatedSeat.SeatTypeId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Used for deleteing the seats
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Tp view seats page if successfull </returns>
        public bool DeleteSeat(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("SPD_Seat", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@seatId", id));

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Used to check whether the seat exists during creation and editing of seats
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public bool CheckSeatExists(string rowNumber, int columnNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SPS_CheckSeatExists", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@RowNumber", rowNumber);
                command.Parameters.AddWithValue("@ColumnNumber", columnNumber);

                connection.Open();
                var result = command.ExecuteScalar();
                return Convert.ToInt32(result) == 1;
            }
        }



    }
}
