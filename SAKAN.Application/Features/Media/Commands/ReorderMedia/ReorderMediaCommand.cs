using MediatR;

namespace SAKAN.Application.Features.Media.Commands.ReorderMedia
{
    public class ReorderMediaCommand : IRequest<Unit>
    {
        public Guid PropertyId { get; set; }
        public List<MediaOrderItem> Items { get; set; } = new();
    }

    public class MediaOrderItem
    {
        public Guid MediaId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
