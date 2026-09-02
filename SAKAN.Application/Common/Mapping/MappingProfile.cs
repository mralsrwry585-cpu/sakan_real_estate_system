using AutoMapper;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Entities;

namespace SAKAN.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Property mappings
            CreateMap<Property, PropertyDto>()
                .ForMember(d => d.OwnerName, opt => opt.MapFrom(s => s.Owner != null ? s.Owner.FullName : null))
                .ForMember(d => d.FavoritesCount, opt => opt.MapFrom(s => 0))
                .ForMember(d => d.ViewingRequestsCount, opt => opt.MapFrom(s => s.ViewingRequests != null ? s.ViewingRequests.Count : 0))
                .ForMember(d => d.BookingRequestsCount, opt => opt.MapFrom(s => s.BookingRequests != null ? s.BookingRequests.Count : 0))
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address))
                .ForMember(d => d.PropertyMedia, opt => opt.MapFrom(s => s.PropertyMedia))
                .ForMember(d => d.PropertyAmenities, opt => opt.MapFrom(s => s.PropertyAmenities));

            CreateMap<Address, AddressDto>();
            CreateMap<PropertyMedia, PropertyMediaDto>();
            CreateMap<PropertyAmenity, PropertyAmenityDto>()
                .ForMember(d => d.AmenityName, opt => opt.MapFrom(s => s.Amenity != null ? s.Amenity.Name : null))
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Amenity != null ? s.Amenity.Category : default));
        }
    }
}
