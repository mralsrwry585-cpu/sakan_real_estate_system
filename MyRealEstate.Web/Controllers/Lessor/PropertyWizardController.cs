using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Models.Lessor;
using MyRealEstate.Web.Services;

namespace MyRealEstate.Web.Controllers.Lessor
{
    [Authorize]
    [Route("owner/properties/new")]
    public class PropertyWizardController : Controller
    {
        private readonly PropertiesApiClient _properties;
        private readonly AmenitiesApiClient _amenities;

        public PropertyWizardController(PropertiesApiClient properties, AmenitiesApiClient amenities)
        {
            _properties = properties;
            _amenities = amenities;
        }

        [HttpGet]
        public IActionResult Index() => RedirectToAction(nameof(Step), new { step = 1 });

        [HttpGet("step/{step:int}")]
        public async Task<IActionResult> Step(int step, CancellationToken ct)
        {
            if (step < 1 || step > 5)
                step = 1;

            var state = HttpContext.Session.GetWizardState();

            var vm = state.ToViewModel(HarvestStep(state));
            vm.CurrentStep = step;

            // Step 3 needs the amenity catalog grouped by category.
            if (step == 3)
            {
                await LoadAmenityCatalogAsync(vm, ct);
            }

            // Step 4/5: if a draft exists, pull media
            if ((step == 4 || step == 5) && state.PropertyId is not null)
            {
                try
                {
                    var media = await _properties.GetMediaAsync(state.PropertyId.Value, ct);
                    if (media is not null)
                    {
                        vm.MediaItems = media.OrderBy(m => m.DisplayOrder).ToList();
                    }
                }
                catch (ApiClientException)
                {
                    // media may be empty
                }
            }

            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("step/{step:int}")]
        public async Task<IActionResult> Step(int step, PropertyWizardViewModel model, CancellationToken ct)
        {
            var state = HttpContext.Session.GetWizardState();

            switch (step)
            {
                case 1:
                    MapStep1(state, model);
                    break;
                case 2:
                    MapStep2(state, model);
                    break;
                case 3:
                    MapStep3(state, model);
                    break;
            }

            // Persist state
            HttpContext.Session.SetWizardState(state);

            // When leaving step 3 (amenities) toward step 4, create the draft property
            if (step == 3 && !state.IsDraftCreated)
            {
                var ownerId = HttpContext.Session.GetOwnerId();
                if (ownerId is null)
                    return RedirectToAction("Login", "Account");

                var draft = await CreateDraftAsync(state, ownerId.Value, ct);
                if (draft is null)
                {
                    TempData["Error"] = "تعذر إنشاء مسودة العقار. راجع البيانات المدخلة.";
                    return RedirectToAction(nameof(Step), new { step = 3 });
                }

                state.PropertyId = draft.Id;
                state.PropertyTitle = draft.Title;
                HttpContext.Session.SetWizardState(state);
            }

            var next = step + 1;
            if (next > 5)
                next = 5;

            return RedirectToAction(nameof(Step), new { step = next });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("publish")]
        public async Task<IActionResult> Publish(CancellationToken ct)
        {
            var state = HttpContext.Session.GetWizardState();
            if (state.PropertyId is null)
            {
                TempData["Error"] = "لا يوجد عقار قيد الإنشاء.";
                return RedirectToAction(nameof(Step), new { step = 5 });
            }

            try
            {
                var command = new UpdatePropertyStatusCommand
                {
                    Id = state.PropertyId.Value,
                    Status = PropertyStatus.Available
                };
                await _properties.UpdateStatusAsync(command, ct);

                var reference = $"PROP-{state.PropertyId.Value.ToString("N")[..5].ToUpperInvariant()}";
                HttpContext.Session.ClearWizardState();
                TempData["Published"] = reference;
                return RedirectToAction(nameof(Published));
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر نشر العقار. حاول مرة أخرى.";
                return RedirectToAction(nameof(Step), new { step = 5 });
            }
        }

