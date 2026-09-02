using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;

namespace SAKAN.Application.Features.Media.Commands.DeletePropertyMedia
{
    public class DeletePropertyMediaCommandHandler : IRequestHandler<DeletePropertyMediaCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeletePropertyMediaCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePropertyMediaCommand request, CancellationToken cancellationToken)
        {
            var media = await _context.PropertyMedia
                .FirstOrDefaultAsync(pm => pm.Id == request.MediaId && pm.PropertyId == request.PropertyId, cancellationToken);

            if (media == null)
                throw new KeyNotFoundException($"Media with ID {request.MediaId} was not found for property {request.PropertyId}.");

            var wasCover = media.IsCover;

            _context.PropertyMedia.Remove(media);
            await _context.SaveChangesAsync(cancellationToken);

            // If the deleted media was the cover, promote the first remaining media to cover
            if (wasCover)
            {
                var nextMedia = await _context.PropertyMedia
                    .Where(pm => pm.PropertyId == request.PropertyId)
                    .OrderBy(pm => pm.DisplayOrder)
                    .FirstOrDefaultAsync(cancellationToken);

                if (nextMedia != null)
                {
                    nextMedia.IsCover = true;
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            return Unit.Value;
        }
    }
}
