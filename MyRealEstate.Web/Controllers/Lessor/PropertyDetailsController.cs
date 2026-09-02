using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Models.Lessor;
using MyRealEstate.Web.Services;

namespace MyRealEstate.Web.Controllers.Lessor
{
    [Authorize]
    [Route("owner/properties")]
    public class PropertyDetailsController : Controller
    {
        private readonly PropertiesApiClient _properties;

        public PropertyDetailsController(PropertiesApiClient properties)
        {
            _properties = properties;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Index(Guid id, CancellationToken ct)
        {
            var ownerId = HttpContext.Session.GetOwnerId();
            if (ownerId is null)
                return RedirectToAction("Login", "Account");

            try
            {
                var property = await _properties.GetByIdAsync(id, ct);
                if (property is null)
                {
                    TempData["Error"] = "العقار غير موجود.";
                    return RedirectToAction("Index", "Properties");
                }

                var vm = new PropertyDetailsViewModel
                {
                    Property = property,
                    IsOwner = property.OwnerId == ownerId.Value,
                    CoverUrl = property.PropertyMedia.FirstOrDefault(m => m.IsCover)?.Url
                               ?? property.PropertyMedia.FirstOrDefault()?.Url
                               ?? string.Empty,
                    Gallery = property.PropertyMedia.OrderBy(m => m.DisplayOrder).ToList(),
                    AmenityNames = property.PropertyAmenities.Select(a => a.AmenityName).ToList()
                };

                // Status options labeled per Lovable
                vm.StatusOptions = BuildStatusOptions(property.Status);

                return View(vm);
            }
            catch (ApiClientException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    TempData["Error"] = "العقار غير موجود.";
                    return RedirectToAction("Index", "Properties");
                }
                TempData["Error"] = "تعذر تحميل تفاصيل العقار.";
                return RedirectToAction("Index", "Properties");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("set-status/{id:guid}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromForm] UpdatePropertyStatusCommand command, CancellationToken ct)
        {
            command.Id = id;
            try
            {
                await _properties.UpdateStatusAsync(command, ct);
                TempData["Success"] = "تم تحديث حالة العقار بنجاح.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر تحديث حالة العقار.";
            }
            return RedirectToAction(nameof(Index), new { id });
        }

        private static List<PropertyStatusOptionViewModel> BuildStatusOptions(PropertyStatus current)
        {
            var options = new (PropertyStatus Status, string Label, string Tone)[]
            {
                (PropertyStatus.Available, "متاح", "success"),
                (PropertyStatus.Reserved, "محجوز", "warning"),
                (PropertyStatus.Sold, "مباع", "danger"),
                (PropertyStatus.Rented, "مؤجر", "info"),
                (PropertyStatus.Hidden, "مخفي", "neutral")
            };

            return options.Select(o => new PropertyStatusOptionViewModel
            {
                Status = o.Status,
                Label = o.Label,
                Tone = o.Tone,
                IsCurrent = o.Status == current
            }).ToList();
        }
    }
}
