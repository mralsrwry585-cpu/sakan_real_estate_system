using MediatR;
using SAKAN.Application.Features.Analytics.DTOs;

namespace SAKAN.Application.Features.Analytics.Queries.GetOwnerDashboardStats
{
    public class GetOwnerDashboardStatsQuery : IRequest<OwnerDashboardStatsDto>
    {
        public Guid OwnerId { get; set; }
    }
}
