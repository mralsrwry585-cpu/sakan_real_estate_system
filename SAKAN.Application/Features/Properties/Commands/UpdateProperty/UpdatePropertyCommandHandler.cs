using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Entities;

namespace SAKAN.Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePropertyCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties
                .Include(p => p.Address)
                .Include(p => p.PropertyAmenities)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (property == null)
                throw new KeyNotFoundException($"Property with ID {request.Id} was not found.");

            // Update property fields
            property.Title = request.Title;
            property.Description = request.Description;
            property.PropertyType = request.PropertyType;
            property.ContractType = request.ContractType;
            property.Price = request.Price;
            property.Area = request.Area;
            property.Bedrooms = request.Bedrooms;
            property.Bathrooms = request.Bathrooms;
            property.FloorsCount = request.FloorsCount;
            property.AgeYears = request.AgeYears;
            property.Status = request.Status;

            // Update address
            if (property.Address != null)
            {
                property.Address.City = request.City;
                property.Address.District = request.District;
                property.Address.Street = request.Street;
                property.Address.PostalCode = request.PostalCode;
                property.Address.BuildingNumber = request.BuildingNumber;
                property.Address.Floor = request.Floor;
                property.Address.Latitude = request.Latitude;
                property.Address.Longitude = request.Longitude;
            }

            // Update amenities
            var existingAmenities = property.PropertyAmenities.ToList();
            foreach (var existing in existingAmenities)
            {
                _context.PropertyAmenities.Remove(existing);
            }

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

            await _context.SaveChangesAsync(cancellationToken);

            // Reload with includes
            var updatedProperty = await _context.Properties
                .Include(p => p.Owner)
                .Include(p => p.Address)
                .Include(p => p.PropertyMedia)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == property.Id, cancellationToken);

            return _mapper.Map<PropertyDto>(updatedProperty);
        }
    }
}
