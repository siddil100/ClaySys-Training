using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MovieTicketBooking.Models;
using MovieTicketBooking.ViewModels;
using MovieTicketBooking.Repositories;
using System.Diagnostics;
using System.Linq;

namespace MovieTicketBooking.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminRepository _adminRepository;

        public AdminController()
        {
            // Retrieve the connection string from web.config
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Initialize the repository with the connection string
            _adminRepository = new AdminRepository(connectionString);
        }
        /// <summary>
        /// Used to load admin home page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult AdminHome()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while accessing AdminHome.", ex);

                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to load add movie form
        /// </summary>
        /// <returns>A form to add new movies</returns>
        [HttpGet]
        public ActionResult AddMovie()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                LoggingHelper.LogError("Error occured loading add movies page", ex);
                return View("Error");
            }

        }

        /// <summary>
        /// Used to add a new movie to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To succes page if movie is added</returns>
        [HttpPost]
        public ActionResult AddMovie(Movie model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Converting image to base64 string
                    string moviePosterBase64 = null;
                    if (Request.Files["MoviePoster"] != null && Request.Files["MoviePoster"].ContentLength > 0)
                    {
                        var moviePosterFile = Request.Files["MoviePoster"];
                        using (var binaryReader = new BinaryReader(moviePosterFile.InputStream))
                        {
                            byte[] imageBytes = binaryReader.ReadBytes(moviePosterFile.ContentLength);
                            moviePosterBase64 = Convert.ToBase64String(imageBytes);
                        }
                    }

                    // Create a movie object 
                    var movieToAdd = new Movie
                    {
                        MovieName = model.MovieName,
                        Description = model.Description,
                        Duration = model.Duration,
                        Genre = model.Genre,
                        ReleaseDate = model.ReleaseDate,
                        Language = model.Language,
                        MoviePoster = moviePosterBase64,
                        Actor = model.Actor,
                        Actress = model.Actress,
                        Director = model.Director
                    };

                    _adminRepository.AddMovie(movieToAdd);

                    return RedirectToAction("ViewAllMovies");
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while adding the movie.", ex);
                ModelState.AddModelError("", "An error occurred while adding the movie. Please try again.");
            }

            return View(model);
        }

        /// <summary>
        /// To load a page with list of added movies
        /// </summary>
        /// <returns>A list of movies</returns>
        [HttpGet]
        public ActionResult ViewAllMovies()
        {
            try
            {
                List<Movie> movies = _adminRepository.GetAllMovies();
                return View(movies);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the list of movies.", ex);
                return View("Error");
            }
        }

        /// <summary>
        /// To load edit movie form
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To view movies page if edit is successfull</returns>
        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            try
            {
                var movie = _adminRepository.GetMovieById(id);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                return View(movie);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the movie details.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to edit a movie with changes
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To view movies</returns>
        [HttpPost]
        public ActionResult EditMovie(Movie model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMovie = _adminRepository.GetMovieById(model.MovieId);

                    // Use the existing poster if no new file is uploaded
                    string moviePosterBase64 = existingMovie.MoviePoster;

                    if (Request.Files["MoviePoster"] != null && Request.Files["MoviePoster"].ContentLength > 0)
                    {
                        var moviePosterFile = Request.Files["MoviePoster"];
                        using (var binaryReader = new BinaryReader(moviePosterFile.InputStream))
                        {
                            byte[] imageBytes = binaryReader.ReadBytes(moviePosterFile.ContentLength);
                            moviePosterBase64 = Convert.ToBase64String(imageBytes);
                        }
                    }

                    model.MoviePoster = moviePosterBase64;

                    _adminRepository.UpdateMovie(model);

                    return RedirectToAction("ViewAllMovies");
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while editing the movie.", ex);
            }

            return View(model);
        }

        /// <summary>
        /// Used to delete a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To view movies page if deletion is successfull</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMovie(int id)
        {
            try
            {
                _adminRepository.DeleteMovie(id);
                return RedirectToAction("ViewAllMovies"); // Or redirect to the appropriate view
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while deleting the movie.", ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Used to load create showtime form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateShowtime()
        {
            try
            {
                ViewBag.Movies = new SelectList(_adminRepository.GetAllMovies(), "MovieId", "MovieName");
                return View();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while creating showtime.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to insert a new showtime
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To view showtimes if successfull</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateShowtime(ShowTime model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ScreenNumber = 1; // Set screen number to 1 as it's a single screen theater
                    _adminRepository.InsertShowtime(model);
                    return RedirectToAction("ViewShowtimes");
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while creating the showtime.", ex);
            }

            ViewBag.Movies = new SelectList(_adminRepository.GetAllMovies(), "MovieId", "MovieName");
            return View(model);
        }

        /// <summary>
        /// Used to load view showtimes page
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewShowtimes()
        {
            try
            {
                var showtimes = _adminRepository.GetAllShowtimes();
                return View(showtimes);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the list of showtimes.", ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Used to load editshowtime page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To view showtimes if successfull</returns>
        public ActionResult EditShowtime(int id)
        {
            try
            {
                var showtime = _adminRepository.GetShowtimeById(id);
                ViewBag.Movies = new SelectList(_adminRepository.GetAllMovies(), "MovieId", "MovieName", showtime.MovieId);
                return View(showtime);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while editing the showtime.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to edit the show time model with new changes
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To view showtimes page if edit is sucessfull</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShowtime(ShowTime model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _adminRepository.UpdateShowtime(model);
                    return RedirectToAction("ViewShowtimes");
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while updating the showtime.", ex);
                return View("Error"); 
            }

            ViewBag.Movies = new SelectList(_adminRepository.GetAllMovies(), "MovieId", "MovieName", model.MovieId);
            return View(model);
        }

        /// <summary>
        /// Used to delete a showtime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteShowtime(int id)
        {
            try
            {
                bool isDeleted = _adminRepository.DeleteShowtime(id);

                if (isDeleted)
                {
                    return RedirectToAction("ViewShowtimes");
                }

                ModelState.AddModelError("", "Failed to delete showtime. Please try again.");
                return RedirectToAction("ViewShowtimes");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while deleting the showtime.", ex);
                ModelState.AddModelError("", "An error occurred while deleting the showtime. Please try again later.");
                return RedirectToAction("ViewShowtimes");
            }
        }

        /// <summary>
        /// Used to load create seat form
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSeat()
        {
            try
            {
                var model = new SeatViewModel
                {
                    SeatTypes = _adminRepository.GetSeatTypes() // Fetch seat types
                };
                return View(model);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while creating the seat.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to create a seat
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To viewseats if successfull</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSeat(SeatViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _adminRepository.AddSeat(model.Seat);
                    return RedirectToAction("ViewSeats");
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while creating the seat.", ex);
                return View("Error"); 
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Debug.WriteLine(error.ErrorMessage);
            }

            // Re-fetch seat types if model is invalid
            model.SeatTypes = _adminRepository.GetSeatTypes();
            return View(model);
        }

        /// <summary>
        /// Used for admin to see the layout of the seats in theatre
        /// </summary>
        /// <returns>The total Seats in theatre</returns>
        public ActionResult ViewLayout()
        {
            try
            {
                var seats = _adminRepository.GetSeats();
                return View(seats);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the layout.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to edit the seats
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewSeats()
        {
            try
            {
                var seats = _adminRepository.GetSeats();
                return View(seats);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the seats.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to load editseat form
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditSeat(int id)
        {
            try
            {
                var seat = _adminRepository.GetSeatById(id);
                if (seat == null)
                {
                    return HttpNotFound();
                }
                return View(seat);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the seat for editing.", ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Used to make changes to existing seats by editing
        /// </summary>
        /// <param name="updatedSeat"></param>
        /// <returns>To view seats if successfull</returns>
        [HttpPost]
        public ActionResult EditSeat(Seat updatedSeat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _adminRepository.UpdateSeat(updatedSeat);
                    return RedirectToAction("ViewSeats");
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while updating the seat.", ex);
                return View("Error"); 
            }

            return View(updatedSeat);
        }

        /// <summary>
        /// Used to delete a seat
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSeat(int id)
        {
            try
            {
                bool isDeleted = _adminRepository.DeleteSeat(id);

                if (isDeleted)
                {
                    return RedirectToAction("ViewSeats");
                }

                ModelState.AddModelError("", "Failed to delete seat. Please try again.");
                return RedirectToAction("ViewSeats");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while deleting the seat.", ex);
                ModelState.AddModelError("", "An error occurred while deleting the seat. Please try again later.");
                return RedirectToAction("ViewSeats");
            }
        }

        /// <summary>
        /// Used to check whether seat exists
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="columnNumber"></param>
        /// <returns>True or false</returns>
        public JsonResult CheckSeatExists(string rowNumber, int columnNumber)
        {
            try
            {
                bool seatExists = _adminRepository.CheckSeatExists(rowNumber, columnNumber);
                return Json(new { exists = seatExists }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while checking if the seat exists.", ex);
                return Json(new { exists = false }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Used to list the users for managing
        /// </summary>
        /// <returns></returns>
        public ActionResult ListUsers()
        {
            try
            {
                var users = _adminRepository.GetUsers();
                return View(users);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the list of users.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used for soft deletion of users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ToggleUserStatus(int id)
        {
            try
            {
                bool isToggled = _adminRepository.ToggleUserStatus(id);

                if (!isToggled)
                {
                    ModelState.AddModelError("", "Failed to toggle user status. Please try again.");
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while toggling the user status.", ex);
            }

            return RedirectToAction("ListUsers");
        }

        /// <summary>
        /// Used to view details of a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewUserDetails(int id)
        {
            try
            {
                UserDetailsViewModel user = _adminRepository.GetUserDetails(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving user details.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to retrive bookings from admin
        /// </summary>
        /// <param name="showtimeId"></param>
        /// <returns></returns>
        public ActionResult ViewBookings(int showtimeId)
        {
            try
            {
                var bookings = _adminRepository.GetBookingsByShowtime(showtimeId);
                return View(bookings);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving bookings.", ex);
                return View("Error"); 
            }
        }

        /// <summary>
        /// Used to check whether the same showtime already exists for the same date
        /// </summary>
        /// <param name="showDate"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckShowtimeExists(DateTime showDate, TimeSpan startTime)
        {
            try
            {
                var showtimeExists = _adminRepository.CheckShowtimeExists(showDate, startTime);
                return Json(new { isAvailable = !showtimeExists }, JsonRequestBehavior.AllowGet); // Return false if the showtime exists, meaning it's not available
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while checking if the showtime exists.", ex);
                return new JsonResult(); 
            }
        }
    }

}
