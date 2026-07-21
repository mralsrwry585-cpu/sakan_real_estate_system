using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Models.Lessor
{
    public class PropertyListViewModel
    {
        public string Title { get; set; } = "عقاراتي";
        public List<PropertyRowViewModel> Items { get; set; } = new();
        public List<string> StatusFilters { get; set; } = new();

        /// <summary>Structured, clickable status filter pills (label + status + count + active).</summary>
        public List<PropertyFilterViewModel> Filters { get; set; } = new();

        public int TotalCount { get; set; }
        public bool ShowGrid { get; set; }
        public string? SearchTerm { get; set; }
        public PropertyStatus? ActiveStatus { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public int TotalPages { get; set; } = 1;
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        /// <summary>Sorting (DataTables column sort convention: name|asc|desc).</summary>
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }

    public class PropertyFilterViewModel
    {
        public PropertyStatus? Status { get; set; }
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; } = string.Empty;
    }

    public class PropertyRowViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Contract { get; set; } = string.Empty;
        public string ContractTone { get; set; } = "info";
        public string Price { get; set; } = string.Empty;
        public int Views { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Tone { get; set; } = "success";
        public string? ImageUrl { get; set; }
    }
}
