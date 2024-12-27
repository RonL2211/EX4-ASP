using Matala2_ASP.BL;
using Matala2_ASP.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Matala2_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Register a new user
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserOfMovies newUser)
        {
            int result = newUser.Insert();
            if (result > 0)
                return Ok(new { message = "User registered successfully." });
            return BadRequest(new { message = "Failed to register user." });
        }

        // Login a user
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserOfMovies loginUser)
        {
            UserOfMovies user = UserOfMovies.Login(loginUser.Email, loginUser.Password); // Call the DAL method

            if (user == null)
                return Unauthorized("Invalid email or password.");
            return Ok(user);
        }

        // Get all users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            UserDal userDal = new UserDal(); // Use the DAL instance
            List<UserOfMovies> users = userDal.GetAllUsers(); // Call the DAL method

            if (users.Count == 0)
                return NotFound("No users found.");
            return Ok(users);
        }


        //// Update user
        //[HttpPut("{id}")]
        //public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        //{
        //    updatedUser.Id = id;
        //    bool result = updatedUser.Update();
        //    if (result == true)
        //        return Ok("User updated successfully.");
        //    return BadRequest("Failed to update user.");
        //}

        //// GET: api/User/favorites
        //[HttpGet("favorites")]
        //public IActionResult GetUsersWithFavorites()
        //{
        //    UserDal userDal = new UserDal();
        //    var users = userDal.GetAllUsers(); // Fetch all users

        //    if (users.Count == 0)
        //        return NotFound("No users found.");

        //    var userFavorites = new Dictionary<string, List<Movie>>();
        //    foreach (var user in users)
        //    {
        //        var favorites = Movie.GetAllUserMovies(user.Id); // Fetch movies for each user
        //        userFavorites[user.UserName] = favorites;
        //    }

        //    return Ok(userFavorites);
        //}

        //// GET: api/User/{userId}/favorites
        //[HttpGet("{userId}/favorites")]
        //public IActionResult GetUserFavorites(int userId)
        //{
        //    var favorites = Movie.GetAllUserMovies(userId); // Fetch movies for the user
        //    if (favorites == null || favorites.Count == 0)
        //        return NotFound("No favorite movies found for this user.");

        //    return Ok(favorites);
        //}

        //// DELETE: api/User/{userId}/favorites/{movieId}
        //[HttpDelete("{userId}/favorites/{movieId}")]
        //public IActionResult RemoveUserFavorite(int userId, int movieId)
        //{
        //    var success = Movie.UnlinkFromUser(userId, movieId); // Use Movie's method to unlink
        //    if (!success) return BadRequest("Failed to remove the movie from the user's favorites.");
        //    return Ok("Movie removed from the user's favorites successfully.");
        //}





    }
}
