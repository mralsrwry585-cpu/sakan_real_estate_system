using MediatR;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Media.Commands.AddPropertyMedia
{
    public class AddPropertyMediaCommand : IRequest<PropertyMediaDto>
    {
        public Guid PropertyId { get; set; }
        public string Url { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public bool IsCover { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
