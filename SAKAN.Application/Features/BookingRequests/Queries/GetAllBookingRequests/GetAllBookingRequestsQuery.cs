using MediatR;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.BookingRequests.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.BookingRequests.Queries.GetAllBookingRequests
{
    public class GetAllBookingRequestsQuery : IRequest<PagedResult<BookingRequestDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? TenantId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? PropertyId { get; set; }
        public BookingStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }
}
