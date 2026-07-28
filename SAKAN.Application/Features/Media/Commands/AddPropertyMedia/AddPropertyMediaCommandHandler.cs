using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Entities;

namespace SAKAN.Application.Features.Media.Commands.AddPropertyMedia
{
    public class AddPropertyMediaCommandHandler : IRequestHandler<AddPropertyMediaCommand, PropertyMediaDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddPropertyMediaCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PropertyMediaDto> Handle(AddPropertyMediaCommand request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == request.PropertyId, cancellationToken);

            if (property == null)
                throw new KeyNotFoundException($"Property with ID {request.PropertyId} was not found.");

            // Determine display order
            var maxOrder = await _context.PropertyMedia
                .Where(pm => pm.PropertyId == request.PropertyId)
                .MaxAsync(pm => (int?)pm.DisplayOrder, cancellationToken) ?? 0;

            var media = new PropertyMedia
            {
                Id = Guid.NewGuid(),
                PropertyId = request.PropertyId,
                Url = request.Url,
                MediaType = request.MediaType,
                IsCover = request.IsCover,
                DisplayOrder = request.DisplayOrder ?? maxOrder + 1
            };

            // If this is the cover, unset any existing cover
            if (request.IsCover)
            {
                var existingCovers = await _context.PropertyMedia
                    .Where(pm => pm.PropertyId == request.PropertyId && pm.IsCover)
                    .ToListAsync(cancellationToken);

                foreach (var cover in existingCovers)
                    cover.IsCover = false;
            }
            // If no cover exists, make the first media the cover automatically
            else if (maxOrder == 0)
            {
                media.IsCover = true;
            }

            await _context.PropertyMedia.AddAsync(media, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PropertyMediaDto>(media);
        }
    }
}
