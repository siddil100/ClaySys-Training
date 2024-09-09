﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [StringLength(10)]
        public string Role { get; set; } = "user";
    }
}