using HtmlAgilityPack;

namespace Smab.TTInfo;

public partial class TT365Reader
{
	public async Task<FixturesView?> GetFixturesByTeamName(string TeamName = "")
		=> await GetFixturesByTeamName(TeamName, League, Season);

	public async Task<FixturesView?> GetFixturesByTeamName(string TeamName = "", string? League = null, string? Season = null)
	{
		// This function will deliberately use different ways to access the data using the HTML Agility Pack
		FixturesView fixturesView = new();
		string Division = "All Divisions";
		string ClubId = ""; // OLOP 411
		string TeamId = ""; // OLOP E 40043
		string VenueId = "";
		int ViewModeType = FixturesViewType.Advanced;
		bool HideCompletedFixtures = false;
		bool MergeDivisions = true;
		bool ShowByWeekNo = true;

		League ??= this.League;
		Season ??= this.Season;

		string lookupTeamName = TeamName.Replace("_", " ");
		Dictionary<string, TeamInfo> TeamInfoLookup;

		TeamInfoLookup = GetTeamInfoForSeason(Season);

		if (TeamName != "")
		{
			if (TeamInfoLookup.ContainsKey(lookupTeamName))
			{
				TeamId = TeamInfoLookup[lookupTeamName].Id.ToString();
				Division = TeamInfoLookup[lookupTeamName].Division;
			}
			else
				return null; /* TODO Change to default(_) if this is not a reference type */
		}

		fixturesView.URL = $"{"https"}://www.tabletennis365.com/{League}/Fixtures/{Season}/{Division}?c=False&vm={ViewModeType}&d={Division}&vn={VenueId}&cl={ClubId}&t={TeamId}&swn={ShowByWeekNo}&hc={HideCompletedFixtures}&md={MergeDivisions}";
		HtmlDocument doc = await LoadPage(
			fixturesView.URL,
			$@"{League}_Fixtures_{TeamName}.html");

		// fixture.Description = fixtureNode.SelectSingleNode("//meta[@itemprop='description']").Attributes("content").Value
		fixturesView.Fixtures = new List<Fixture>();
		foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@id='Fixtures']"))
		{
			fixturesView.Caption = node.SelectSingleNode(".//div[@class='caption']").InnerText.Replace("&gt;", ">");
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
						Division = Division.Replace("%20", " "),
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
					fixturesView.Fixtures.Add(fixture);
				}
			}
		}

		return fixturesView;
	}

}

