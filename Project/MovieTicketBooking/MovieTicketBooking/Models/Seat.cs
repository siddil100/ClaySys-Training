using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Models
{
    public class Seat
    {
        public int SeatId { get; set; }

        [Required]
        [StringLength(1, ErrorMessage = "Row number must be a single character.")]
        [DisplayName("Row number")]
        public string RowNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Column number must be a positive integer.")]
        [DisplayName("Column number")]
        public int ColumnNumber { get; set; }

        public int SeatTypeId { get; set; }

      
        public int ScreenNumber { get; set; }


    }
}