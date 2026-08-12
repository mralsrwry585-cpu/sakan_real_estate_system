using MediatR;
using SAKAN.Application.Features.ViewingRequests.DTOs;

namespace SAKAN.Application.Features.ViewingRequests.Commands.CreateViewingRequest
{
    public class CreateViewingRequestCommand : IRequest<ViewingRequestDto>
    {
        public Guid PropertyId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty;

        public Guid TenantId { get; set; }
        public string? Note { get; set; }
    }
}
