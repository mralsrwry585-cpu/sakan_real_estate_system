using MediatR;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Properties.Commands.UpdatePropertyStatus
{
    public class UpdatePropertyStatusCommand : IRequest<PropertyDto>
    {
        public Guid Id { get; set; }
        public PropertyStatus Status { get; set; }
    }
}
