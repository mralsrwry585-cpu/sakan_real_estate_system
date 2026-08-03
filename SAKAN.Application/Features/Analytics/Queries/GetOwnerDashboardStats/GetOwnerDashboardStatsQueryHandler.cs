using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Analytics.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Analytics.Queries.GetOwnerDashboardStats
{
    public class GetOwnerDashboardStatsQueryHandler : IRequestHandler<GetOwnerDashboardStatsQuery, OwnerDashboardStatsDto>
    {
        private readonly IApplicationDbContext _context;

        public GetOwnerDashboardStatsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OwnerDashboardStatsDto> Handle(GetOwnerDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            // Owner's properties
            var propertiesQuery = _context.Properties.Where(p => p.OwnerId == request.OwnerId);
            var properties = await propertiesQuery.ToListAsync(cancellationToken);

            var propertyIds = properties.Select(p => p.Id).ToList();

            // Owner's requests
            var viewingRequests = await _context.ViewingRequests
                .Where(vr => vr.OwnerId == request.OwnerId)
                .ToListAsync(cancellationToken);

            var bookingRequests = await _context.BookingRequests
                .Include(br => br.Property)
                .Where(br => br.OwnerId == request.OwnerId)
                .ToListAsync(cancellationToken);

            // Media count
            var mediaCount = await _context.PropertyMedia
                .Where(pm => propertyIds.Contains(pm.PropertyId))
                .CountAsync(cancellationToken);

            var activeStatuses = new[] { PropertyStatus.Available, PropertyStatus.Reserved, PropertyStatus.Rented };

            var statusBreakdown = properties
                .GroupBy(p => p.Status)
                .Select(g => new PropertyStatusBreakdown
                {
                    Status = g.Key,
                    StatusName = GetStatusName(g.Key),
                    Count = g.Count()
                })
                .ToList();

            // Monthly stats for the last 6 months
            var monthlyStats = new List<MonthlyStats>();
            for (var i = 5; i >= 0; i--)
            {
                var month = monthStart.AddMonths(-i);
                var nextMonth = month.AddMonths(1);

                monthlyStats.Add(new MonthlyStats
                {
                    Year = month.Year,
                    Month = month.Month,
                    PropertiesAdded = properties.Count(p => p.CreatedAt >= month && p.CreatedAt < nextMonth),
                    Views = properties.Where(p => p.CreatedAt >= month && p.CreatedAt < nextMonth).Sum(p => p.Views),
                    Bookings = bookingRequests.Count(br => br.CreatedAt >= month && br.CreatedAt < nextMonth)
                });
            }

            return new OwnerDashboardStatsDto
            {
                TotalProperties = properties.Count,
                ActiveProperties = properties.Count(p => activeStatuses.Contains(p.Status)),
                PendingProperties = properties.Count(p => p.Status == PropertyStatus.PendingApproval),
                ReservedProperties = properties.Count(p => p.Status == PropertyStatus.Reserved),
                RentedProperties = properties.Count(p => p.Status == PropertyStatus.Rented),
                TotalViews = properties.Sum(p => p.Views),
                TotalViewingRequests = viewingRequests.Count,
                PendingViewingRequests = viewingRequests.Count(vr => vr.Status == ViewingStatus.Pending),
                TotalBookingRequests = bookingRequests.Count,
                PendingBookingRequests = bookingRequests.Count(br => br.Status == BookingStatus.Pending),
                ConfirmedBookings = bookingRequests.Count(br => br.Status == BookingStatus.Approved),
                TotalMediaItems = mediaCount,
                AveragePrice = properties.Count > 0 ? properties.Average(p => p.Price) : 0,
                TotalRevenue = bookingRequests
                    .Where(br => br.Status == BookingStatus.Approved && br.Property != null)
                    .Sum(br => br.Property.Price),
                NewThisMonth = properties.Count(p => p.CreatedAt >= monthStart),
                ViewsThisMonth = properties.Sum(p => p.Views),
                StatusBreakdown = statusBreakdown,
                MonthlyStats = monthlyStats
            };
        }

        private static string GetStatusName(PropertyStatus status)
        {
            return status switch
            {
                PropertyStatus.Draft => "مسودة",
                PropertyStatus.PendingApproval => "قيد المراجعة",
                PropertyStatus.Available => "متاح",
                PropertyStatus.Reserved => "محجوز",
                PropertyStatus.Sold => "مباع",
                PropertyStatus.Rented => "مؤجر",
                PropertyStatus.Hidden => "مخفي",
                _ => status.ToString()
            };
        }
    }
}
