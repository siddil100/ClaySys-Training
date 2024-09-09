using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MovieTicketBooking.Models
{
    public class UserDetails
    {
        [Key]
        public int DetailId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

       
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        
        public string Gender { get; set; }

        [StringLength(15)]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        
        public int StateId { get; set; }
        public State State { get; set; }

      
        public int CityId { get; set; }
        public City City { get; set; }
    }
}