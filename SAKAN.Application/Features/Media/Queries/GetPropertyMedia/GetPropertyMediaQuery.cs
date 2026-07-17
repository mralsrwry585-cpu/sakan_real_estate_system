using MediatR;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Media.Queries.GetPropertyMedia
{
    public class GetPropertyMediaQuery : IRequest<IReadOnlyList<PropertyMediaDto>>
    {
        public Guid PropertyId { get; set; }
    }
}
