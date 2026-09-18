using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Retrieves all fixtures for a specified league and optionally for a specific season.
	/// If the season isn't specified the current season of the league is used.
	/// </summary>
	/// <remarks>If the <paramref name="seasonId"/> is not provided and the league does not have a current season,
	/// an empty list is returned. The method attempts to load cached fixture data first; if unavailable,  it fetches the
	/// data from an external source.</remarks>
	/// <param name="leagueId">The unique identifier of the league for which to retrieve fixtures.</param>
	/// <param name="seasonId">The unique identifier of the season for which to retrieve fixtures. If <see langword="null"/>,  the current season
	/// of the league will be used.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see cref="Fixture"/>
	/// objects representing the fixtures for the specified league and season. If no fixtures are found, an empty list is
	/// returned.</returns>
	public async Task<List<Fixture>> GetAllFixtures(TT365LeagueId leagueId, TT365SeasonId? seasonId = null)
	{
		return WebsiteVersion switch
		{
			TT365WebsiteVersion.Original => await GetAllFixturesOriginal(leagueId, seasonId),
			TT365WebsiteVersion.New2026 => await GetAllFixturesNew2026(leagueId, seasonId),
			_ => throw new NotSupportedException($"Website version {WebsiteVersion} is not supported ."),
		};
	}

	private async Task<List<Fixture>> GetAllFixturesNew2026(TT365LeagueId leagueId, TT365SeasonId? seasonId)
	{
		League? league = await GetLeague(leagueId);
		TT365SeasonId currentSeasonId = league?.CurrentSeasonId ?? throw new InvalidOperationException($"League with ID {leagueId} does not have a current season.");
		seasonId ??= currentSeasonId;
		if (seasonId is null) {
			// TODO: Log this error
			return [];
		}

		string filename = $@"{leagueId}_{seasonId}_fixtures_all.json";

		// Attempt to load fixtures from cache
		// If the requested season is the current season, use the default cache expiration; otherwise, use a long expiration for historical data
		List<Fixture> fixtures = currentSeasonId == seasonId
			? await LoadAsync<List<Fixture>?>(leagueId, null, filename) ?? []
			: await LoadAsync<List<Fixture>?>(leagueId, null, filename, cacheHours: 10_000_000) ?? [];

		if (fixtures is not []) { return fixtures; }

		Dictionary<string, string> teamToDivision = league.GetSeason((TT365SeasonId)seasonId)?.Divisions
			.SelectMany(d => d.Teams.Select(t => new { TeamName = t.Name, DivisionId = d.Id }))
			.ToDictionary(x => x.TeamName, x => x.DivisionId) ?? [];

		FixturesViewOptions fvo = FixturesViewOptions.Create
		(
			season: seasonId,
			divisionName: "",
			clubId: "",
			teamId: "",
			venueId: "",
			viewModeType: FixturesViewType.Advanced,
			hideCompletedFixtures: false,
			mergeDivisions: true,
			showByWeekNo: true
		);

		string url = $"Fixtures?leagueName={seasonId.ToString()?.Replace("_", "+")}&divisionName={fvo.DivisionName}&vm=1&vn={fvo.VenueId}&cl={fvo.ClubId}&t={fvo.TeamId}&showCompleted={!fvo.HideCompletedFixtures}&merge={fvo.MergeDivisions}";
		HtmlDocument? doc = await LoadAsync<HtmlDocument>(
			leagueId,
			url,
			filename.Replace("json", "html")
		);

		if (string.IsNullOrWhiteSpace(doc?.Text)) { return fixtures; }

		if (doc.DocumentNode.SelectNodes("//div[contains(@class, 'tt-filters-content')]") is null) { return fixtures; }

		foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//table[contains(@class, 'tt-fixture-table')]") ?? EMPTY_NODE_COLLECTION) {
			foreach (HtmlNode fixtureNode in node.SelectNodes("./tbody//tr") ?? EMPTY_NODE_COLLECTION) {
				string nodeClass = fixtureNode.Attributes["class"]?.Value ?? "";

				string fixtureDescription = "";
				//string fixtureDescription = fixtureNode.Descendants("meta").Where(x => x.Attributes["itemprop"].Value == "description").Single().Attributes["content"].Value;
				string partialDate = fixtureNode.SelectSingleNode("td[@class='tt-fixture-date']").InnerText.Trim();
				_ = DateHelpers.TryParseWithMissingYear(partialDate, seasonId?.StartYear ?? 0, out DateOnly fixtureDate);

				string fixtureVenue = HttpUtility.HtmlDecode(fixtureNode.SelectSingleNode("td[@class='tt-fixture-venue']")?.InnerText ?? "");

				HtmlNode? homeNode = fixtureNode.SelectSingleNode("td[2]/a");
				HtmlNode? awayNode = fixtureNode.SelectSingleNode("td[4]/a");

				string fixtureHomeTeam = homeNode?.InnerText.Trim() ?? "";
				string fixtureAwayTeam = awayNode?.InnerText.Trim() ?? "";

				string fixtureDivision = teamToDivision.GetValueOrDefault(fixtureHomeTeam, "");
				Fixture fixture = new(fixtureDivision, fixtureDescription, fixtureDate, fixtureHomeTeam, fixtureAwayTeam, fixtureVenue);

				FixtureType fixtureType = DetermineFixtureTypeNew2026(fixtureNode, nodeClass);
				fixture = fixtureType switch
				{
					FixtureType.Completed => ParseToCompletedFixtureNew2026(fixtureNode, fixture),
					FixtureType.Postponed => ParseToPostponedFixtureNew2026(fixtureNode, fixture),
					FixtureType.Rearranged => ParseToRearrangedFixtureNew2026(fixtureNode, fixture),
					FixtureType.Void => ParseToVoidFixtureNew2026(fixtureNode, fixture),
					_ => fixture
				};

				fixtures.Add(fixture);
			}
		}

		string jsonString = JsonSerializer.Serialize(fixtures);
		_ = SaveFileToCache(jsonString, filename);

		return fixtures;

	}

	private Fixture ParseToCompletedFixtureNew2026(HtmlNode fixtureNode, Fixture fixture)
	{
		string[] scores = fixtureNode.SelectSingleNode(".//span[contains(@class,'tt-result-score-text')]")?.InnerText.Split('-', StringSplitOptions.TrimEntries) ?? [];
		int forHome = 0;
		int forAway = 0;
		// ToDo: this should never happen, but there is a bug on the beta TT365 site where the score is not displayed for some completed fixtures, so we need to handle this gracefully
		if (scores.Count() == 2) {
			forHome = int.Parse(scores[0]);
			forAway = int.Parse(scores[1]);
		}


		string cardURL = $"{TT365_COM}{fixtureNode.SelectSingleNode("td[contains(@class,'tt-fixture-score')]/a")?.Attributes["href"].Value.Trim() ?? ""}";
		HtmlNodeCollection? playerNodes = fixtureNode.SelectNodes(".//li[contains(@class, 'tt-fixture-scorecard-player')]");
		string playerOfTheMatchName = "";
		List<MatchPlayer> homePlayers = [];
		List<MatchPlayer> awayPlayers = [];
		if (playerNodes is not null) {
			foreach (HtmlNode playerNode in playerNodes) {
				string playerName = playerNode.SelectSingleNode("a")?.InnerText ?? "";
				playerName = FixPlayerName(playerName);
				string? playerIdString = playerNode.SelectSingleNode("a")?.GetAttributeValue("href", null!);
				int playerId = 0;
				_ = int.TryParse(playerNode.SelectSingleNode("span")?.InnerText, out int setsWon);
				if (playerIdString is not null) {
					playerId = string.IsNullOrWhiteSpace(playerIdString) ? 0 : int.Parse(playerIdString.Split('=').LastOrDefault() ?? "");
				}

				bool playerPoM = playerNode.HasClass("tt-fixture-scorecard-pom");
				MatchPlayer matchPlayer = new(playerName, playerId, setsWon, playerPoM);
				if (playerNode.ParentNode.HasClass("tt-fixture-scorecard-home")) {
					homePlayers.Add(matchPlayer);
				} else {
					awayPlayers.Add(matchPlayer);
				}

				if (playerPoM) {
					playerOfTheMatchName = FixPlayerName(matchPlayer.Name);
				}
			}
		}

		return fixture.ToCompleted() with
		{
			ForHome = forHome,
			ForAway = forAway,
			PlayerOfTheMatch = playerOfTheMatchName,
			CardURL = cardURL,
			HomePlayers = homePlayers,
			AwayPlayers = awayPlayers,
		};
	}

	private static Fixture ParseToPostponedFixtureNew2026(HtmlNode fixtureNode, Fixture fixture) => fixture;
	private static RearrangedFixture ParseToRearrangedFixtureNew2026(HtmlNode fixtureNode, Fixture fixture)
	{
		string title = HttpUtility.HtmlDecode(
			fixtureNode
			.SelectSingleNode(".//span[contains(@class,'tt-fixture-badge-rearranged')]")?
			.Attributes["title"].Value
			.Trim()
			) ?? "";
		string[] tokens = title.Split([':', '-'], StringSplitOptions.TrimEntries);
		if (tokens.Length == 2) {
			string reason = tokens[^1];
			int startYYear = fixture.Date.Month >= 9 ? fixture.Date.Year : fixture.Date.Year - 1;
			_ = DateHelpers.TryParseWithMissingYear(tokens[0].Replace("Rearranged from", "").Trim(), startYYear, out DateOnly originalDate);
			return fixture.ToRearranged(originalDate, reason);
		} else {
			return fixture.ToRearranged(default, title);
		}
	}

	private static Fixture ParseToVoidFixtureNew2026(HtmlNode fixtureNode, Fixture fixture) => fixture;

	private static FixtureType DetermineFixtureTypeNew2026(HtmlNode fixtureNode, string nodeClass)
	{
		return nodeClass.HasClass("tt-fixture-completed")
			? fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'voided')]") is not null
				? FixtureType.Void
				: FixtureType.Completed
			: fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'postponed')]") is not null
				? FixtureType.Postponed
				: fixtureNode.SelectSingleNode(".//span[contains(@class,'tt-fixture-badge-rearranged')]") is not null
					? FixtureType.Rearranged
					: FixtureType.Fixture;
	}


	private async Task<List<Fixture>> GetAllFixturesOriginal(TT365LeagueId leagueId, TT365SeasonId? seasonId)
	{
		TT365SeasonId currentSeasonId = (await GetLeague(leagueId))?.CurrentSeasonId ?? throw new InvalidOperationException($"League with ID {leagueId} does not have a current season.");
		seasonId ??= currentSeasonId;
		if (seasonId is null) {
			// TODO: Log this error
			return [];
		}

		string filename = $@"{leagueId}_{seasonId}_fixtures_all.json";

		// Attempt to load fixtures from cache
		// If the requested season is the current season, use the default cache expiration; otherwise, use a long expiration for historical data
		List<Fixture> fixtures = currentSeasonId == seasonId
			? await LoadAsync<List<Fixture>?>(leagueId, null, filename) ?? []
			: await LoadAsync<List<Fixture>?>(leagueId, null, filename, cacheHours: 10_000_000) ?? [];

		if (fixtures is not []) { return fixtures; }

		FixturesViewOptions fvo = FixturesViewOptions.Create
		(
			season: seasonId,
			divisionName: "All Divisions",
			clubId: "",
			teamId: "",
			venueId: "",
			viewModeType: FixturesViewType.Advanced,
			hideCompletedFixtures: false,
			mergeDivisions: true,
			showByWeekNo: true
		);

		string url = $"Fixtures/{seasonId}/{fvo.DivisionName}?c=False&vm={fvo.ViewModeType}&d={fvo.DivisionName}&vn={fvo.VenueId}&cl={fvo.ClubId}&t={fvo.TeamId}&swn={fvo.ShowByWeekNo}&hc={fvo.HideCompletedFixtures}&md={fvo.MergeDivisions}";
		HtmlDocument? doc = await LoadAsync<HtmlDocument>(
			leagueId,
			url,
			filename.Replace("json", "html")
		);

		if (string.IsNullOrWhiteSpace(doc?.Text)) { return fixtures; }

		if (doc.DocumentNode.SelectNodes("//div[@id='Fixtures']") is null) { return fixtures; }

		foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@id='Fixtures']") ?? EMPTY_NODE_COLLECTION) {
			foreach (HtmlNode fixtureNode in node.SelectNodes(".//div[contains(@class, 'fixture')]") ?? EMPTY_NODE_COLLECTION) {
				string nodeClass = fixtureNode.Attributes["class"].Value;

				if (nodeClass.HasClass("fixture")) {
					string fixtureDescription = fixtureNode.Descendants("meta").Where(x => x.Attributes["itemprop"].Value == "description").Single().Attributes["content"].Value;
					_ = DateOnly.TryParse(fixtureNode.Descendants("time").SingleOrDefault()?.Attributes["datetime"].Value, out DateOnly fixtureDate);
					string fixtureDivision = fixtureNode.GetSingleNodeByClass("div")?.InnerText ?? "";
					string fixtureVenue = HttpUtility.HtmlDecode(fixtureNode.SelectSingleNode("div[@class='venue']/span/a")?.InnerText ?? "");

					HtmlNode? homeNode = fixtureNode.GetSingleNodeByClass("home");
					HtmlNode? awayNode = fixtureNode.GetSingleNodeByClass("away");

					string fixtureHomeTeam = GetTeamName(homeNode);
					string fixtureAwayTeam = GetTeamName(awayNode);

					Fixture fixture = new(fixtureDivision, fixtureDescription, fixtureDate, fixtureHomeTeam, fixtureAwayTeam, fixtureVenue);

					FixtureType fixtureType = DetermineFixtureType(fixtureNode, nodeClass);
					fixture = fixtureType switch
					{
						FixtureType.Completed => ParseToCompletedFixture(fixtureNode, homeNode, awayNode, fixture),
						FixtureType.Postponed => ParseToPostponedFixture(fixtureNode, fixture),
						FixtureType.Rearranged => ParseToRearrangedFixture(fixtureNode, fixture),
						FixtureType.Void => ParseToVoidFixture(fixtureNode, fixture),
						_ => fixture
					};

					fixtures.Add(fixture);
				}
			}
		}

		string jsonString = JsonSerializer.Serialize(fixtures);
		_ = SaveFileToCache(jsonString, filename);

		return fixtures;

		static string GetTeamName(HtmlNode? homeNode)
			=> HttpUtility.HtmlDecode(
				homeNode?
				.Descendants("div")
				.Where(x => x.HasClass("teamName"))
				.SingleOrDefault()?
				.InnerText
				) ?? "";
	}

	/// <summary>
	/// Determines the type of a fixture based on the provided HTML node and its associated CSS class.
	/// </summary>
	/// <remarks>This method inspects the structure and attributes of the provided HTML node to determine the
	/// fixture's state. The determination is based on specific child elements and their associated CSS classes.</remarks>
	/// <param name="fixtureNode">The HTML node representing the fixture. This node is used to inspect specific child elements to determine the
	/// fixture type.</param>
	/// <param name="nodeClass">The CSS class of the node, which is used to identify certain fixture states such as "complete".</param>
	/// <returns>A <see cref="FixtureType"/> value indicating the type of the fixture. Possible values include: <list type="bullet">
	/// <item><description><see cref="FixtureType.Void"/> if the fixture is marked as voided.</description></item>
	/// <item><description><see cref="FixtureType.Completed"/> if the fixture is marked as completed.</description></item>
	/// <item><description><see cref="FixtureType.Postponed"/> if the fixture is marked as postponed.</description></item>
	/// <item><description><see cref="FixtureType.Rearranged"/> if the fixture is marked as
	/// rearranged.</description></item> <item><description><see cref="FixtureType.Fixture"/> if none of the above
	/// conditions are met.</description></item> </list></returns>
	private static FixtureType DetermineFixtureType(HtmlNode fixtureNode, string nodeClass)
	{
		return nodeClass.HasClass("complete")
			? fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'voided')]") is not null
				? FixtureType.Void
				: FixtureType.Completed
			: fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'postponed')]") is not null
				? FixtureType.Postponed
				: fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'rearranged')]") is not null
					? FixtureType.Rearranged
					: FixtureType.Fixture;
	}

	/// <summary>
	/// Parses the provided HTML nodes and fixture data to create a <see cref="CompletedFixture"/> object.
	/// </summary>
	/// <remarks>This method extracts detailed information about a completed fixture, including scores for both
	/// teams, player details, and the URL of the match card. It assumes a specific HTML structure for parsing the
	/// data.</remarks>
	/// <param name="fixtureNode">The root HTML node containing the fixture details.</param>
	/// <param name="homeNode">The HTML node containing the home team's score. Can be <see langword="null"/>.</param>
	/// <param name="awayNode">The HTML node containing the away team's score. Can be <see langword="null"/>.</param>
	/// <param name="fixture">The base <see cref="Fixture"/> object to be converted into a <see cref="CompletedFixture"/>.</param>
	/// <returns>A <see cref="CompletedFixture"/> object populated with the parsed data, including scores, players, and additional
	/// metadata.</returns>
	private CompletedFixture ParseToCompletedFixture(HtmlNode fixtureNode, HtmlNode? homeNode, HtmlNode? awayNode, Fixture fixture)
	{
		int forHome = int.Parse(homeNode?.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
		int forAway = int.Parse(awayNode?.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
		string cardURL = $"{TT365_COM}{fixtureNode.SelectSingleNode("div/div[@class='matchCardIcon']/a")?.Attributes["href"].Value.Trim() ?? ""}";
		HtmlNodeCollection? playerNodes = fixtureNode.SelectNodes(".//div[@itemprop='performer' and starts-with(@class, 'player')]");
		string playerOfTheMatchName = "";
		List<MatchPlayer> homePlayers = [];
		List<MatchPlayer> awayPlayers = [];
		if (playerNodes is not null) {
			foreach (HtmlNode playerNode in playerNodes) {
				string playerName = playerNode.SelectSingleNode("span/a")?.InnerText ?? playerNode.SelectSingleNode("span")?.InnerText ?? "";
				playerName = FixPlayerName(playerName);
				string? playerIdString = playerNode.SelectSingleNode("span/a")?.GetAttributeValue("href", null!);
				int playerId = 0;
				_ = int.TryParse(playerNode.LastChild.InnerText.Replace("(", "").Replace(")", ""), out int setsWon);
				if (playerIdString is not null) {
					playerId = string.IsNullOrWhiteSpace(playerIdString) ? 0 : int.Parse(playerIdString.Split('/').LastOrDefault() ?? "");
				}

				bool playerPoM = playerNode.HasClass("pom");
				MatchPlayer matchPlayer = new(playerName, playerId, setsWon, playerPoM);
				if (playerNode.ParentNode.HasClass("homeTeam")) {
					homePlayers.Add(matchPlayer);
				} else {
					awayPlayers.Add(matchPlayer);
				}

				if (playerPoM) {
					playerOfTheMatchName = FixPlayerName(matchPlayer.Name);
				}
			}
		}

		return fixture.ToCompleted() with
		{
			ForHome = forHome,
			ForAway = forAway,
			PlayerOfTheMatch = playerOfTheMatchName,
			CardURL = cardURL,
			HomePlayers = homePlayers,
			AwayPlayers = awayPlayers,
		};
	}

	/// <summary>
	/// Converts a fixture node from an HTML document into a <see cref="PostponedFixture"/> object, including the reason
	/// for postponement.
	/// </summary>
	/// <remarks>The postponement reason is extracted from the "title" attribute of a specific HTML element within
	/// the <paramref name="fixtureNode"/>. If the attribute is missing or empty, the reason will default to an empty
	/// string.</remarks>
	/// <param name="fixtureNode">The HTML node representing the fixture. Must not be <c>null</c>.</param>
	/// <param name="fixture">The <see cref="Fixture"/> object to be converted. Must not be <c>null</c>.</param>
	/// <returns>A <see cref="PostponedFixture"/> object containing the data from the provided <paramref name="fixture"/> and the
	/// postponement reason extracted from the <paramref name="fixtureNode"/>.</returns>
	private static PostponedFixture ParseToPostponedFixture(HtmlNode fixtureNode, Fixture fixture)
	{
		string reason = HttpUtility.HtmlDecode(
			fixtureNode
			.SelectSingleNode("div[@class='spacer']/div[contains(@class,'postponed')]")?
			.Attributes["title"].Value
			.Trim()
			) ?? "";
		return fixture.ToPostponed(reason);
	}

	/// <summary>
	/// Parses the provided HTML node and fixture to create a <see cref="RearrangedFixture"/> object.
	/// </summary>
	/// <remarks>The method extracts rearrangement details, including the original date and reason, from the title
	/// attribute of the provided HTML node. The extracted information is used to create a rearranged fixture based on the
	/// provided <paramref name="fixture"/>.</remarks>
	/// <param name="fixtureNode">The HTML node containing fixture information. Must include a title attribute with rearrangement details.</param>
	/// <param name="fixture">The original <see cref="Fixture"/> object to be transformed into a rearranged fixture.</param>
	/// <returns>A <see cref="RearrangedFixture"/> object containing the original date and reason for the rearrangement.</returns>
	private static RearrangedFixture ParseToRearrangedFixture(HtmlNode fixtureNode, Fixture fixture)
	{
		string title = HttpUtility.HtmlDecode(
			fixtureNode
			.SelectSingleNode("div[@class='spacer']/div[contains(@class,'rearranged')]")?
			.Attributes["title"].Value
			.Trim()
			) ?? "";
		string[] tokens = title.Split([':', '-'], StringSplitOptions.TrimEntries);
		string reason = tokens[^1];
		DateOnly originalDate = DateOnly.Parse(tokens[2].Split([' '])[0]);
		return fixture.ToRearranged(originalDate, reason);
	}

	/// <summary>
	/// Converts the specified fixture to a voided fixture based on the provided HTML node.
	/// </summary>
	/// <remarks>This method extracts the reason for voiding the fixture from the provided HTML node and applies it
	/// to the specified fixture.</remarks>
	/// <param name="fixtureNode">The HTML node containing information about the voided fixture. Must not be null.</param>
	/// <param name="fixture">The fixture to be converted. Must not be null.</param>
	/// <returns>A <see cref="VoidFixture"/> representing the voided state of the fixture.</returns>
	private static VoidFixture ParseToVoidFixture(HtmlNode fixtureNode, Fixture fixture)
	{
		string reason = fixtureNode
			.SelectSingleNode("div[@class='spacer']/div[contains(@class,'voided')]")?
			.Attributes["title"].Value
			.Trim() ?? "";
		return fixture.ToVoid(reason);
	}
}

/// <summary>
/// Represents the type or status of a fixture in a scheduling or event context.
/// </summary>
/// <remarks>This enumeration is used to categorize fixtures based on their current state,  such as
/// whether they are completed, postponed, or voided. The default value is <see cref="Unknown"/>.</remarks>
enum FixtureType
{
	Unknown,
	Fixture,
	Completed,
	Postponed,
	Rearranged,
	Void
}
