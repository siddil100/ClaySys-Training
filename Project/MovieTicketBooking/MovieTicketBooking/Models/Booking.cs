using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Models
{
    public class Booking
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public int ShowtimeId { get; set; }
        public decimal TotalAmount { get; set; }
        public string SeatIds { get; set; }
        public DateTime BookingDate { get; set; }
        public string SeatDescriptions { get; set; }
    }
}
