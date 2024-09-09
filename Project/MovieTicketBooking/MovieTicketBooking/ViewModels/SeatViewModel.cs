using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.ViewModels
{
    public class SeatViewModel
    {
        public Seat Seat { get; set; }
        public IEnumerable<SeatType> SeatTypes { get; set; }
    }
}