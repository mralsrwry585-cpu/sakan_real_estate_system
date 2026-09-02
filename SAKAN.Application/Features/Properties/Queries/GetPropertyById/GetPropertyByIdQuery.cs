using MediatR;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQuery : IRequest<PropertyDto>
    {
        public Guid Id { get; set; }
    }
}
