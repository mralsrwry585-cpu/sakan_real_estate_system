using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties
                .Include(p => p.Owner)
                .Include(p => p.Address)
                .Include(p => p.PropertyMedia)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (property == null)
                throw new KeyNotFoundException($"Property with ID {request.Id} was not found.");

            // Increment views
            property.Views++;
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PropertyDto>(property);
        }
    }
}
