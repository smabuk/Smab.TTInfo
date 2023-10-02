namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TenantsHostBooking(
	int  TenantId,
	bool CompleteProfileRequired,
	int  BookingTimeFrame
);
