using Smab.TTInfo.Shared.Models;

namespace Smab.TTInfo.TT365.Models;

/// <summary>
/// Represents configuration options specific to the TT365 service.
/// </summary>
/// <remarks>This class inherits from <see cref="TTInfoOptions"/> and can be used to configure settings related to
/// the TT365 service. Additional properties or methods may be added to this class to extend its
/// functionality.</remarks>
public sealed class TT365Options : TTInfoOptions
{
	public string WebsiteVersion { get; set; } = @"new2026";
}

public static class TT365OptionsExtensions
{
	extension(TT365Options options)
	{
		/// <summary>
		/// Converts the <see cref="WebsiteVersion"/> string property of <see cref="TT365Options"/> to its corresponding
		/// <see cref="TT365WebsiteVersion"/> enum value.
		/// </summary>
		/// <param name="options">
		/// The <see cref="TT365Options"/> instance containing the <see cref="WebsiteVersion"/> property.
		/// </param>
		/// <returns>The corresponding <see cref="TT365WebsiteVersion"/> enum value.</returns>
		public TT365WebsiteVersion GetTT365WebsiteVersion()
		{
			return options.WebsiteVersion.ToLowerInvariant() switch
			{
				"original" => TT365WebsiteVersion.Original,
				"new2026" => TT365WebsiteVersion.New2026,
				_ => throw new ArgumentOutOfRangeException(nameof(options.WebsiteVersion), $"Invalid website version: {options.WebsiteVersion}")
			};
		}
	}
}
