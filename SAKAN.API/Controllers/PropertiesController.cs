using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Application.Features.Properties.Commands.CreateProperty;
using SAKAN.Application.Features.Properties.Commands.DeleteProperty;
using SAKAN.Application.Features.Properties.Commands.UpdateProperty;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Application.Features.Properties.Queries.GetAllProperties;
using SAKAN.Application.Features.Properties.Queries.GetPropertyById;
using SAKAN.Application.Features.Properties.Commands.UpdatePropertyStatus;
using SAKAN.Application.Features.Media.Commands.AddPropertyMedia;
using SAKAN.Application.Features.Media.Commands.DeletePropertyMedia;
using SAKAN.Application.Features.Media.Commands.SetCoverMedia;
using SAKAN.Application.Features.Media.Commands.ReorderMedia;
using SAKAN.Application.Features.Media.Queries.GetPropertyMedia;
using SAKAN.Application.Common.Models;

namespace SAKAN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all properties with filtering, sorting, and pagination
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<PropertyListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<PropertyListDto>>> GetAll([FromQuery] GetAllPropertiesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a property by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropertyDto>> GetById(Guid id)
        {
            var query = new GetPropertyByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new property
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PropertyDto>> Create([FromBody] CreatePropertyCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing property
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropertyDto>> Update(Guid id, [FromBody] UpdatePropertyCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch between route and body.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete a property
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeletePropertyCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Update a property's status
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropertyDto>> UpdateStatus(Guid id, [FromBody] UpdatePropertyStatusCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch between route and body.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get all media items for a property
        /// </summary>
        [HttpGet("{id}/media")]
        [ProducesResponseType(typeof(List<PropertyMediaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PropertyMediaDto>>> GetMedia(Guid id)
        {
            var query = new GetPropertyMediaQuery { PropertyId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

/// <summary>
        /// Add a media item to a property
        /// </summary>
        [HttpPost("{id}/media")]
        [ProducesResponseType(typeof(PropertyMediaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropertyMediaDto>> AddMedia(Guid id, [FromBody] AddPropertyMediaCommand command)
        {
            if (id != command.PropertyId)
                return BadRequest("ID mismatch between route and body.");

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMedia), new { id = id }, result);
        }

        /// <summary>
        /// Delete a media item from a property
        /// </summary>
        [HttpDelete("{id}/media/{mediaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMedia(Guid id, Guid mediaId)
        {
            var command = new DeletePropertyMediaCommand { PropertyId = id, MediaId = mediaId };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Set a media item as the property cover
        /// </summary>
        [HttpPut("{id}/media/cover")]
        [ProducesResponseType(typeof(PropertyMediaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropertyMediaDto>> SetCover(Guid id, [FromBody] SetCoverMediaCommand command)
        {
            if (id != command.PropertyId)
                return BadRequest("ID mismatch between route and body.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Reorder media items for a property
        /// </summary>
        [HttpPut("{id}/media/reorder")]
        [ProducesResponseType(typeof(List<PropertyMediaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PropertyMediaDto>>> ReorderMedia(Guid id, [FromBody] ReorderMediaCommand command)
        {
            if (id != command.PropertyId)
                return BadRequest("ID mismatch between route and body.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
