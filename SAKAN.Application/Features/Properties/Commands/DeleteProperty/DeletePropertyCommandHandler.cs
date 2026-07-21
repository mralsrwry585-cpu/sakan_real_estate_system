using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;

namespace SAKAN.Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeletePropertyCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties
                .Include(p => p.Address)
                .Include(p => p.PropertyMedia)
                .Include(p => p.PropertyAmenities)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (property == null)
                throw new KeyNotFoundException($"Property with ID {request.Id} was not found.");

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
