﻿namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<TeamMember>> GetTeamMembers(int teamId, string leagueId)
	{
		string fileName = $"{leagueId}_team_{teamId}_members.json";
		List<TeamMember>? teamMembers = await LoadJsonAsync<List<TeamMember>>(
			leagueId,
			$"teams/{teamId}/members",
			fileName);

		return teamMembers ?? [];
	}
}