namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a booking associated with a tenant and its related requirements.
/// </summary>
/// <param name="TenantId">The unique identifier of the tenant associated with the booking.</param>
/// <param name="CompleteProfileRequired">A value indicating whether the tenant is required to complete their profile before proceeding with the booking. 
/// <see langword="true"/> if profile completion is required; otherwise, <see langword="false"/>.</param>
/// <param name="BookingTimeFrame">The time frame, in days, within which the booking is valid or must be completed.</param>
internal sealed record TenantsHostBooking(
	int  TenantId,
	bool CompleteProfileRequired,
	int  BookingTimeFrame
);
