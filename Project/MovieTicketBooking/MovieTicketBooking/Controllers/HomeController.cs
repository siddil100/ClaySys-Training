using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MovieTicketBooking.Models; 
using System.Configuration; 
using MovieTicketBooking.Repositories;
namespace MovieTicketBooking.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private readonly HomeRepository _homeRepository;

        public HomeController()
        {
            _homeRepository = new HomeRepository();
        }
        /// <summary>
        /// Used to load the index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Used to load aboutus page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "About us.";
            return View();
        }

        /// <summary>
        /// Used to load contact us page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContactUs()
        {
            
            return View();
        }

        /// <summary>
        /// Used to submit enquiry form
        /// </summary>
        /// <param name="contactUs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitEnquiry(ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _homeRepository.SubmitEnquiry(contactUs);

                    
                    return RedirectToAction("FeedbackSuccess");
                }
                catch (Exception ex)
                {
                    LoggingHelper.LogError("An error occurred.", ex);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    ModelState.AddModelError("", "An error occurred while submitting your enquiry.");
                }
            }
            return View("ContactUs");
        }

       /// <summary>
       /// Used to load success page after enquiry form is submitted
       /// </summary>
       /// <returns></returns>
        public ActionResult FeedbackSuccess()
        {
            ViewBag.Title = "Feedback Success";
            return View();
        }
    }
}
