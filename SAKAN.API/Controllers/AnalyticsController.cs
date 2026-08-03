using MediatR;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Application.Features.Analytics.DTOs;
using SAKAN.Application.Features.Analytics.Queries.GetOwnerDashboardStats;

namespace SAKAN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnalyticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get owner dashboard statistics (properties, requests, revenue, monthly trends)
        /// </summary>
        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(typeof(OwnerDashboardStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OwnerDashboardStatsDto>> GetOwnerDashboardStats(Guid ownerId)
        {
            var query = new GetOwnerDashboardStatsQuery { OwnerId = ownerId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
