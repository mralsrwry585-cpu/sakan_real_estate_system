using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Models.Lessor;
using MyRealEstate.Web.Services;

namespace MyRealEstate.Web.Controllers.Lessor
{
    [Authorize]
    [Route("owner")]
    public class DashboardController : Controller
    {
        private readonly AnalyticsApiClient _analytics;
        private readonly ViewingRequestsApiClient _viewings;
        private readonly BookingRequestsApiClient _bookings;

        public DashboardController(
            AnalyticsApiClient analytics,
            ViewingRequestsApiClient viewings,
            BookingRequestsApiClient bookings)
        {
            _analytics = analytics;
            _viewings = viewings;
            _bookings = bookings;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var ownerId = HttpContext.Session.GetOwnerId();
            if (ownerId is null)
            {
                TempData["Error"] = "تعذر تحديد هوية المالك. سجل الدخول مرة أخرى.";
                return RedirectToAction("Login", "Account");
            }

            var vm = new DashboardViewModel
            {
                WelcomeMessage = $"مرحباً، {HttpContext.Session.GetOwnerName() ?? "المالك"} 👋"
            };

            try
            {
                var stats = await _analytics.GetOwnerDashboardStatsAsync(ownerId.Value, ct);
                if (stats is not null)
                {
                    vm.Stats = new List<StatCardViewModel>
                    {
                        new() { Title = "إجمالي العقارات", Value = stats.TotalProperties.FormatNumber(), Delta = $"+{stats.NewThisMonth} هذا الشهر", Tone = "primary", Icon = "build" },
                        new() { Title = "طلبات المعاينة", Value = stats.TotalViewingRequests.FormatNumber(), Delta = $"{stats.PendingViewingRequests} جديدة", Tone = "gold", Icon = "eye" },
                        new() { Title = "طلبات الحجز", Value = stats.TotalBookingRequests.FormatNumber(), Delta = $"{stats.PendingBookingRequests} قيد المراجعة", Tone = "success", Icon = "cal" },
                        new() { Title = "إجمالي المشاهدات", Value = stats.TotalViews.FormatNumber(), Delta = $"+{stats.ViewsThisMonth} هذا الشهر", Tone = "info", Icon = "eye" }
                    };

                    vm.StatusBreakdown = stats.StatusBreakdown
                        .Select(s => new PropertyStatusCardViewModel
                        {
                            Label = s.Status.ToArabic(),
                            Count = s.Count,
                            Percentage = stats.TotalProperties == 0 ? 0 : (int)Math.Round(s.Count * 100.0 / stats.TotalProperties),
                            Color = StatusColor(s.Status)
                        }).ToList();

                    vm.MonthlyTrends = stats.MonthlyStats
                        .OrderBy(m => m.Year).ThenBy(m => m.Month)
                        .Select(m => new MonthlyTrendViewModel
                        {
                            Label = ArabicMonth(m.Month),
                            Value = m.Views
                        }).ToList();
                }
            }
            catch (ApiClientException)
            {
                ModelState.AddModelError(string.Empty, "تعذر تحميل إحصائيات لوحة التحكم.");
            }

            try
            {
                var viewings = await _viewings.GetAllAsync(new GetAllViewingRequestsQuery
                {
                    OwnerId = ownerId.Value,
                    Page = 1,
                    PageSize = 5
                }, ct);
                var bookings = await _bookings.GetAllAsync(new GetAllBookingRequestsQuery
                {
                    OwnerId = ownerId.Value,
                    Page = 1,
                    PageSize = 5
                }, ct);

                var recent = new List<RequestRowViewModel>();
                if (viewings?.Items is not null)
                {
                    recent.AddRange(viewings.Items.Select(v => new RequestRowViewModel
                    {
                        Number = $"VW-{v.Id.ToString("N")[..5].ToUpperInvariant()}",
                        Type = "معاينة",
                        PropertyName = v.PropertyTitle,
                        UserName = v.TenantName,
                        Date = v.CreatedAt.FormatShortDate(),
                        Status = v.Status.ToArabic(),
                        Tone = v.Status.RequestTone()
                    }));
                }
                if (bookings?.Items is not null)
                {
                    recent.AddRange(bookings.Items.Select(b => new RequestRowViewModel
                    {
                        Number = $"BK-{b.Id.ToString("N")[..5].ToUpperInvariant()}",
                        Type = "حجز",
                        PropertyName = b.PropertyTitle,
                        UserName = b.TenantName,
                        Date = b.CreatedAt.FormatShortDate(),
                        Status = b.Status.ToArabic(),
                        Tone = b.Status.RequestTone()
                    }));
                }

                vm.RecentRequests = recent
                    .OrderByDescending(r => r.Date)
                    .Take(5)
                    .ToList();
            }
            catch (ApiClientException)
            {
                // Non-fatal: recent requests section can be empty
            }

            // Fallback to empty state if no stats loaded
            if (vm.Stats.Count == 0)
            {
                vm.Stats = new List<StatCardViewModel>
                {
                    new() { Title = "إجمالي العقارات", Value = "0", Delta = "لا توجد بيانات بعد", Tone = "primary", Icon = "build" },
                    new() { Title = "طلبات المعاينة", Value = "0", Delta = "لا توجد بيانات بعد", Tone = "gold", Icon = "eye" },
                    new() { Title = "طلبات الحجز", Value = "0", Delta = "لا توجد بيانات بعد", Tone = "success", Icon = "cal" },
                    new() { Title = "إجمالي المشاهدات", Value = "0", Delta = "لا توجد بيانات بعد", Tone = "info", Icon = "eye" }
                };
            }

            return View("~/Views/Lessor/Dashboard/Index.cshtml", vm);
        }

        private static string StatusColor(PropertyStatus status) => status switch
        {
            PropertyStatus.Available => "oklch(0.62 0.14 155)",
            PropertyStatus.Reserved => "oklch(0.78 0.15 75)",
            PropertyStatus.Rented => "oklch(0.62 0.12 240)",
            PropertyStatus.Sold => "oklch(0.55 0.20 25)",
            PropertyStatus.PendingApproval => "oklch(0.75 0.12 75)",
            PropertyStatus.Draft => "oklch(0.55 0.02 200)",
            _ => "oklch(0.55 0.02 200)"
        };

        private static string ArabicMonth(int month) => month switch
        {
            1 => "يناير", 2 => "فبراير", 3 => "مارس", 4 => "أبريل",
            5 => "مايو", 6 => "يونيو", 7 => "يوليو", 8 => "أغسطس",
            9 => "سبتمبر", 10 => "أكتوبر", 11 => "نوفمبر", 12 => "ديسمبر",
            _ => month.ToString()
        };
    }
}
