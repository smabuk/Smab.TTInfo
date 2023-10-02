namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record DivisionBooking(
	int  TenantId,
	bool CompleteProfileRequired,
	int  BookingTimeFrame
);

