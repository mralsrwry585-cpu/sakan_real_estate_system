using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Application.Features.Amenities.DTOs;
using SAKAN.Application.Features.Amenities.Queries.GetAllAmenities;

namespace SAKAN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AmenitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AmenitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all amenities grouped by category
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<AmenityGroupDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AmenityGroupDto>>> GetAll([FromQuery] bool includeInactive = false)
        {
            var query = new GetAllAmenitiesQuery { IncludeInactive = includeInactive };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
