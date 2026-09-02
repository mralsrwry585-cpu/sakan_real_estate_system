namespace MyRealEstate.Web.Models.Lessor
{
    public class DashboardViewModel
    {
        public string WelcomeMessage { get; set; } = "مرحباً، أحمد 👋";
        public List<StatCardViewModel> Stats { get; set; } = new();
        public List<PropertyStatusCardViewModel> StatusBreakdown { get; set; } = new();
        public List<MonthlyTrendViewModel> MonthlyTrends { get; set; } = new();
        public List<RequestRowViewModel> RecentRequests { get; set; } = new();
    }

    public class StatCardViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Delta { get; set; } = string.Empty;
        public string Tone { get; set; } = "primary";
        public string Icon { get; set; } = "build";
    }

    public class PropertyStatusCardViewModel
    {
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Percentage { get; set; }
        public string Color { get; set; } = "#4f46e5";
    }

    public class MonthlyTrendViewModel
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    public class RequestRowViewModel
    {
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Tone { get; set; } = "warning";
    }
}
