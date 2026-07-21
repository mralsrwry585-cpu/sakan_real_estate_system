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
    public class PropertiesController : Controller
    {
        private readonly PropertiesApiClient _properties;

        public PropertiesController(PropertiesApiClient properties)
        {
            _properties = properties;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] PropertyStatus? status = null,
            [FromQuery] string? q = null,
            [FromQuery] int page = 1,
            [FromQuery] bool grid = false,
            CancellationToken ct = default)
        {
            var ownerId = HttpContext.Session.GetOwnerId();
            if (ownerId is null)
                return RedirectToAction("Login", "Account");

            var vm = new PropertyListViewModel
            {
                ShowGrid = grid,
                ActiveStatus = status,
                SearchTerm = q,
                PageNumber = page,
                PageSize = 6,
                Items = new List<PropertyRowViewModel>(),
                StatusFilters = new List<string>()
            };

            try
            {
                var result = await _properties.GetAllAsync(new GetAllPropertiesQuery
                {
                    OwnerId = ownerId.Value,
                    PageNumber = page,
                    PageSize = 6,
                    Status = status,
                    SearchTerm = string.IsNullOrWhiteSpace(q) ? null : q
                }, ct);

                if (result is not null)
                {
                    vm.TotalCount = result.TotalCount;
                    vm.TotalPages = result.TotalPages;
                    vm.HasNextPage = result.HasNextPage;
                    vm.HasPreviousPage = result.HasPreviousPage;

                    vm.Items = result.Items.Select(p => new PropertyRowViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Type = p.PropertyType.ToArabic(),
                        Contract = p.ContractType.ToArabic(),
                        ContractTone = ContractTone(p.ContractType),
                        Price = p.ContractType == ContractType.Rent
                            ? $"{p.Price.FormatPrice()} / شهر"
                            : p.Price.FormatPrice(),
                        Views = p.Views,
                        Status = p.Status.ToArabic(),
                        Tone = p.Status.PropertyTone(),
                        ImageUrl = p.CoverImageUrl
                    }).ToList();
                }
            }
            catch (ApiClientException)
            {
                ModelState.AddModelError(string.Empty, "تعذر تحميل العقارات. حاول مرة أخرى.");
            }

// Filter pills with real counts (from the fetched page hit counts are approximate)
            BuildFilterPills(vm, status, q, ownerId.Value, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            try
            {
                await _properties.DeleteAsync(id, ct);
                TempData["Success"] = "تم حذف العقار بنجاح.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر حذف العقار. حاول مرة أخرى.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id:guid}/status")]
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
            return RedirectToAction("Index", "PropertyDetails", new { id });
        }

private void BuildFilterPills(PropertyListViewModel vm, PropertyStatus? active, string? q, Guid ownerId, CancellationToken ct)
        {
            var counts = new Dictionary<PropertyStatus, int>();
            try
            {
                var all = _properties.GetAllAsync(new GetAllPropertiesQuery
                {
                    OwnerId = ownerId,
                    PageNumber = 1,
                    PageSize = 1000
                }, ct).ConfigureAwait(false).GetAwaiter().GetResult();

                if (all?.Items is not null)
                {
                    foreach (var p in all.Items)
                    {
                        counts[p.Status] = counts.TryGetValue(p.Status, out var c) ? c + 1 : 1;
                    }
                }
            }
            catch
            {
                // fall back to no count data
            }

vm.StatusFilters = new List<string>
            {
                $"الكل ({vm.TotalCount})"
            };
            var list = new List<(PropertyStatus Status, string Label, int Count)>
            {
                (PropertyStatus.Available, "متاح", counts.GetValueOrDefault(PropertyStatus.Available)),
                (PropertyStatus.Reserved, "محجوز", counts.GetValueOrDefault(PropertyStatus.Reserved)),
                (PropertyStatus.Rented, "مؤجر", counts.GetValueOrDefault(PropertyStatus.Rented)),
                (PropertyStatus.Draft, "مسودة", counts.GetValueOrDefault(PropertyStatus.Draft)),
                (PropertyStatus.PendingApproval, "قيد المراجعة", counts.GetValueOrDefault(PropertyStatus.PendingApproval)),
                (PropertyStatus.Hidden, "مخفي", counts.GetValueOrDefault(PropertyStatus.Hidden)),
                (PropertyStatus.Sold, "مباع", counts.GetValueOrDefault(PropertyStatus.Sold))
            };
            vm.StatusFilters.AddRange(list.Where(x => x.Count > 0).Select(x => $"{x.Label} ({x.Count})"));

            // Structured, clickable filter pills
            var baseUrl = Url.Action(nameof(Index), "Properties") ?? "/owner/properties";
            var allLink = baseUrl + (string.IsNullOrWhiteSpace(q) ? "" : $"?q={Uri.EscapeDataString(q)}");
            vm.Filters = new List<PropertyFilterViewModel>
            {
                new() { Status = null, Label = "الكل", Count = vm.TotalCount, IsActive = active is null, Link = allLink }
            };
            vm.Filters.AddRange(list
                .Where(x => x.Count > 0)
                .Select(x =>
                {
                    var link = $"{baseUrl}?status={(int)x.Status}" +
                               (string.IsNullOrWhiteSpace(q) ? "" : $"&q={Uri.EscapeDataString(q)}");
                    return new PropertyFilterViewModel
                    {
                        Status = x.Status,
                        Label = x.Label,
                        Count = x.Count,
                        IsActive = active == x.Status,
                        Link = link
                    };
                }));
        }

        private static string ContractTone(ContractType contract) => contract switch
        {
            ContractType.Sale => "gold",
            ContractType.Rent => "info",
            ContractType.DailyRent => "primary",
            _ => "info"
        };
    }
}
