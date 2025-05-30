﻿namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents the available view types for displaying fixtures in the application.
/// </summary>
/// <remarks>Each view type is represented by a constant integer value. These values can be used to specify or
/// identify the desired view type for fixtures, such as advanced views, simple views, or export options.</remarks>
public enum FixturesViewType
{
	Advanced = 1,
	Simple   = 2,
	Chart    = 3,
	Print    = 4,
	Export   = 5,
}
