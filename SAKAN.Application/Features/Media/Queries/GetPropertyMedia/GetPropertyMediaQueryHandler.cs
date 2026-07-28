using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Media.Queries.GetPropertyMedia
{
    public class GetPropertyMediaQueryHandler : IRequestHandler<GetPropertyMediaQuery, IReadOnlyList<PropertyMediaDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertyMediaQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<PropertyMediaDto>> Handle(GetPropertyMediaQuery request, CancellationToken cancellationToken)
        {
            var media = await _context.PropertyMedia
                .Where(pm => pm.PropertyId == request.PropertyId)
                .OrderBy(pm => pm.DisplayOrder)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<PropertyMediaDto>>(media);
        }
    }
}
