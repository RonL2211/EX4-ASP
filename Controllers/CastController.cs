using Matala2_ASP.BL;
using Matala2_ASP.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Matala2_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController : ControllerBase
    {
        // GET: api/Cast
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                List<Cast> casts = Cast.GetAllCasts();
                if (casts == null)
                {
                    return NotFound("No casts found.");
                }
                else
                    return Ok(casts);

            }
            catch (Exception)
            {
                    return BadRequest("Failed to get casts.");
                throw;
            }
        }

        //// GET api/Cast/{id}
        //[HttpGet("{id}")]
        //public ActionResult<Cast> Get(string id)
        //{
        //    var cast = Cast.GetAllCasts().Find(c => c.Id == id);
        //    if (cast == null) return NotFound("Cast not found.");
        //    return Ok(cast);
        //}

        // POST api/Cast
        [HttpPost]
        public IActionResult Post([FromBody] Cast newCast)
        {
            bool success = Cast.Insert(newCast);
            if (!success) return BadRequest("Failed to add the cast.");
            return Ok("Cast added successfully.");
        }

        //// PUT api/Cast/{id}
        //[HttpPut("{id}")]
        //public IActionResult Put(string id, [FromBody] Cast updatedCast)
        //{
        //    updatedCast.Id = id; // Ensure ID remains unchanged
        //    var success = Cast.Update(updatedCast);
        //    if (!success) return NotFound("Failed to update the cast.");
        //    return Ok("Cast updated successfully.");
        //}

        //// DELETE api/Cast/{id}
        //[HttpDelete("{id}")]
        //public IActionResult Delete(string id)
        //{
        //    var success = Cast.Delete(id);
        //    if (!success) return NotFound("Failed to delete the cast.");
        //    return Ok("Cast deleted successfully.");
        //}

        //public static List<Cast> GetAllMovieCasts(int movieId)
        //{
        //    CastDal castDal = new CastDal();
        //    return castDal.GetAllMovieCasts(movieId);
        //}

        //public static bool LinkToMovie(int movieId, string castId)
        //{
        //    CastDal castDal = new CastDal();
        //    return castDal.InsertMovieCast(movieId, castId);
        //}

        //// Fetch all casts for a specific movie
        //[HttpGet("movie/{movieId}")]
        //public ActionResult<List<Cast>> GetCastsForMovie(int movieId)
        //{
        //    // Fetch casts using the BL method
        //    var casts = Cast.GetAllMovieCasts(movieId);

        //    // Check if the list is empty
        //    if (casts == null || casts.Count == 0)
        //    {
        //        return NotFound("No casts found for this movie.");
        //    }

        //    // Return the list of casts
        //    return Ok(casts);
        //}


        // Link a cast to a movie
        [HttpPost("link/{movieId}/{castId}")]
        public IActionResult LinkCastToMovie(int movieId, string castId)
        {
            bool success = Cast.LinkToMovie(movieId, castId); // Use BL method
            if (!success) return BadRequest("Failed to link cast to movie.");
            return Ok("Cast linked to movie successfully.");
        }

    }
}
