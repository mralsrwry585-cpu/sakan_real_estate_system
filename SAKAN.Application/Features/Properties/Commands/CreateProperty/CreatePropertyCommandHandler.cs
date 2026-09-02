using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Entities;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreatePropertyCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = new Property
            {
                Id = Guid.NewGuid(),
                OwnerId = request.OwnerId,
                Title = request.Title,
                Description = request.Description,
                PropertyType = request.PropertyType,
                ContractType = request.ContractType,
                Price = request.Price,
                Area = request.Area,
                Bedrooms = request.Bedrooms,
                Bathrooms = request.Bathrooms,
                FloorsCount = request.FloorsCount,
                AgeYears = request.AgeYears,
                Status = PropertyStatus.Draft,
                Views = 0,
                CreatedAt = DateTime.UtcNow,
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    City = request.City,
                    District = request.District,
                    Street = request.Street,
                    PostalCode = request.PostalCode,
                    BuildingNumber = request.BuildingNumber,
                    Floor = request.Floor,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                }
            };

            await _context.Properties.AddAsync(property, cancellationToken);

            // Add amenities
            if (request.AmenityIds.Any())
            {
                foreach (var amenityId in request.AmenityIds)
                {
                    var propertyAmenity = new PropertyAmenity
                    {
                        Id = Guid.NewGuid(),
                        PropertyId = property.Id,
                        AmenityId = amenityId
                    };
                    await _context.PropertyAmenities.AddAsync(propertyAmenity, cancellationToken);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Reload with includes for mapping
            var createdProperty = await _context.Properties
                .Include(p => p.Owner)
                .Include(p => p.Address)
                .Include(p => p.PropertyMedia)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == property.Id, cancellationToken);

            return _mapper.Map<PropertyDto>(createdProperty);
        }
    }
}
