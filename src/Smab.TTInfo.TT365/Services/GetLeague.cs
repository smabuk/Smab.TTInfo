﻿using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Retrieves the league information for the specified identifier.
	/// </summary>
	/// <remarks>This method attempts to load the league data from a cached file. If the data is not available  in
	/// the cache or is incomplete, it fetches the league information from an external source,  including its metadata,
	/// current season, and historical seasons. The method also updates the  cache with the retrieved data for future
	/// use.</remarks>
	/// <param name="leagueId">The unique identifier of the league to retrieve. This value cannot be null or empty.</param>
	/// <returns>A <see cref="League"/> object containing the league's details, including its current season,  or <see
	/// langword="null"/> if the league cannot be found or the data is unavailable.</returns>
	public async Task<League?> GetLeague(TT365LeagueId leagueId)
	{
		string url;
		HtmlDocument? doc;
		string? jsonString;
		string fileName = $"{leagueId}.json";

		League? league = await LoadAsync<League>(
			leagueId,
			null,
			fileName);

		if (league is null || string.IsNullOrWhiteSpace(league.CurrentSeason.Id))
		{
			url = "";
			doc = await LoadAsync<HtmlDocument>(
				leagueId,
				url);

			if (string.IsNullOrWhiteSpace(doc?.Text)) { return null; }

			string leagueURL         = $"{TT365_COM}/{leagueId}";
			string leagueName        = HttpUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("//title")?.InnerText ?? "");
			string leagueDescription = HttpUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("//meta[@property='og:description']")?.GetAttributeValue("content", "")) ?? "";
			string leagueTheme       = doc.DocumentNode.SelectSingleNode("//body")?.GetAttributeValue("class", "") ?? "";
			string currentSeasonIdAsString = doc.DocumentNode.SelectSingleNode($"//a[starts-with(@href,'/{leagueId}/Tables/')]")?.GetAttributeValue("href", "") ?? "";
			
			TT365SeasonId currentSeasonId = new(currentSeasonIdAsString[(currentSeasonIdAsString.LastIndexOf('/') + 1)..]);
			string currentSeasonName = doc.DocumentNode.SelectSingleNode($"//a[starts-with(@href,'/{leagueId}/Tables/')]")?.GetAttributeValue("title", "").Replace(" Tables", "") ?? "";
			LookupTables seasonLookups = await GetLookupTables(leagueId, currentSeasonId);
			Season currentSeason = new(currentSeasonId, currentSeasonName, seasonLookups, []);

			HtmlDocument archives = await LoadAsync<HtmlDocument>(
				leagueId,
				$"Results/Archive")
				?? new();

			List<Season> seasons = [currentSeason];
			foreach (HtmlNode? item in archives.DocumentNode.SelectNodes("//td//a") ?? EMPTY_NODE_COLLECTION)
			{
				string seasonId = item.GetAttributeValue("href", "");
				seasonId = seasonId[(seasonId.LastIndexOf('/') + 1)..];
				string seasonName = item.InnerText;
				seasons.Add(new(new(seasonId), seasonName, new(), []));
			}

			league = new(leagueId, leagueName, leagueDescription, leagueURL, leagueTheme, [.. seasons], currentSeason);
		} else {
			league = league with 
			{
				CurrentSeason = league.CurrentSeason with {
						Lookups = await GetLookupTables(leagueId, league.GetCurrentSeasonId())
				}
			};
		}

		league = league with {
			CurrentSeason = league.CurrentSeason with {
				Divisions = [.. await GetDivisions(leagueId, league.GetCurrentSeasonId())]
			}
		};

		jsonString = JsonSerializer.Serialize(league);
		_ = SaveFileToCache(jsonString, $"{leagueId}.json");
		_ = SaveFileToCache(jsonString, $"{leagueId}_{league.CurrentSeason.Id}.json");
		
		return league;
	}
}