        [HttpGet("published")]
        public IActionResult Published()
        {
            var reference = TempData["Published"]?.ToString();
            if (string.IsNullOrWhiteSpace(reference))
                return RedirectToAction("Index", "Properties");

            return View("Published", reference);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("cancel")]
        public IActionResult Cancel()
        {
            HttpContext.Session.ClearWizardState();
            return RedirectToAction("Index", "Properties");
        }

        // ---- Helpers ----

private async Task LoadAmenityCatalogAsync(PropertyWizardViewModel vm, CancellationToken ct)
        {
            try
            {
                var groups = await _amenities.GetAllAsync(false, ct);
                if (groups is not null)
                {
                    var selected = vm.SelectedAmenityIds.ToHashSet();
                    vm.AmenityGroups = groups
                        .Where(g => g.Amenities.Any())
                        .Select(g => new AmenityGroupSelectionViewModel
                        {
                            CategoryLabel = g.Category.ToArabic(),
                            Items = g.Amenities.Select(a => new AmenitySelectionViewModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                IsSelected = selected.Contains(a.Id)
                            }).ToList()
                        })
                        .ToList();
                    vm.AmenitiesLoaded = true;
                }
            }
            catch (ApiClientException)
            {
                // keep empty
            }
        }

        private async Task<PropertyDto?> CreateDraftAsync(PropertyWizardState state, Guid ownerId, CancellationToken ct)
        {
            var command = new CreatePropertyCommand
            {
                OwnerId = ownerId,
                Title = state.Title,
                Description = state.Description,
                PropertyType = state.PropertyType ?? PropertyType.Apartment,
                ContractType = state.ContractType ?? ContractType.Rent,
                Price = state.Price ?? 0,
                Area = state.Area ?? 0,
                Bedrooms = state.Bedrooms ?? 0,
                Bathrooms = state.Bathrooms ?? 0,
                FloorsCount = state.FloorsCount ?? 1,
                AgeYears = state.AgeYears ?? 0,
                City = state.City,
                District = state.District,
                Street = state.Street,
                PostalCode = state.PostalCode,
                BuildingNumber = state.BuildingNumber,
                Floor = state.Floor,
                Latitude = state.Latitude,
                Longitude = state.Longitude,
                AmenityIds = state.AmenityIds
            };

            try
            {
                return await _properties.CreateAsync(command, ct);
            }
            catch (ApiClientException)
            {
                return null;
            }
        }

        private static void MapStep1(PropertyWizardState state, PropertyWizardViewModel model)
        {
            state.Title = model.Title;
            state.Description = model.Description;
            state.PropertyType = model.PropertyType;
            state.ContractType = model.ContractType;
            state.Price = model.Price;
            state.Bedrooms = model.Bedrooms;
            state.Bathrooms = model.Bathrooms;
            state.Area = model.Area;
            state.FloorsCount = model.FloorsCount;
            state.AgeYears = model.AgeYears;
        }

        private static void MapStep2(PropertyWizardState state, PropertyWizardViewModel model)
        {
            state.City = model.City;
            state.District = model.District;
            state.Street = model.Street;
            state.PostalCode = model.PostalCode;
            state.BuildingNumber = model.BuildingNumber;
            state.Floor = model.Floor;
            state.Latitude = model.Latitude;
            state.Longitude = model.Longitude;
        }

        private static void MapStep3(PropertyWizardState state, PropertyWizardViewModel model)
        {
            if (model.SelectedAmenityIds.Count > 0)
                state.AmenityIds = model.SelectedAmenityIds;
            else if (model.AmenityGroups.Any())
                state.AmenityIds = model.AmenityGroups
                    .SelectMany(g => g.Items)
                    .Where(i => i.IsSelected)
                    .Select(i => i.Id)
                    .ToList();
        }

        private static int HarvestStep(PropertyWizardState state)
        {
            // Determine furthest completed step based on state
            if (!string.IsNullOrWhiteSpace(state.Title) && state.Price is not null)
                return 2;
            if (!string.IsNullOrWhiteSpace(state.City) && !string.IsNullOrWhiteSpace(state.District))
                return 3;
            if (state.AmenityIds.Count > 0)
                return 4;
            return 1;
        }
    }
}
