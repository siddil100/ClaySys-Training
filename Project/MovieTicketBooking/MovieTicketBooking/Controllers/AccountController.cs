using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.ViewModels;

namespace MovieTicketBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;
        private readonly AccountRepository _accountRepository;

        public AccountController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _accountRepository = new AccountRepository();

        }
        /// <summary>
        /// To load the sign up page for user
        /// </summary>
        /// <returns>A sign up form </returns>
        [HttpGet]
        public ActionResult Signup()
        {
            try
            {
                var states = _accountRepository.GetStates();
                ViewBag.States = states;
            }
            catch (Exception ex)
            {
                // Log exception and return error view
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "An error occurred while loading the states.";
                return View("Error");
            }

            return View();
        }

        
        /// <summary>
        /// To get cities based on state on sign up form
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns>The list of cities</returns>
        [HttpGet]
        public JsonResult GetCities(int stateId)
        {
            try
            {
                var cities = _accountRepository.GetCities(stateId);
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Json(new { success = false, message = "Error loading cities." }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// To check whether email is already taken
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true or false </returns>
        [HttpGet]
        public JsonResult CheckEmailExists(string email)
        {
            try
            {
                bool emailExists = _accountRepository.CheckEmailExists(email);
                return Json(!emailExists, JsonRequestBehavior.AllowGet); // Returns true if email is available
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Json(new { success = false, message = "Error checking email." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To register a user
        /// </summary>
        /// <param name="model"></param>
        /// <Returns>returns to login if the registartion is complete</returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _accountRepository.RegisterUser(model.User, model.UserDetails);
                    TempData["SuccessMessage"] = "Account created successfully. Please log in.";
                    return RedirectToAction("Login", "Account");
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    ViewBag.ErrorMessage = "An error occurred during registration.";
                    return View("Signup", model);
                }
            }

            ViewBag.States = _accountRepository.GetStates();
            return View("Signup", model);
        }
        /// <summary>
        /// Used to load the login form
        /// </summary>
        /// <returns>A login page</returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var repository = new AccountRepository();
                    var user = repository.AuthenticateUser(model.Email, model.Password);

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, false);
                        Session["UserId"] = user.UserId;
                        Session["Email"] = user.Email;

                        if (user.Role == "admin")
                        {
                            return RedirectToAction("AdminHome", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("UserHome", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect credentials, please try again.");
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    ViewBag.ErrorMessage = "An error occurred during login.";
                }
            }

            return View(model);
        }
        /// <summary>
        /// Used for logging out user
        /// </summary>
        /// <returns>Returns to login view after logging out</returns>
        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
