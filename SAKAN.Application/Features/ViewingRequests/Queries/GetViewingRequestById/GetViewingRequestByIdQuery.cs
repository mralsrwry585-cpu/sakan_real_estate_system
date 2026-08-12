using MediatR;
using SAKAN.Application.Features.ViewingRequests.DTOs;

namespace SAKAN.Application.Features.ViewingRequests.Queries.GetViewingRequestById
{
    public class GetViewingRequestByIdQuery : IRequest<ViewingRequestDto>
    {
        public Guid Id { get; set; }
    }
}
