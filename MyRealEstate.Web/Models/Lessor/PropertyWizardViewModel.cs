using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Models.Lessor
{
    /// <summary>Dropdown option for wizard selects.</summary>
    public class SelectOptionViewModel
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// Combined view model for the 5-step Add Property wizard (Lovable screen fidelity).
    /// Steps: 1) البيانات الأساسية  2) الموقع والعنوان  3) المرافق  4) الوسائط  5) المراجعة والنشر
    /// </summary>
    public class PropertyWizardViewModel
    {
        public const int TotalSteps = 5;

        public int CurrentStep { get; set; } = 1;

        // ---- Step 1: basic info ----
        public string Title { get; set; } = string.Empty;
        public PropertyType? PropertyType { get; set; }
        public ContractType? ContractType { get; set; }
        public decimal? Price { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public decimal? Area { get; set; }
        public int? FloorsCount { get; set; }
        public int? AgeYears { get; set; }
        public string Description { get; set; } = string.Empty;

        // ---- Step 2: location ----
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? BuildingNumber { get; set; }
        public string? Floor { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        // ---- Step 3: amenities ----
        public List<AmenityGroupSelectionViewModel> AmenityGroups { get; set; } = new();
        public List<Guid> SelectedAmenityIds { get; set; } = new();

        // ---- Step 4: media (targets the created property) ----
        public Guid? PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public List<PropertyMediaDto> MediaItems { get; set; } = new();

        // ---- Step 5: review + publish ----
        public bool PublishNow { get; set; } = true;

        // ---- Validation ----
        public bool AmenitiesLoaded { get; set; }
    }

    public class AmenityGroupSelectionViewModel
    {
        public string CategoryLabel { get; set; } = string.Empty;
        public List<AmenitySelectionViewModel> Items { get; set; } = new();
    }

    public class AmenitySelectionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}

