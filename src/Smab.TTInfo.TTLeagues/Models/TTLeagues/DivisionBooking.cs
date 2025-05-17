namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a booking configuration for a specific division within a tenant.
/// </summary>
/// <param name="TenantId">The unique identifier of the tenant associated with the booking.</param>
/// <param name="CompleteProfileRequired">A value indicating whether a complete user profile is required to proceed with the booking. <see langword="true"/>
/// if a complete profile is required; otherwise, <see langword="false"/>.</param>
/// <param name="BookingTimeFrame">The time frame, in minutes, within which the booking must be completed.</param>
internal sealed record DivisionBooking(
	int  TenantId,
	bool CompleteProfileRequired,
	int  BookingTimeFrame
);

