using HtmlAgilityPack;

namespace Smab.TTInfo;

public partial class TT365Reader
{

	public async Task<List<Fixture>?> GetAllFixtures(string LeagueId, string? SeasonId = null)
	{
		List<Fixture> fixtures = new();
		string division = "All Divisions";
		string clubId = "";
		string teamId = "";
		string venueId = "";
		int viewModeType = FixturesViewType.Advanced;
		bool hideCompletedFixtures = false;
		bool mergeDivisions = true;
		bool showByWeekNo = true;

		string leagueId = LeagueId;
		string? seasonId = SeasonId ?? (await GetLeague(leagueId))?.CurrentSeason.Id;

		ArgumentNullException.ThrowIfNull(seasonId, nameof(seasonId));

		string url = $"{"https"}://www.tabletennis365.com/{leagueId}/Fixtures/{seasonId}/{division}?c=False&vm={viewModeType}&d={division}&vn={venueId}&cl={clubId}&t={teamId}&swn={showByWeekNo}&hc={hideCompletedFixtures}&md={mergeDivisions}";
		HtmlDocument doc = await LoadPage(
			url,
			$@"{leagueId}_Fixtures_All_Divisions.html");

		foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@id='Fixtures']"))
		{
			foreach (HtmlNode fixtureNode in node.SelectNodes(".//div[contains(@class, 'fixture')]")) // This doesn't work as fixtureWeek is a class that would match this
			{
				// Select all <div>s that have a class of "fixture" (e.g. class="fixture" or class="fixture complete")
				// For Each fixtureNode In node.Descendants("div").Where(Function(x) x.Attributes["class"].Value?.Split(" ").Contains("fixture", StringComparer.InvariantCultureIgnoreCase)))
				string nodeClass = fixtureNode.Attributes["class"].Value;

				if (nodeClass.HasClass("fixture"))
				{
					bool CompletedFixture = nodeClass.HasClass("complete");

					Fixture fixture = new()
					{
						Division = division.Replace("%20", " "),
						IsCompleted = CompletedFixture
					};
					fixture.Description = fixtureNode.Descendants("meta").Where(x => x.Attributes["itemprop"].Value == "description").Single().Attributes["content"].Value;
					foreach (HtmlNode divNode in fixtureNode.Descendants("div"))
					{
						switch (divNode.Attributes["class"]?.Value.Trim().ToUpperInvariant())
						{
							case "DATE":
								{
									if (DateOnly.TryParse(divNode.Descendants("time").SingleOrDefault()?.Attributes["datetime"].Value, out DateOnly tempDate))
									{
										fixture.Date = tempDate;
									};
									break;
								}
							case "DIV":
								{
									fixture.Division = divNode.InnerText;
									break;
								}
							case "HOME":
								{
									fixture.HomeTeam = divNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "teamName").SingleOrDefault()?.InnerText.Replace("&amp;", "&") ?? "";
									if (CompletedFixture)
										fixture.ForHome = int.Parse(divNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
									break;
								}
							case "AWAY":
								{
									fixture.AwayTeam = divNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "teamName").SingleOrDefault()?.InnerText.Replace("&amp;", "&") ?? "";
									if (CompletedFixture)
									fixture.ForAway = int.Parse(divNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
									break;
								}
							case "MATCHCARDICON":
								{
									if (CompletedFixture)
									fixture.CardURL = $"{"https"}://www.tabletennis365.com{divNode.Descendants("a").SingleOrDefault()?.Attributes["href"].Value.Trim() ?? ""}";
									break;
								}
							case "ICON POSTPONED":
								{
									fixture.Postponed = divNode.Attributes["title"].Value.Trim();
									break;
								}
							case "VENUE":
								{
									fixture.Venue = divNode.ChildNodes[0].ChildNodes[0].InnerText.Replace("&amp;", "&");
									break;
								}
							default:
								{
									break;
								}
						}
					}
					fixtures.Add(fixture);
				}
			}
		}

		return fixtures;
	}

}

