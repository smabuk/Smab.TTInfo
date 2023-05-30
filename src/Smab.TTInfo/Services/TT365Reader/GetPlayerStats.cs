using HtmlAgilityPack;

namespace Smab.TTInfo;

public sealed partial class TT365Reader
{
	public async Task<Player?> GetPlayerStats(string leagueId, Player player, string seasonId = "")
	{
		Player newPlayer = player;

		League? league = await GetLeague(leagueId);
		if (league is null) {
			return null;
		}

		if (string.IsNullOrWhiteSpace(seasonId)) {
			if (league is null) { return null; };
			seasonId = league.CurrentSeason.Id;
		}

		HttpClient client = new();
		string lookupPlayerName = player.Name.Replace("%20", "_").Replace(" ", "_");

		//https://www.tabletennis365.com/Reading/Results/Player/Statistics/Senior_2022-23/Simon_Brookes/335062
		if (string.IsNullOrWhiteSpace(player.PlayerURL)) {
			player.PlayerURL = $"{"https"}://www.tabletennis365.com/{leagueId}/Results/Player/Statistics/{seasonId}/{lookupPlayerName}/{player.Id}";
		}
		HtmlDocument doc = await LoadPage(
			player.PlayerURL,
			$@"{leagueId}_{seasonId}_PlayerStats_{player.Id}.html");


		HtmlNode? statsNode = doc.DocumentNode.SelectSingleNode("//div[@id='PlayerStats']");

		if (statsNode == null) {
			return player;
		}
		string playerTeamName = doc.DocumentNode.SelectSingleNode("//div[@class='team']").SelectSingleNode("span").InnerText.Trim();

		foreach (HtmlNode? table in statsNode.Descendants("table").Where(t => t.SelectSingleNode("caption").InnerText.Contains("Results"))) {
			if (table.SelectSingleNode("caption").InnerText.Contains("&gt;")) {
				playerTeamName = table.SelectSingleNode("caption").InnerText.Split("&gt;").Last().Trim();
			}
			foreach (HtmlNode? resultRow in table.Descendants("tr")) {
				HtmlNode[] cells = resultRow.Descendants("td").ToArray();
				if (cells.Length == 7) {
					string opponentHref = cells[0].Descendants("a").Single().Attributes["href"].Value;
					string opponentName = cells[0].Descendants("a").Single().InnerText.Trim();
					Player opponent = new () {
						Name = opponentName,
						PlayerURL = opponentHref,
					};

					string resultReason = "";
					if (cells[1].Descendants("img").Any()) {
						resultReason = cells[1].SelectNodes("img").FirstOrDefault()?.Attributes["title"].Value.Trim() ?? "";
					}

					string opponentTeam = cells[2].Descendants("a").First().InnerText.Trim();

					DateOnly date = DateOnly.Parse(cells[3].SelectSingleNode("time").Attributes["datetime"].Value);

					string scores = string.Join(",", cells[4].Descendants("span").Select(s => s.InnerText.Trim()));

					bool rankingDiffSuccessful = int.TryParse(cells[5].InnerText, null, out int rankingDiff);

					string result = cells[6].InnerText.Trim();
					string matchCardUrl = cells[6].SelectSingleNode("a").Attributes["href"].Value;

					PlayerResult playerResult = new()
					{
						Name = player.Name,
						Id = player.Id,
						PlayerTeamName = playerTeamName,
						Opponent = opponent,
						OpponentTeam = opponentTeam,
						Date = date,
						Scores = scores,
						RankingDiff = rankingDiffSuccessful ? rankingDiff : null,
						Result = result,
						ResultReason = resultReason,
						MatchCardURL = matchCardUrl,
					};

					newPlayer.PlayerResults.Add(playerResult);
				}
			}
		}

		return newPlayer;
	}
}

