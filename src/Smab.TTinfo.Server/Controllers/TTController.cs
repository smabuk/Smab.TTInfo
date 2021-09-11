using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Smab.TTInfo.Server.Controllers;

[Route("/api/[action]")]
public partial class TTController : Controller {
	private readonly ITT365Service _tt365;
	private IMemoryCache _cache;

	public TTController(
		ITT365Service tt365Service,
		IMemoryCache memoryCache
		) {
		_tt365 = tt365Service;
		_cache = memoryCache;
	}


	[HttpGet]
	[Route("{TeamName}")]
	public async Task<IActionResult> ReadingTeam(String TeamName) {
		TeamName = TeamName.Replace("_", " ");
		TT365Models.ReadingTeam model = new(
			 await TT365ReaderHelper.GetTeam(_tt365, TeamName),
			 await _tt365.GetFixturesAdvancedView(TeamName) ?? new()
		);

		return Ok(model);
	}


	[HttpGet]
	[Route("{TeamName}")]
	public async Task<IActionResult> Fixtures(String TeamName = "") {
		TeamName = TeamName.Replace("_", " ");
		TT365Models.FixturesView list = await _tt365.GetFixturesAdvancedView(TeamName) ?? new();

		return Ok(list);
	}

	[HttpGet]
	[Route("{TeamName}")]
	public async Task<IActionResult> Team(String TeamName) {
		TeamName = TeamName.Replace("_", " ");

		TT365Models.Team? team = await _tt365.GetTeamStats(TeamName);
		if (team is null) {
			return NotFound();
		}

		return Ok(team);
	}

	[HttpGet]
	[Route("{TeamName}")]
	public async Task<IActionResult> TeamPlayersList(String TeamName) {
		TeamName = TeamName.Replace("_", " ");

		TT365Models.Team? team = await _tt365.GetTeamStats(TeamName);
		if (team is null) {
			return NotFound();
		}
		List<string>? teamplayers = (from p in team.Players
									 select p.Name + " (" + p.WinPercentage + ")").ToList();
		return Ok(teamplayers);
	}




}
