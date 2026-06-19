using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleForElastic.Application.Commands.Handler.AddCar;
using SampleForElastic.Application.Commands.Handler.CreateUser;
using SampleForElastic.Application.Commands.Handler.UpdateUser;
using SampleForElastic.Application.Queries.GetUserById;
using SampleForElastic.Application.Queries.SearchUsers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleForElastic.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetUserById), new { id = response.UserId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                command.UserId = id;
                var result = await _mediator.Send(command);
                if (result)
                {
                    return NoContent();
                }
                return BadRequest(new { Message = "Failed to update user." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("{id:guid}/cars")]
        public async Task<IActionResult> AddCar([FromRoute] Guid id, [FromBody] AddCarCommand command)
        {
            try
            {
                command.UserId = id;
                var carId = await _mediator.Send(command);
                return Ok(new { CarId = carId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var response = await _mediator.Send(query);
            if (response.User == null)
            {
                return NotFound(new { Message = $"User with ID {id} was not found in the search index." });
            }
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string term)
        {
            var query = new SearchUsersQuery(term);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
