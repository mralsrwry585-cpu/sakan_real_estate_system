using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Media.Commands.SetCoverMedia
{
    public class SetCoverMediaCommandHandler : IRequestHandler<SetCoverMediaCommand, PropertyMediaDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SetCoverMediaCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PropertyMediaDto> Handle(SetCoverMediaCommand request, CancellationToken cancellationToken)
        {
            var media = await _context.PropertyMedia
                .FirstOrDefaultAsync(pm => pm.Id == request.MediaId && pm.PropertyId == request.PropertyId, cancellationToken);

            if (media == null)
                throw new KeyNotFoundException($"Media with ID {request.MediaId} was not found for property {request.PropertyId}.");

            // Unset all existing covers for this property
            var existingCovers = await _context.PropertyMedia
                .Where(pm => pm.PropertyId == request.PropertyId && pm.IsCover)
                .ToListAsync(cancellationToken);

            foreach (var cover in existingCovers)
                cover.IsCover = false;

            // Set the new cover
            media.IsCover = true;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PropertyMediaDto>(media);
        }
    }
}
