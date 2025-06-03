using Smab.TTInfo.Shared.Helpers;

namespace Smab.TTInfo.TT365.Services;

/// <summary>
/// Provides methods to save files to the local cache for TT365.
/// </summary>
public sealed partial class TT365Reader
{
	/// <summary>
	/// Ensures the base address of the HTTP client is set for the specified TT365 league.
	/// </summary>
	/// <param name="leagueId">The TT365 league identifier.</param>
	private void EnsureBaseAddress(TT365LeagueId leagueId)
		=> httpClient.BaseAddress ??= new Uri($"{TT365_COM}/{leagueId}/");
}
