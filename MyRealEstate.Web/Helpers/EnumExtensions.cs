using System.Globalization;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Helpers
{
    /// <summary>
    /// Arabic display names + formatting helpers for domain enums and values.
    /// Mirrors the Lovable UI labels (متاح/محجوز/مؤجر/مسودة/...).
    /// </summary>
    public static class EnumExtensions
    {
        public static string ToArabic(this PropertyType type) => type switch
        {
            PropertyType.Land => "أرض",
            PropertyType.Apartment => "شقة",
            PropertyType.IndependentHouse => "منزل",
            PropertyType.Villa => "فيلا",
            PropertyType.Commercial => "تجاري",
            PropertyType.Office => "مكتب",
            PropertyType.Studio => "استوديو",
            PropertyType.Duplex => "دوبلكس",
            _ => type.ToString()
        };

        public static string ToArabic(this ContractType contract) => contract switch
        {
            ContractType.Sale => "بيع",
            ContractType.Rent => "إيجار",
            ContractType.DailyRent => "إيجار يومي",
            _ => contract.ToString()
        };

        public static string ToArabic(this PropertyStatus status) => status switch
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

        public static string ToArabic(this BookingStatus status) => status switch
        {
            BookingStatus.Pending => "قيد المراجعة",
            BookingStatus.Approved => "مقبول",
            BookingStatus.Rejected => "مرفوض",
            BookingStatus.Cancelled => "ملغى",
            BookingStatus.Completed => "مكتمل",
            _ => status.ToString()
        };

        public static string ToArabic(this ViewingStatus status) => status switch
        {
            ViewingStatus.Pending => "قيد المراجعة",
            ViewingStatus.Approved => "مقبول",
            ViewingStatus.Rejected => "مرفوض",
            ViewingStatus.Completed => "مكتمل",
            ViewingStatus.Cancelled => "ملغى",
            _ => status.ToString()
        };

        public static string ToArabic(this AmenityCategory category) => category switch
        {
            AmenityCategory.Interior => "داخلي",
            AmenityCategory.Exterior => "خارجي",
            AmenityCategory.Services => "خدمات",
            AmenityCategory.Security => "أمن",
            AmenityCategory.Technology => "تكنولوجيا",
            _ => category.ToString()
        };

        /// <summary>
        /// Status badge tone (primary/gold/success/info/warning/danger/neutral)
        /// matching the Lovable Badge component tones.
        /// </summary>
        public static string PropertyTone(this PropertyStatus status) => status switch
        {
            PropertyStatus.Available => "success",
            PropertyStatus.Reserved => "warning",
            PropertyStatus.Rented => "info",
            PropertyStatus.Sold => "danger",
            PropertyStatus.Draft => "neutral",
            PropertyStatus.PendingApproval => "warning",
            PropertyStatus.Hidden => "neutral",
            _ => "neutral"
        };

        public static string RequestTone(this BookingStatus status) => status switch
        {
            BookingStatus.Pending => "warning",
            BookingStatus.Approved => "success",
            BookingStatus.Rejected => "danger",
            BookingStatus.Cancelled => "neutral",
            BookingStatus.Completed => "info",
            _ => "neutral"
        };

        public static string RequestTone(this ViewingStatus status) => status switch
        {
            ViewingStatus.Pending => "warning",
            ViewingStatus.Approved => "success",
            ViewingStatus.Rejected => "danger",
            ViewingStatus.Completed => "info",
            ViewingStatus.Cancelled => "neutral",
            _ => "neutral"
        };
    }

    /// <summary>
    /// Currency / number formatting that matches the Lovable Arabic (ar-SA) presentation.
    /// </summary>
    public static class FormatExtensions
    {
        /// <summary>Format a decimal as a Western-style grouped number (e.g. 2,450,000).</summary>
        public static string FormatPrice(this decimal value)
        {
            return value.ToString("#,##0", CultureInfo.InvariantCulture);
        }

        /// <summary>Format an int with thousands separators (e.g. 8,432).</summary>
        public static string FormatNumber(this int value)
        {
            return value.ToString("#,##0", CultureInfo.InvariantCulture);
        }

        /// <summary>Format a date as dd/MM/yyyy (Western digits, LTR).</summary>
        public static string FormatDate(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>Format a date as dd / MM (short, e.g. 14 / 07).</summary>
        public static string FormatShortDate(this DateTime value)
        {
            return value.ToString("dd / MM", CultureInfo.InvariantCulture);
        }
    }
}
