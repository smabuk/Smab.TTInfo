using HtmlAgilityPack;

using Smab.TTInfo.Models.TT365;
using System;

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
						//Division = divisionName.Replace("%20", " "),
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

