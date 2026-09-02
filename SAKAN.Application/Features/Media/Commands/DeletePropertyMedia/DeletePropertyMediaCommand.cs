using MediatR;

namespace SAKAN.Application.Features.Media.Commands.DeletePropertyMedia
{
    public class DeletePropertyMediaCommand : IRequest<Unit>
    {
        public Guid PropertyId { get; set; }
        public Guid MediaId { get; set; }
    }
}
