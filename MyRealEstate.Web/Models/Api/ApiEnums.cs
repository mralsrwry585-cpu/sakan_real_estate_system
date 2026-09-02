namespace MyRealEstate.Web.Models.Api
{
    /// <summary>Mirror of SAKAN.Domain.Enums.Role — order must match backend.</summary>
    public enum Role
    {
        Tenant,
        Owner
    }

    /// <summary>Mirror of SAKAN.Domain.Enums.PropertyType — order must match backend.</summary>
    public enum PropertyType
    {
        Land,
        Apartment,
        IndependentHouse,
        Villa,
        Commercial,
        Office,
        Studio,
        Duplex
    }

    /// <summary>Mirror of SAKAN.Domain.Enums.ContractType — order must match backend.</summary>
    public enum ContractType
    {
        Sale,
        Rent,
        DailyRent
    }

    /// <summary>Mirror of SAKAN.Domain.Enums.PropertyStatus — order must match backend.</summary>
    public enum PropertyStatus
    {
        Draft,
        PendingApproval,
        Available,
        Reserved,
        Sold,
        Rented,
        Hidden
    }

    /// <summary>Mirror of SAKAN.Domain.Enums.BookingStatus — order must match backend.</summary>
    public enum BookingStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled,
        Completed
    }

    /// <summary>Mirror of SAKAN.Domain.Enums.ViewingStatus — order must match backend.</summary>
    public enum ViewingStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed,
        Cancelled
    }

    /// <summary>Mirror of SAKAN.Domain.Enums.AmenityCategory — order must match backend.</summary>
    public enum AmenityCategory
    {
        Interior,
        Exterior,
        Services,
        Security,
        Technology
    }

    /// <summary>Mirror of SAKAN.Domain.Enums.MediaType — order must match backend.</summary>
    public enum MediaType
    {
        Image
    }
}
