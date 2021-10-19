using HtmlAgilityPack;

namespace Smab.TTInfo;

public partial class TT365Reader
{

	public async Task<List<Fixture>?> GetAllFixtures(string LeagueId, string? SeasonId = null)
	{
		List<Fixture> fixtures = new();

		string leagueId = LeagueId;
		string? seasonId = SeasonId ?? (await GetLeague(leagueId))?.CurrentSeason.Id;

		ArgumentNullException.ThrowIfNull(seasonId, nameof(seasonId));

		FixturesViewOptions fvo = new()
		{
			DivisionName = "All Divisions",
			ClubId = "",
			TeamId = "",
			VenueId = "",
			ViewModeType = FixturesViewType.Advanced,
			HideCompletedFixtures = false,
			MergeDivisions = true,
			ShowByWeekNo = true
		};

		string url = $"{"https"}://www.tabletennis365.com/{leagueId}/Fixtures/{seasonId}/{fvo.DivisionName}?c=False&vm={fvo.ViewModeType}&d={fvo.DivisionName}&vn={fvo.VenueId}&cl={fvo.ClubId}&t={fvo.TeamId}&swn={fvo.ShowByWeekNo}&hc={fvo.HideCompletedFixtures}&md={fvo.MergeDivisions}";
		HtmlDocument doc = await LoadPage(
					url,
					$@"{leagueId}_Fixtures_All_Divisions.html");

		if (string.IsNullOrWhiteSpace(doc.Text)) { return null; }

		foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@id='Fixtures']"))
		{
			foreach (HtmlNode fixtureNode in node.SelectNodes(".//div[contains(@class, 'fixture')]")) // This doesn't work as fixtureWeek is a class that would match this
			{
				// Select all <div>s that have a class of "fixture" (e.g. class="fixture" or class="fixture complete")
				// For Each fixtureNode In node.Descendants("div").Where(Function(x) x.Attributes["class"].Value?.Split(" ").Contains("fixture", StringComparer.InvariantCultureIgnoreCase)))
				string nodeClass = fixtureNode.Attributes["class"].Value;

				if (nodeClass.HasClass("fixture"))
				{
					Fixture fixture;

					string? postponed = fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'postponed')]")?.Attributes["title"].Value.Trim();

					if (nodeClass.HasClass("complete")) { 
						fixture = new CompletedFixture();
					} else if (postponed is not null) {
						fixture = new PostponedFixture();
					} else {
						fixture = new();
					}

					fixture.Description = fixtureNode.Descendants("meta").Where(x => x.Attributes["itemprop"].Value == "description").Single().Attributes["content"].Value;
					if (DateOnly.TryParse(fixtureNode.Descendants("time").SingleOrDefault()?.Attributes["datetime"].Value, out DateOnly tempDate))
					{
						fixture.Date = tempDate;
					};
					fixture.Division = fixtureNode.SelectSingleNode("div[@class='div']").InnerText;
					fixture.Venue = fixtureNode.SelectSingleNode("div[@class='venue']/span/a").InnerText.Replace("&amp;", "&");

					HtmlNode homeNode = fixtureNode.SelectSingleNode("div[@class='home']");
					fixture.HomeTeam = homeNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "teamName").SingleOrDefault()?.InnerText.Replace("&amp;", "&") ?? "";

					HtmlNode awayNode = fixtureNode.SelectSingleNode("div[@class='away']");
					fixture.AwayTeam = awayNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "teamName").SingleOrDefault()?.InnerText.Replace("&amp;", "&") ?? "";

					if (fixture is CompletedFixture completedFixture)
					{
						completedFixture.ForHome = int.Parse(homeNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
						completedFixture.ForAway = int.Parse(awayNode.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
						completedFixture.CardURL = $"{"https"}://www.tabletennis365.com{fixtureNode.SelectSingleNode("div/div[@class='matchCardIcon']/a").Attributes["href"].Value.Trim() ?? ""}";
						foreach (HtmlNode playerNode in fixtureNode.SelectNodes(".//div[@itemprop='performer' and starts-with(@class, 'player')]"))
						{
							string playerName = playerNode.SelectSingleNode("span/a")?.InnerText ?? playerNode.SelectSingleNode("span").InnerText;
							playerName = FixPlayerName(playerName);
							string? playerIdString = playerNode.SelectSingleNode("span/a")?.GetAttributeValue("href", null);
							int playerId = 0;
							_ = int.TryParse(playerNode.LastChild.InnerText.Replace("(", "").Replace(")", ""), out int setsWon);
							if (playerIdString is not null)
							{
								playerId = string.IsNullOrWhiteSpace(playerIdString) ? 0 : int.Parse(playerIdString.Split('/').LastOrDefault() ?? "");
							}
							bool playerPoM = playerNode.HasClass("pom");
							MatchPlayer matchPlayer = new(playerName, playerId, setsWon, playerPoM);
							if (playerNode.ParentNode.HasClass("homeTeam"))
							{
								completedFixture.HomePlayers.Add(matchPlayer);
							}
							else
							{
								completedFixture.AwayPlayers.Add(matchPlayer);
							}
						}
					}

					if (fixture is PostponedFixture pf)
					{
						pf.Postponed = postponed;
					}

					fixtures.Add(fixture);
				}
			}
		}

		return fixtures;
	}

}

