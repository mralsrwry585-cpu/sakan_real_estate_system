using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.BookingRequests.Commands.CreateBookingRequest;
using SAKAN.Application.Features.BookingRequests.Commands.UpdateBookingRequestStatus;
using SAKAN.Application.Features.BookingRequests.DTOs;
using SAKAN.Application.Features.BookingRequests.Queries.GetAllBookingRequests;
using SAKAN.Application.Features.BookingRequests.Queries.GetBookingRequestById;

namespace SAKAN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BookingRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all booking requests with filtering, sorting, and pagination
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BookingRequestDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<BookingRequestDto>>> GetAll([FromQuery] GetAllBookingRequestsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a booking request by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookingRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingRequestDto>> GetById(Guid id)
        {
            var query = new GetBookingRequestByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new booking request (by Tenant)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BookingRequestDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingRequestDto>> Create([FromBody] CreateBookingRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update booking request status (by Owner)
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(BookingRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingRequestDto>> UpdateStatus(Guid id, [FromBody] UpdateBookingRequestStatusCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch between route and body.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
