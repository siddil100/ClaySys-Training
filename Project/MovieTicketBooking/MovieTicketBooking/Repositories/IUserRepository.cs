using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace MovieTicketBooking.Repositories
{
    public interface IUserRepository
    {
        
        //Used for interface implementation for UserRepository
        void UpdateUserDetails(UserDetails userDetails);
        UserDetails GetUserDetailsById(int userId);
        List<SelectListItem> GetCities_Ajax(int stateId);
        List<SelectListItem> GetCities(int stateId);
        bool UpdatePassword(int userId, string oldPassword, string newPassword);
        List<SelectListItem> GetStates();
        Task<List<Movie>> GetMoviesAsync();
        List<ShowTime> GetShowtimesByMovieId(int movieId);
        Movie GetMovieDetailsById(int movieId);
        List<Seat> GetAllSeatsByShowtimeId(int showtimeId);

        List<SeatType> GetSeatPrices();  //for fetching prices
        void InsertBooking(int userId, int showtimeId, decimal totalAmount, string seatIds);
        Booking GetLatestBookingForUser(int userId);
       Task<List<Movie>> GetFilteredMoviesAsync(string language, string genre);

        IEnumerable<UserBookings> GetUserBookings(int userId);



    }

}