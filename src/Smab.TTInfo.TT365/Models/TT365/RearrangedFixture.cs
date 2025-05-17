namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a fixture that has been rescheduled to a new date, including the original date and the reason for the
/// change.
/// </summary>
/// <remarks>This record extends the <see cref="Fixture"/> type to include additional details about a rescheduled
/// fixture. Use this type to track changes to fixture schedules and the reasons for those changes.</remarks>
public record RearrangedFixture : Fixture
{
	public DateOnly OriginalDate {  get; set; }
	public string Reason { get; set; } = "";
}
