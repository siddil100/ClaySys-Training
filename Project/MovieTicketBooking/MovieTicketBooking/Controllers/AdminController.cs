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
        [HttpGet]
        public ActionResult AdminHome()
        {
            return View();
        }
        /// <summary>
        /// Used to load add movie form
        /// </summary>
        /// <returns>A form to add new movies</returns>
        [HttpGet]
        public ActionResult AddMovie()
        {
            return View();
        }

        /// <summary>
        /// Used to add a new movie to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To succes page if movie is added</returns>
        [HttpPost]
        public ActionResult AddMovie(Movie model)
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

            return View(model);
        }
        /// <summary>
        /// To load a page with list of added movies
        /// </summary>
        /// <returns>A list of movies</returns>
        [HttpGet]
        public ActionResult ViewAllMovies()
        {
            
            List<Movie> movies = _adminRepository.GetAllMovies();
            return View(movies);
        }


        /// <summary>
        /// To load edit movie form
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To view movies page if edit is successfull</returns>
        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            var movie = _adminRepository.GetMovieById(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// Used to edit a movie with changes
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To view movies</returns>
        [HttpPost]
        public ActionResult EditMovie(Movie model)
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
                // Handle the error (log it, show an error message, etc.)
                ViewBag.ErrorMessage = "An error occurred while trying to delete the movie.";
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
            ViewBag.Movies = new SelectList(_adminRepository.GetAllMovies(), "MovieId", "MovieName"); // Dropdown for selecting movie.
            return View();
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
            if (ModelState.IsValid)
            {
                model.ScreenNumber = 1; // Set screen number to 1 as it's a single screen theater
                _adminRepository.InsertShowtime(model);
                return RedirectToAction("ViewShowtimes");
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
            var showtimes = _adminRepository.GetAllShowtimes();
            return View(showtimes);
        }

        /// <summary>
        /// Used to load editshowtime page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To view showtimes if successfull</returns>
        public ActionResult EditShowtime(int id)
        {
            var showtime = _adminRepository.GetShowtimeById(id);
            ViewBag.Movies = new SelectList(_adminRepository.GetAllMovies(), "MovieId", "MovieName", showtime.MovieId);
            return View(showtime);
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
            if (ModelState.IsValid)
            {
                _adminRepository.UpdateShowtime(model);
                return RedirectToAction("ViewShowtimes");
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
            bool isDeleted = _adminRepository.DeleteShowtime(id);

            if (isDeleted)
            {
                return RedirectToAction("ViewShowtimes"); 
            }

            ModelState.AddModelError("", "Failed to delete showtime. Please try again.");
            return RedirectToAction("ViewShowtimes"); 
        }


        /// <summary>
        /// Used to load create seat form
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSeat()
        {
            var model = new SeatViewModel
            {
                SeatTypes = _adminRepository.GetSeatTypes() // Fetch seat types
            };
            return View(model);
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
            if (ModelState.IsValid)
            {
                _adminRepository.AddSeat(model.Seat);
                return RedirectToAction("ViewSeats");
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
            var seats = _adminRepository.GetSeats();
            return View(seats);
        }

        /// <summary>
        /// Used to edit the seats
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewSeats()
        {
            var seats = _adminRepository.GetSeats();
            return View(seats);
        }



       /// <summary>
       /// Used to load editseat form
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult EditSeat(int id)
        {
            var seat = _adminRepository.GetSeatById(id); 
            if (seat == null)
            {
                return HttpNotFound();
            }
            return View(seat); 
        }

       /// <summary>
       /// Used to make changes to existing seats by editing
       /// </summary>
       /// <param name="updatedSeat"></param>
       /// <returns>To view seats if successfull</returns>
        [HttpPost]
        public ActionResult EditSeat(Seat updatedSeat)
        {
            if (ModelState.IsValid)
            {
                _adminRepository.UpdateSeat(updatedSeat); 
                return RedirectToAction("ViewSeats"); 
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
    
            bool isDeleted = _adminRepository.DeleteSeat(id);

            if (isDeleted)
            {
                return RedirectToAction("ViewSeats"); 
            }

            ModelState.AddModelError("", "Failed to delete seat. Please try again.");
            return RedirectToAction("ViewSeats"); 
        }


        /// <summary>
        /// Used to check whether seat exists
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="columnNumber"></param>
        /// <returns>True or false</returns>
        public JsonResult CheckSeatExists(string rowNumber, int columnNumber)
        {
            bool seatExists = _adminRepository.CheckSeatExists(rowNumber, columnNumber);
            return Json(new { exists = seatExists }, JsonRequestBehavior.AllowGet);
        }


    }

}
