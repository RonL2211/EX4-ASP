using Matala2_ASP.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Collections.Generic;

namespace Matala2_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        // GET: api/Movie
        [HttpGet]
        public ActionResult Get()
        {
            List<object> movies = Movie.GetAllMovies();
            if (movies == null)
            {
                return NotFound("No movies found.");
            }
            else
               return  Ok(movies);

        }

        //// GET api/Movie/{id}
        //[HttpGet("{id}")]
        //public ActionResult<Movie> Get(int id)
        //{
        //    var movie = Movie.GetAllMovies().Find(m => m.Id == id);
        //    if (movie == null) return NotFound("Movie not found.");
        //    return Ok(movie);
        //}

        // POST api/Movie
        [HttpPost]
        public IActionResult Post([FromBody] Movie newMovie)
        {
            var success = Movie.Insert(newMovie);
            if (!success) return BadRequest("Failed to add the movie.");
            return Ok("Movie added successfully.");
        }

        [HttpPost("wishlist/{userId}/{movieId}")]
        public IActionResult Post(int userId, int movieId)
        {
            bool success = Movie.InsertWishlist(userId,movieId);
            if (!success) return BadRequest("Failed to add the movie.");
            return Ok("Movie added to wishlist successfully.");
        }


       // GET api/Movie/wishlist/{id}
        [HttpGet("wishlist/{id}")]
        public ActionResult<Movie> Get(int id)
        {
            try
            {
                List<Movie> wishlist = Movie.ShowWishlist(id);
                return Ok(wishlist);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"server error: {e.Message}");
                throw;
            }

        }





        // PUT api/Movie/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Movie updatedMovie)
        {
            updatedMovie.Id = id; // Ensure ID remains unchanged
            var success = Movie.Update(updatedMovie);
            if (!success) return NotFound("Failed to update the movie.");
            return Ok("Movie updated successfully.");
        }

        // DELETE api/Movie/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = Movie.Delete(id);
            if (!success) return NotFound("Failed to delete the movie.");
            return Ok("Movie deleted successfully.");
        }

        // POST: api/Movie/link/{userId}/{movieId}
        [HttpPost("link/{userId}/{movieId}")]
        public IActionResult LinkUserToMovie(int userId, int movieId)
        {
            bool success = Movie.LinkToUser(userId, movieId); // Implement `LinkToUser` in `Movie.cs`
            if (!success) return BadRequest("Failed to link user to movie.");
            return Ok("Movie linked to user successfully.");
        }

        // GET: api/Movie/user/{userId}
        [HttpGet("user/{userId}")]
        public ActionResult<List<Movie>> GetAllUserMovies(int userId)
        {
            var movies = Movie.GetAllUserMovies(userId); // Implement `GetMoviesForUser` in `Movie.cs`
            if (movies == null || movies.Count == 0) return NotFound("No movies found for this user.");
            return Ok(movies);
        }

        // DELETE: api/Movie/unlink/{userId}/{movieId}
        [HttpDelete("unlink/{userId}/{movieId}")]
        public IActionResult UnlinkUserFromMovie(int userId, int movieId)
        {
            bool success = Movie.UnlinkFromUser(userId, movieId); // Implement `UnlinkFromUser` in `Movie.cs`
            if (!success) return BadRequest("Failed to unlink user from movie.");
            return Ok("Movie unlinked from user successfully.");
        }



    }
}
