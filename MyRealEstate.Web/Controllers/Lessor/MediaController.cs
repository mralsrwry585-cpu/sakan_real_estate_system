using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Models.Lessor;
using MyRealEstate.Web.Services;

namespace MyRealEstate.Web.Controllers.Lessor
{
    [Authorize]
    [Route("owner/properties/{propertyId:guid}/media")]
    public class MediaController : Controller
    {
        private readonly PropertiesApiClient _properties;

        public MediaController(PropertiesApiClient properties)
        {
            _properties = properties;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid propertyId, CancellationToken ct)
        {
            var ownerId = HttpContext.Session.GetOwnerId();
            if (ownerId is null)
                return RedirectToAction("Login", "Account");

            var vm = new MediaViewModel { PropertyId = propertyId };

            try
            {
                var property = await _properties.GetByIdAsync(propertyId, ct);
                if (property is not null)
                {
                    vm.IsOwner = property.OwnerId == ownerId.Value;
                    vm.PropertyTitle = property.Title;
                }

                var media = await _properties.GetMediaAsync(propertyId, ct);
                if (media is not null)
                {
                    vm.Items = media
                        .OrderBy(m => m.DisplayOrder)
                        .Select(m => new MediaItemViewModel
                        {
                            Id = m.Id,
                            Url = m.Url,
                            Type = m.MediaType,
                            IsCover = m.IsCover,
                            DisplayOrder = m.DisplayOrder
                        })
                        .ToList();
                }
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر تحميل وسائط العقار.";
                return RedirectToAction("Index", "Properties");
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add")]
        public async Task<IActionResult> Add(Guid propertyId, [FromForm] AddPropertyMediaCommand command, CancellationToken ct, [FromQuery] string? returnUrl = null)
        {
            command.PropertyId = propertyId;
            command.MediaType = MediaType.Image;
            try
            {
                await _properties.AddMediaAsync(command, ct);
                TempData["Success"] = "تمت إضافة الوسيط بنجاح.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر إضافة الوسيط. تأكد من الرابط.";
            }
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
                
            return RedirectToAction(nameof(Index), new { propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete/{mediaId:guid}")]
        public async Task<IActionResult> Delete(Guid propertyId, Guid mediaId, CancellationToken ct, [FromQuery] string? returnUrl = null)
        {
            try
            {
                await _properties.DeleteMediaAsync(propertyId, mediaId, ct);
                TempData["Success"] = "تم حذف الوسيط بنجاح.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر حذف الوسيط.";
            }
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
                
            return RedirectToAction(nameof(Index), new { propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("cover")]
        public async Task<IActionResult> SetCover(Guid propertyId, Guid mediaId, CancellationToken ct, [FromQuery] string? returnUrl = null)
        {
            try
            {
                await _properties.SetCoverAsync(propertyId, mediaId, ct);
                TempData["Success"] = "تم تعيين صورة الغلاف بنجاح.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر تعيين الغلاف.";
            }
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
                
            return RedirectToAction(nameof(Index), new { propertyId });
        }

[HttpPost]
        [Route("reorder")]
        public async Task<IActionResult> Reorder(Guid propertyId, [FromBody] ReorderMediaCommand command, CancellationToken ct)
        {
            command.PropertyId = propertyId;
            try
            {
                await _properties.ReorderMediaAsync(command, ct);
                return Json(new { ok = true });
            }
            catch (ApiClientException)
            {
                return Json(new { ok = false, message = "تعذر إعادة الترتيب." });
            }
        }
    }
}
