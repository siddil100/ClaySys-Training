using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Models
{
    public class UserBookings
    {
        public int BookingId { get; set; }
        public string MovieName { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public string SeatNames{ get; set; }
        public decimal TotalAmount { get; set; }
    }
}