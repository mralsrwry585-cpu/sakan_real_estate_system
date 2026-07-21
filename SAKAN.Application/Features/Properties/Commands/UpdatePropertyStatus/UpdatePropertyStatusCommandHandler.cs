using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Properties.Commands.UpdatePropertyStatus
{
    public class UpdatePropertyStatusCommandHandler : IRequestHandler<UpdatePropertyStatusCommand, PropertyDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePropertyStatusCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(UpdatePropertyStatusCommand request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties
                .Include(p => p.Owner)
                .Include(p => p.Address)
                .Include(p => p.PropertyMedia)
                .Include(p => p.PropertyAmenities)
                .ThenInclude(pa => pa.Amenity)
                .Include(p => p.ViewingRequests)
                .Include(p => p.BookingRequests)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (property == null)
                throw new KeyNotFoundException($"Property with ID {request.Id} was not found.");

            property.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PropertyDto>(property);
        }
    }
}
