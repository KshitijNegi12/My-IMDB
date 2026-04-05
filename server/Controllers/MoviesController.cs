using IMDB.Exceptions;
using IMDB.Models.Request;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IMDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;

        public MoviesController(IMovieService movieService, IReviewService reviewService)
        {
            _movieService = movieService;
            _reviewService = reviewService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] MovieRequest movie)
        {
            try
            {
                int movieId = _movieService.Add(movie);
                return CreatedAtAction(nameof(GetById), new { id = movieId }, new { id = movieId});
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidFieldException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? year = null, [FromQuery] bool home = false)
        {
            return Ok(_movieService.GetAll(year, home));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var movie = _movieService.GetById(id);
                return Ok(movie);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] MovieRequest movie)
        {
            try
            {
                bool result = _movieService.Update(id, movie);
                return Ok(new { isUpdated = result });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidFieldException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}/cover-image")]
        public async Task<IActionResult> UpdateCoverImage([FromRoute] int id, IFormFile img)
        {
            try
            {
                var result = await _movieService.UpdateCover(id, img);
                return Ok(new { URL = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                bool result = _movieService.Delete(id);
                return Ok(new { isDeleted = result });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // for reviews
        [HttpPost("{movieId:int}/reviews")]
        public IActionResult AddReview([FromRoute] int movieId, [FromBody] ReviewRequest review)
        {
            try
            {
                int reviewId = _reviewService.Add(movieId, review);
                return CreatedAtAction(nameof(GetReviewById), new { movieId = movieId, id = reviewId }, null);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidFieldException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{movieId:int}/reviews")]
        public IActionResult GetAllReviews([FromRoute] int movieId)
        {
            try
            {
                var reviews = _reviewService.GetAll(movieId);
                return Ok(reviews);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{movieId:int}/reviews/{id:int}")]
        public IActionResult GetReviewById([FromRoute] int id, [FromRoute] int movieId)
        {
            try
            {
                var review = _reviewService.GetById(id, movieId);
                return Ok(review);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{movieId:int}/reviews/{id:int}")]
        public IActionResult UpdateReview([FromRoute] int id, [FromRoute] int movieId, [FromBody] ReviewRequest review)
        {
            try
            {
                bool result = _reviewService.Update(id, movieId, review);
                return Ok(new { isUpdated = result });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidFieldException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{movieId:int}/reviews/{id:int}")]
        public IActionResult DeleteReview([FromRoute] int id, [FromRoute] int movieId)
        {
            try
            {
                bool result = _reviewService.Delete(id, movieId);
                return Ok(new { isDeleted = result });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
