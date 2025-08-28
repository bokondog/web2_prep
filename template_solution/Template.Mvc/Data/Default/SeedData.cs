using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Template.Mvc.Models;

namespace Template.Mvc.Data.DefaultData
{
    public class SeedData
    {
        private static UserManager<IdentityUser> _userManager;
        private static TemplateDbContext _context;
        public static async Task EnsurePopulatedAsync(WebApplication? app)
        {

            var scope = app.Services.CreateScope();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _context = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();
            if (!_userManager.Users.Any())
            {
                await CreateIdentityUserAsync("Jim", "Herbots", "jim.herbots@pxl.be");
                await CreateIdentityUserAsync("Chris", "Pannaers", "chris.pannaers@pxl.be");
            }
            //if (!_context.PadelSquares.Any())
            //{
            //    CreateSquare("Indoor square 1", 100);
            //    CreateSquare("Outdoor square 1", 50);
            //}
            //if (!_context.Reservations.Any())
            //{
            //    CreateReservation(1, 1, new DateTime(2025, 06, 11, 9, 0, 0));
            //    CreateReservation(2, 2, new DateTime(2025, 06, 11, 9, 0, 0));
            //    CreateReservation(2, 1, new DateTime(2025, 06, 12, 9, 0, 0));
            //    CreateReservation(1, 2, new DateTime(2025, 06, 12, 9, 0, 0));
            //    CreateReservation(1, 2, new DateTime(2025, 06, 13, 9, 0, 0));
            //}
        }
        //private static void CreateReservation(int squareId, int userId, DateTime date)
        //{
        //    try
        //    {
        //        var reservation = new Reservation();
        //        reservation.PadelSquareId = squareId;
        //        reservation.PadelUserId = userId;
        //        reservation.ReservationDate = date;
        //        _context.Reservations.Add(reservation);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        //private static void CreateSquare(string name, int price)
        //{
        //    try
        //    {
        //        var square = new PadelSquare();
        //        square.SquareName = name;
        //        square.RentalPrice = price;
        //        _context.PadelSquares.Add(square);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        private static async Task CreateIdentityUserAsync(string firstName, string lastName, string email)
        {
            try
            {
                var user = new IdentityUser();
                user.Email = email;
                user.UserName = $"{firstName}_{lastName}";
                var pwd = $"{user.UserName}!123";
                var result = await _userManager.CreateAsync(user, pwd);
                if (result.Succeeded)
                {
                    var templateUser = new TemplateUser();
                    templateUser.FirstName = firstName;
                    templateUser.LastName = lastName;
                    templateUser.IdentityUserId = user.Id;
                    _context.TemplateUsers.Add(templateUser);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}