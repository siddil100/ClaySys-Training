using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MovieTicketBooking.Models;

namespace MovieTicketBooking.ViewModels
{
    public class RegisterViewModel
    {
        public User User { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}
