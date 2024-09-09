using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Models
{
    public class ShowTime
    {
        public int ShowtimeId { get; set; }
        public int MovieId { get; set; }
        [DisplayName("Show date")]
        public DateTime ShowDate { get; set; }
        [DisplayName("Start time")]
        public TimeSpan StartTime { get; set; }
        public int ScreenNumber { get; set; }
        public string MovieName { get; set; }  //for displaying showtimes with movies
    }
}