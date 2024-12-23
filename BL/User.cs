using System.Collections.Generic;
using Matala2_ASP.DAL;
using System;
using System.Linq;
using System.Web;

namespace Matala2_ASP.BL
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Integration with UserDAL
        private static readonly UserDal userDal = new UserDal();

        // Insert user into the database
        public int Insert()
        {
            return userDal.AddUser(this) ? 1 : 0;
        }

        // Update user in the database
        public bool Update()
        {
            return userDal.UpdateUser(this);
        }

        // Get all users
        public static List<User> GetAllUsers()
        {
            UserDal userDal = new UserDal();
            return userDal.GetAllUsers();
        }

        // Login user
        public static User? Login(string email, string password)
        {
            // Validate email and password before proceeding
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email and password must be provided.");
            }

            UserDal userDal = new UserDal();
            return userDal.GetUserByEmailAndPassword(email, password);
        }

        public static List<Movie> GetFavoriteMovies(int userId)
        {
            MovieDal movieDal = new MovieDal();
            return movieDal.ShowWishlist(userId); // Use DAL method to fetch favorite movies
        }

        public static bool RemoveFavoriteMovie(int userId, int movieId)
        {
            MovieDal movieDal = new MovieDal();
            return movieDal.DeleteUserMovie(userId, movieId); // Use DAL method to unlink
        }





    }
}
