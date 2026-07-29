using MediatR;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.ViewingRequests.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.ViewingRequests.Queries.GetAllViewingRequests
{
    public class GetAllViewingRequestsQuery : IRequest<PagedResult<ViewingRequestDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? TenantId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? PropertyId { get; set; }
        public ViewingStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }
}
