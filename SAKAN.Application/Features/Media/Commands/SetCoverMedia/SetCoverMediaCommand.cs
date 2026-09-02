using MediatR;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Media.Commands.SetCoverMedia
{
    public class SetCoverMediaCommand : IRequest<PropertyMediaDto>
    {
        public Guid PropertyId { get; set; }
        public Guid MediaId { get; set; }
    }
}
