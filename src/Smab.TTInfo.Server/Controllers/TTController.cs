using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

namespace Smab.TTInfo.Server.Controllers;

[Route("/api/[action]")]
public partial class TTController : Controller {
	private readonly ITT365Reader _tt365;
	private IMemoryCache _cache;

	public TTController(
		ITT365Reader tt365Reader,
		IMemoryCache memoryCache
		) {
		_tt365 = tt365Reader;
		_cache = memoryCache;
	}


	[HttpGet]
	[Route("{LeagueId}/{TeamName}")]
	public async Task<IActionResult> Fixtures(string LeagueId, string SeasonId, string? TeamName = null) {
		List<Fixture> list = await _tt365.GetAllFixtures(LeagueId, SeasonId) ?? new();
        if (TeamName is not null)

		{
			TeamName = TeamName.Replace("_", " ");
			list = list
				.Where(f => string.Equals(f.HomeTeam, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, TeamName, StringComparison.CurrentCultureIgnoreCase))
				.ToList();
		}

		return Ok(JsonConvert.SerializeObject(list));
	}

	[HttpGet]
	[Route("{LeagueId}/{TeamName}")]
	public async Task<IActionResult> Team(string LeagueId, string TeamName) {
		TeamName = TeamName.Replace("_", " ");

		Team? team = await _tt365.GetTeamStats(LeagueId, TeamName);
		if (team is null) {
			return NotFound();
		}

		return Ok(JsonConvert.SerializeObject(team));
	}

	[HttpGet]
	[Route("{LeagueId}/{TeamName}")]
	public async Task<IActionResult> TeamPlayersList(string LeagueId, string TeamName) {
		TeamName = TeamName.Replace("_", " ");

		Team? team = await _tt365.GetTeamStats(LeagueId, TeamName);
		if (team is null) {
			return NotFound();
		}
		List<string>? teamplayers = (from p in team.Players
									 select p.Name + " (" + p.WinPercentage + ")").ToList();
		return Ok(JsonConvert.SerializeObject(teamplayers));
	}


}
