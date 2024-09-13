using MovieTicketBooking.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieTicketBooking.ViewModels;
using System.Data.SqlClient;
using System.Data;
using MovieTicketBooking.Models;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MovieTicketBooking.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            _userRepository = new UserRepository(connectionString);
        }

        /// <summary>
        /// Used to load the home page for user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        /*[Authorize]*/
        public async Task<ActionResult> UserHome()
        {
            try
            {

                LoggingHelper.LogInfo("My UserHome started execution");

                var movies = await _userRepository.GetMoviesAsync();
                return View(movies);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred in UserHome.", ex);
                Debug.WriteLine($"Error: {ex.Message}");
                return View("Error");
            }
        }

        /// <summary>
        /// Get cities using ajax when editing profile info
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCities_Ajax(int stateId)
        {
            try
            {
                var cities = _userRepository.GetCities_Ajax(stateId);
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json(new { success = false, message = "An error occurred while fetching cities." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Used to load edit form
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                var userDetails = _userRepository.GetUserDetailsById(id);

                if (userDetails == null)
                {
                    return HttpNotFound();
                }

                var model = new UserDetailsViewModel
                {
                    UserId = userDetails.UserId,
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    Address = userDetails.Address,
                    StateId = userDetails.StateId,
                    CityId = userDetails.CityId,
                    DateOfBirth = userDetails.Dob,
                    Gender = userDetails.Gender,
                    PhoneNumber = userDetails.PhoneNumber
                };

                ViewBag.States = _userRepository.GetStates();
                ViewBag.Cities = _userRepository.GetCities(model.StateId);

                return View(model);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred.", ex);
                Debug.WriteLine(ex.Message);
                return View("Error");
            }
        }

        /// <summary>
        /// Used to edit the profile info of user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Edit(UserDetailsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepository.UpdateUserDetails(new UserDetails
                    {
                        UserId = model.UserId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        StateId = model.StateId,
                        CityId = model.CityId,
                        Dob = model.DateOfBirth,
                        Gender = model.Gender,
                        PhoneNumber = model.PhoneNumber
                    });

                    return RedirectToAction("UserHome", "User");
                }

                ViewBag.States = _userRepository.GetStates();
                ViewBag.Cities = _userRepository.GetCities(model.StateId);

                return View(model);
            }
            catch (Exception ex)
            {
                
                LoggingHelper.LogError("An error occurred in Edit action.", ex);

                return View("Error");
            }
        }



        /// <summary>
        /// Used to load password change form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdatePassword()
        {
            return View();
        }

        /// <summary>
        /// Used to change the user's password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
       public ActionResult UpdatePassword(UpdatePasswordViewModel model)
{
    if (ModelState.IsValid)
    {
        try
        {
            int userId = GetLoggedInUserId();

            bool isUpdated = _userRepository.UpdatePassword(userId, model.OldPassword, model.NewPassword);

            if (isUpdated)
            {
                ViewBag.Message = "Password updated successfully.";
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to update the password. Please try again.";
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            
            ViewBag.ErrorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            LoggingHelper.LogError("An error occured.", ex);
          
            ViewBag.ErrorMessage = "an error occure try again later.";
        }
    }

    return View(model);
}

        /// <summary>
        /// Used to get userid from session
        /// </summary>
        /// <returns></returns>
        private int GetLoggedInUserId()
        {   
            return Convert.ToInt32(Session["UserId"]);
        }

        /// <summary>
        /// Used to view showtimes for a specific movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public ActionResult SeeShowtimes(int movieId)
        {
            try
            {
                Debug.WriteLine($"Received movieId: {movieId}");
                var showtimes = _userRepository.GetShowtimesByMovieId(movieId);
                return View(showtimes);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"An error occurred while fetching showtimes for movieId: {movieId}.", ex);
                return View("Error");
            }
        }

        /// <summary>
        /// For viewing movies information in detail
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>Details of the movie</returns>
        public ActionResult ViewMovieDetailsUser(int movieId)
        {
            try
            {
                var movie = _userRepository.GetMovieDetailsById(movieId);
                if (movie == null)
                {
                    LoggingHelper.LogWarning($"Movie with ID {movieId} not found.");
                    return HttpNotFound();
                }
                return View(movie);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"An error occurred while fetching movie details for movieId: {movieId}.", ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Allows users to select seats for a specific showtime
        /// </summary>
        /// <param name="showtimeId"></param>
        /// <returns></returns>
        public ActionResult SelectSeats(int showtimeId)
        {
            try
            {
                Session["ShowtimeId"] = showtimeId;
                var seats = _userRepository.GetAllSeatsByShowtimeId(showtimeId);
                return View(seats);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"An error occurred while selecting seats for showtimeId: {showtimeId}.", ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Used to fetch prices for seats
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSeatPrices()
        {
            var seatPrices = _userRepository.GetSeatPrices(); 
            return Json(seatPrices, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Used to insert booking details
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmBooking(FormCollection form)
        {
            try
            {
                var userId = GetLoggedInUserId();
                var showtimeId = int.Parse(form["showtimeId"]);
                var totalAmount = decimal.Parse(form["totalAmount"].Replace("₹", ""));
                var seatIds = string.Join(",", form.GetValues("selectedSeats")); // Comma-separated seat IDs

                _userRepository.InsertBooking(userId, showtimeId, totalAmount, seatIds);
                return RedirectToAction("BookingConfirmation");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while confirming the booking.", ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Used to show booking confirmation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BookingConfirmation()
        {
            try
            {
                var userId = GetLoggedInUserId();
                var bookingDetails = _userRepository.GetLatestBookingForUser(userId);

                if (bookingDetails == null)
                {
                    ViewBag.ErrorMessage = "No recent bookings found.";
                    return View("Error");
                }

                return View(bookingDetails);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving the booking confirmation.", ex);
                return View("Error");
            }
        }
        /// <summary>
        /// Used to search specific movies
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<ActionResult> SearchMovies(string searchTerm)
        {
            try
            {
                var movies = await _userRepository.GetMoviesAsync();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    // Filter the movie list based on the search term (case-insensitive)
                    movies = movies.Where(m => m.MovieName.ToLower().Contains(searchTerm.ToLower())).ToList();
                }

                // Return the filtered list as a partial view
                return PartialView("_MovieListPartial", movies);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"An error occurred while searching for movies with the term: {searchTerm}.", ex);
                return PartialView("Error");
            }
        }

        /// <summary>
        /// Used to filter movies with language and genre
        /// </summary>
        /// <param name="language"></param>
        /// <param name="genre"></param>
        /// <returns>list of filtered movies</returns>
        public async Task<ActionResult> FilterMovies(string language, string genre)
        {
            try
            {
                var movies = await _userRepository.GetFilteredMoviesAsync(language, genre);
                return PartialView("_MovieListPartial", movies);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"An error occurred while filtering movies with language: {language} and genre: {genre}.", ex);
                return PartialView("Error");
            }
        }

        /// <summary>
        /// Used to list my bookings
        /// </summary>
        /// <returns>list of filtered movies</returns>
        public ActionResult MyBookings()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var bookings = _userRepository.GetUserBookings(userId);

                return View(bookings);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("An error occurred while retrieving user bookings.", ex);
                return View("Error");
            }
        }

    }
}