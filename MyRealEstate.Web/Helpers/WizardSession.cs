using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Models.Lessor;
using System.Text.Json;

namespace MyRealEstate.Web.Helpers
{
    /// <summary>
    /// Session-backed wizard state for the 5-step Add Property flow.
    /// Steps 1-3 accumulate data; a draft property is created on the backend
    /// when the user proceeds to media (step 4). The persisted property id is
    /// kept here so step 4/5 can operate on it.
    /// </summary>
    public class PropertyWizardState
    {
        // Step 1
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType? PropertyType { get; set; }
        public ContractType? ContractType { get; set; }
        public decimal? Price { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public decimal? Area { get; set; }
        public int? FloorsCount { get; set; }
        public int? AgeYears { get; set; }

        // Step 2
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? BuildingNumber { get; set; }
        public string? Floor { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        // Step 3
        public List<Guid> AmenityIds { get; set; } = new();

        // Step 4/5 (persisted draft)
        public Guid? PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;

        /// <summary>True once the draft property has been created on the backend.</summary>
        public bool IsDraftCreated => PropertyId is not null;
    }

    public static class WizardSession
    {
        private const string SessionKey = "PropertyWizardState";
        private static readonly JsonSerializerOptions Options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public static PropertyWizardState GetWizardState(this ISession session)
        {
            var json = session.GetString(SessionKey);
            if (string.IsNullOrWhiteSpace(json))
                return new PropertyWizardState();

            try
            {
                return JsonSerializer.Deserialize<PropertyWizardState>(json, Options) ?? new PropertyWizardState();
            }
            catch
            {
                return new PropertyWizardState();
            }
        }

        public static void SetWizardState(this ISession session, PropertyWizardState state) =>
            session.SetString(SessionKey, JsonSerializer.Serialize(state, Options));

        public static void ClearWizardState(this ISession session) => session.Remove(SessionKey);
    }

    public static class WizardStateMapper
    {
        /// <summary>
        /// Map the persisted wizard state to the view model.
        /// Amenity groups are only rebuilt when an explicit list of amenity ids is
        /// provided (generally after fetching from the API).
        /// </summary>
        public static PropertyWizardViewModel ToViewModel(this PropertyWizardState state, int furthestStep)
        {
            return new PropertyWizardViewModel
            {
                CurrentStep = furthestStep,
                Title = state.Title,
                Description = state.Description,
                PropertyType = state.PropertyType,
                ContractType = state.ContractType,
                Price = state.Price,
                Bedrooms = state.Bedrooms,
                Bathrooms = state.Bathrooms,
                Area = state.Area,
                FloorsCount = state.FloorsCount,
                AgeYears = state.AgeYears,
                City = state.City,
                District = state.District,
                Street = state.Street,
                PostalCode = state.PostalCode,
                BuildingNumber = state.BuildingNumber,
                Floor = state.Floor,
                Latitude = state.Latitude,
                Longitude = state.Longitude,
                PropertyId = state.PropertyId,
                PropertyTitle = state.PropertyTitle,
                SelectedAmenityIds = state.AmenityIds,
                AmenitiesLoaded = state.AmenityIds.Count > 0
            };
        }
    }
}

