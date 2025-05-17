namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents the available view types for displaying fixtures in the application.
/// </summary>
/// <remarks>Each view type is represented by a constant integer value. These values can be used to  specify or
/// identify the desired view type for fixtures, such as advanced views, simple  views, or export options.</remarks>
public static class FixturesViewType
{
	public const int Advanced = 1;
	public const int Simple   = 2;
	public const int Chart    = 3;
	public const int Print    = 4;
	public const int Export   = 5;
}
