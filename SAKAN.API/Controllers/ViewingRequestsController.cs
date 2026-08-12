using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.ViewingRequests.Commands.CreateViewingRequest;
using SAKAN.Application.Features.ViewingRequests.Commands.UpdateViewingRequestStatus;
using SAKAN.Application.Features.ViewingRequests.DTOs;
using SAKAN.Application.Features.ViewingRequests.Queries.GetAllViewingRequests;
using SAKAN.Application.Features.ViewingRequests.Queries.GetViewingRequestById;

namespace SAKAN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ViewingRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ViewingRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all viewing requests with filtering, sorting, and pagination
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ViewingRequestDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ViewingRequestDto>>> GetAll([FromQuery] GetAllViewingRequestsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a viewing request by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewingRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewingRequestDto>> GetById(Guid id)
        {
            var query = new GetViewingRequestByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new viewing request (by Tenant)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ViewingRequestDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ViewingRequestDto>> Create([FromBody] CreateViewingRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update viewing request status (by Owner)
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(ViewingRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewingRequestDto>> UpdateStatus(Guid id, [FromBody] UpdateViewingRequestStatusCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch between route and body.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
