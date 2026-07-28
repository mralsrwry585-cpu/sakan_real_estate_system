using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;

namespace SAKAN.Application.Features.Media.Commands.ReorderMedia
{
    public class ReorderMediaCommandHandler : IRequestHandler<ReorderMediaCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public ReorderMediaCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ReorderMediaCommand request, CancellationToken cancellationToken)
        {
            var mediaIds = request.Items.Select(i => i.MediaId).ToList();

            var mediaItems = await _context.PropertyMedia
                .Where(pm => pm.PropertyId == request.PropertyId && mediaIds.Contains(pm.Id))
                .ToListAsync(cancellationToken);

            var orderMap = request.Items.ToDictionary(i => i.MediaId, i => i.DisplayOrder);

            foreach (var media in mediaItems)
            {
                if (orderMap.TryGetValue(media.Id, out var order))
                    media.DisplayOrder = order;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
