using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.ViewModels
{
    public class BookingViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MovieName { get; set; }
        public string SeatNames { get; set; }
        public decimal TotalAmount { get; set; }
    }
}