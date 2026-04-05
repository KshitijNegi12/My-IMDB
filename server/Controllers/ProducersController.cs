using IMDB.Exceptions;
using IMDB.Models.Request;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IMDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProducersController : ControllerBase
    {
        private readonly IProducerService _producerService;

        public ProducersController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] ProducerRequest producer)
        {
            try
            {
                int producerId = _producerService.Add(producer);
                return CreatedAtAction(nameof(GetById), new { id = producerId }, new { id = producerId });
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
        public IActionResult GetAll()
        {
            return Ok(_producerService.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var producer = _producerService.GetById(id);
                return Ok(producer);
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
        public IActionResult Update([FromRoute] int id, [FromBody] ProducerRequest producer)
        {
            try
            {
                bool result = _producerService.Update(id, producer);
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

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                bool result = _producerService.Delete(id);
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
