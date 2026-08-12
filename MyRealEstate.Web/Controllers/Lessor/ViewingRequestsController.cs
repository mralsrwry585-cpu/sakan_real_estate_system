using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Models.Lessor;
using MyRealEstate.Web.Services;

namespace MyRealEstate.Web.Controllers.Lessor
{
    [Authorize]
    [Route("owner/requests/viewings")]
    public class ViewingRequestsController : Controller
    {
        private readonly ViewingRequestsApiClient _client;

        public ViewingRequestsController(ViewingRequestsApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewingStatus? status = null, CancellationToken ct = default)
        {
            var ownerId = HttpContext.Session.GetOwnerId();
            if (ownerId is null)
                return RedirectToAction("Login", "Account");

            var vm = new ViewingRequestsViewModel { ActiveFilter = status };

            // Stats
            vm.Stats = new List<StatCardViewModel>
            {
                new() { Title = "طلبات جديدة", Value = "…", Tone = "gold", Icon = "bell" },
                new() { Title = "مؤكدة اليوم", Value = "…", Tone = "success", Icon = "check" },
                new() { Title = "هذا الأسبوع", Value = "…", Tone = "primary", Icon = "cal" },
                new() { Title = "مكتملة", Value = "…", Tone = "info", Icon = "eye" }
            };

            try
            {
                // Fetch first page (up to 100) to compute stats locally
                var all = await _client.GetAllAsync(new GetAllViewingRequestsQuery
                {
                    OwnerId = ownerId.Value,
                    Page = 1,
                    PageSize = 100
                }, ct);

                if (all?.Items is not null)
                {
                    var items = all.Items;
                    var today = DateTime.Today;
                    var weekStart = today.AddDays(-7);

                    int pending = items.Count(i => i.Status == ViewingStatus.Pending);
                    int todayCount = items.Count(i => i.RequestedDate.Date == today && i.Status == ViewingStatus.Approved);
                    int thisWeek = items.Count(i => i.RequestedDate >= weekStart && i.Status == ViewingStatus.Approved);
                    int completed = items.Count(i => i.Status == ViewingStatus.Completed);
                    int total = all.TotalCount;

                    vm.Stats = new List<StatCardViewModel>
                    {
                        new() { Title = "طلبات جديدة", Value = pending.FormatNumber(), Tone = "gold", Icon = "bell" },
                        new() { Title = "مؤكدة اليوم", Value = todayCount.FormatNumber(), Tone = "success", Icon = "check" },
                        new() { Title = "هذا الأسبوع", Value = thisWeek.FormatNumber(), Tone = "primary", Icon = "cal" },
                        new() { Title = "مكتملة", Value = completed.FormatNumber(), Tone = "info", Icon = "eye" }
                    };

                    // Filters
var filtered = status is null ? items : items.Where(i => i.Status == status.Value).ToList();
                    vm.Items = filtered
                        .OrderByDescending(i => i.CreatedAt)
                        .Select(i => new ViewingRequestRowViewModel
                        {
                            Id = i.Id,
                            Number = $"VW-{i.Id.ToString("N")[..5].ToUpperInvariant()}",
                            PropertyTitle = i.PropertyTitle,
                            TenantName = i.TenantName,
                            RequestedDate = i.RequestedDate,
                            RequestedTime = i.RequestedTime,
                            Note = i.Note,
                            OwnerResponseNote = i.OwnerResponseNote,
                            Status = i.Status
                        })
                        .ToList();
                    vm.TotalCount = vm.Items.Count;

                    vm.Filters = new List<RequestFilterViewModel>
                    {
                        new() { Label = "الكل", IsActive = status is null, Count = total },
                        new() { Label = "قيد المراجعة", IsActive = status == ViewingStatus.Pending, Count = pending },
                        new() { Label = "مقبول", IsActive = status == ViewingStatus.Approved, Count = items.Count(i => i.Status == ViewingStatus.Approved) },
                        new() { Label = "مرفوض", IsActive = status == ViewingStatus.Rejected, Count = items.Count(i => i.Status == ViewingStatus.Rejected) },
                        new() { Label = "مكتمل", IsActive = status == ViewingStatus.Completed, Count = completed },
                        new() { Label = "ملغى", IsActive = status == ViewingStatus.Cancelled, Count = items.Count(i => i.Status == ViewingStatus.Cancelled) }
                    };
                }
            }
            catch (ApiClientException)
            {
                ModelState.AddModelError(string.Empty, "تعذر تحميل طلبات المعاينة.");
                vm.Filters = new List<RequestFilterViewModel> { new() { Label = "الكل", IsActive = true } };
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("accept/{id:guid}")]
        public async Task<IActionResult> Accept(Guid id, string? requestedTime, string? ownerResponseNote, CancellationToken ct)
        {
            var command = new UpdateViewingRequestStatusCommand
            {
                Id = id,
                Status = ViewingStatus.Approved,
                OwnerResponseNote = string.IsNullOrWhiteSpace(ownerResponseNote) ? null : ownerResponseNote
            };

            try
            {
                await _client.UpdateStatusAsync(command, ct);
                TempData["Success"] = "تم تأكيد موعد المعاينة بنجاح.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر تأكيد المعاينة.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("reject/{id:guid}")]
        public async Task<IActionResult> Reject(Guid id, string? ownerResponseNote, CancellationToken ct)
        {
            var command = new UpdateViewingRequestStatusCommand
            {
                Id = id,
                Status = ViewingStatus.Rejected,
                OwnerResponseNote = string.IsNullOrWhiteSpace(ownerResponseNote) ? null : ownerResponseNote
            };

            try
            {
                await _client.UpdateStatusAsync(command, ct);
                TempData["Success"] = "تم رفض طلب المعاينة.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر رفض الطلب.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
