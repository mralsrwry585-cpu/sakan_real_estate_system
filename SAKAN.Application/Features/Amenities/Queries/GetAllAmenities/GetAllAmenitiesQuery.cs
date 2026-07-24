using MediatR;
using SAKAN.Application.Features.Amenities.DTOs;

namespace SAKAN.Application.Features.Amenities.Queries.GetAllAmenities
{
    public class GetAllAmenitiesQuery : IRequest<IReadOnlyList<AmenityGroupDto>>
    {
        public bool IncludeInactive { get; set; } = false;
    }
}
