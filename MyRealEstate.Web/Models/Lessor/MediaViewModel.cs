using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Models.Lessor
{
    /// <summary>
    /// View model for the Media Management screen (Lovable OwnerMedia fidelity).
    /// </summary>
    public class MediaViewModel
    {
        public Guid PropertyId { get; set; }
        public string PropertyTitle { get; set; } = "العقار";
        public List<MediaItemViewModel> Items { get; set; } = new();
        public int TotalCount => Items.Count;
        public bool IsOwner { get; set; }
    }

    public class MediaItemViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public MediaType Type { get; set; }
        public bool IsCover { get; set; }
        public int DisplayOrder { get; set; }
    }
}

