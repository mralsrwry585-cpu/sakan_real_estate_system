using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Models.Lessor
{
    /// <summary>
    /// View model for the Property Details screen (Lovable OwnerPropertyDetails fidelity).
    /// Exposes the raw PropertyDto plus pre-rendered Arabic labels/badges and gallery helpers.
    /// </summary>
    public class PropertyDetailsViewModel
    {
        public PropertyDto Property { get; set; } = new();

        public string TypeLabel => Property.PropertyType.ToArabic();
        public string ContractLabel => Property.ContractType.ToArabic();
        public string StatusLabel => Property.Status.ToArabic();
        public string StatusTone => Property.Status.PropertyTone();
        public string PriceLabel => Property.Price.FormatPrice();

        public string CoverUrl { get; set; } = string.Empty;
        public List<PropertyMediaDto> Gallery { get; set; } = new();
        public List<string> AmenityNames { get; set; } = new();

        public List<PropertyStatusOptionViewModel> StatusOptions { get; set; } = new();

        /// <summary>True if the currently logged-in owner owns this property.</summary>
        public bool IsOwner { get; set; }
    }

    public class PropertyStatusOptionViewModel
    {
        public PropertyStatus Status { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Tone { get; set; } = "neutral";
        public bool IsCurrent { get; set; }
    }
}

