using MediatR;

namespace SAKAN.Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
