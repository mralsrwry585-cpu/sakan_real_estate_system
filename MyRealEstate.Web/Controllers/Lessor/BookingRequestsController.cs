using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Models.Lessor;
using MyRealEstate.Web.Services;

namespace MyRealEstate.Web.Controllers.Lessor
{
    [Authorize]
    [Route("owner/requests/bookings")]
    public class BookingRequestsController : Controller
    {
        private readonly BookingRequestsApiClient _client;

        public BookingRequestsController(BookingRequestsApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index(BookingStatus? status = null, CancellationToken ct = default)
        {
            var ownerId = HttpContext.Session.GetOwnerId();
            if (ownerId is null)
                return RedirectToAction("Login", "Account");

            var vm = new BookingRequestsViewModel { ActiveFilter = status };

            try
            {
                var all = await _client.GetAllAsync(new GetAllBookingRequestsQuery
                {
                    OwnerId = ownerId.Value,
                    Page = 1,
                    PageSize = 100
                }, ct);

                if (all?.Items is not null)
                {
                    var items = all.Items;

                    int pending = items.Count(i => i.Status == BookingStatus.Pending);
                    int approved = items.Count(i => i.Status == BookingStatus.Approved);
                    int rejected = items.Count(i => i.Status == BookingStatus.Rejected);
                    int completed = items.Count(i => i.Status == BookingStatus.Completed);
                    int total = all.TotalCount;

                    vm.Stats = new List<StatCardViewModel>
                    {
                        new() { Title = "جديد", Value = pending.FormatNumber(), Tone = "gold", Icon = "bell" },
                        new() { Title = "مقبول", Value = approved.FormatNumber(), Tone = "success", Icon = "check" },
                        new() { Title = "مرفوض", Value = rejected.FormatNumber(), Tone = "primary", Icon = "x" },
                        new() { Title = "مكتمل", Value = completed.FormatNumber(), Tone = "info", Icon = "cal" }
                    };

                    var filtered = status is null ? items : items.Where(i => i.Status == status.Value).ToList();
                    vm.Items = filtered
                        .OrderByDescending(i => i.CreatedAt)
                        .Select(i => new BookingRequestRowViewModel
                        {
                            Id = i.Id,
                            Number = string.IsNullOrWhiteSpace(i.BookingNumber)
                                ? $"BK-{i.Id.ToString("N")[..5].ToUpperInvariant()}"
                                : i.BookingNumber,
                            PropertyId = i.PropertyId,
                            PropertyTitle = i.PropertyTitle,
                            TenantName = i.TenantName,
                            StartDate = i.StartDate,
                            DurationMonths = i.DurationMonths,
                            Note = i.Note,
                            OwnerResponseNote = i.OwnerResponseNote,
                            Status = i.Status
                        })
                        .ToList();
                    vm.TotalCount = vm.Items.Count;

                    vm.Filters = new List<RequestFilterViewModel>
                    {
                        new() { Label = "الكل", IsActive = status is null, Count = total },
                        new() { Label = "قيد المراجعة", IsActive = status == BookingStatus.Pending, Count = pending },
                        new() { Label = "مقبول", IsActive = status == BookingStatus.Approved, Count = approved },
                        new() { Label = "مرفوض", IsActive = status == BookingStatus.Rejected, Count = rejected },
                        new() { Label = "مكتمل", IsActive = status == BookingStatus.Completed, Count = completed },
                        new() { Label = "ملغى", IsActive = status == BookingStatus.Cancelled, Count = items.Count(i => i.Status == BookingStatus.Cancelled) }
                    };
                }
            }
            catch (ApiClientException)
            {
                ModelState.AddModelError(string.Empty, "تعذر تحميل طلبات الحجز.");
                vm.Filters = new List<RequestFilterViewModel> { new() { Label = "الكل", IsActive = true } };
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("approve/{id:guid}")]
        public async Task<IActionResult> Approve(Guid id, string? ownerResponseNote, CancellationToken ct)
        {
            var command = new UpdateBookingRequestStatusCommand
            {
                Id = id,
                Status = BookingStatus.Approved,
                OwnerResponseNote = string.IsNullOrWhiteSpace(ownerResponseNote) ? null : ownerResponseNote
            };

            try
            {
                await _client.UpdateStatusAsync(command, ct);
                TempData["Success"] = "تم قبول طلب الحجز بنجاح.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر قبول طلب الحجز.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("reject/{id:guid}")]
        public async Task<IActionResult> Reject(Guid id, string? ownerResponseNote, CancellationToken ct)
        {
            var command = new UpdateBookingRequestStatusCommand
            {
                Id = id,
                Status = BookingStatus.Rejected,
                OwnerResponseNote = string.IsNullOrWhiteSpace(ownerResponseNote) ? null : ownerResponseNote
            };

            try
            {
                await _client.UpdateStatusAsync(command, ct);
                TempData["Success"] = "تم رفض طلب الحجز.";
            }
            catch (ApiClientException)
            {
                TempData["Error"] = "تعذر رفض طلب الحجز.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
