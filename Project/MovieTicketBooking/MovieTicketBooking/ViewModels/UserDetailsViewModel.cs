using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.ViewModels
{
    public class UserDetailsViewModel
    {
        public int UserId { get; set; } // UserId for the update
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public DateTime? DateOfBirth { get; set; } 
        public string Gender { get; set; } 
        public string PhoneNumber { get; set; }
    }
}