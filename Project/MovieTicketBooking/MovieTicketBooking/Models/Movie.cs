using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        [DisplayName("Movie name")]
        public string MovieName { get; set; }
        public string Description { get; set; }
        [DisplayName("Duration in minutes")]
        public int Duration { get; set; }
        public string Genre { get; set; }
        [DisplayName("Release date")]
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        [DisplayName("Movie poster")]
        public string MoviePoster { get; set; } // base64 string
        public string Actor { get; set; }
        public string Actress { get; set; }
        public string Director { get; set; }
    }
}