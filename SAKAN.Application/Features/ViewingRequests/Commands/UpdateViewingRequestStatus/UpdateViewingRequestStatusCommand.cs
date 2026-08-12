using MediatR;
using SAKAN.Application.Features.ViewingRequests.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.ViewingRequests.Commands.UpdateViewingRequestStatus
{
    public class UpdateViewingRequestStatusCommand : IRequest<ViewingRequestDto>
    {
        public Guid Id { get; set; }
        public ViewingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
    }
}
