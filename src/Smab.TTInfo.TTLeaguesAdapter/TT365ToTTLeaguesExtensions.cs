using System.Collections.Immutable;

using TTLeaguesIntKeyValue     = Smab.TTInfo.TTLeagues.Models.TTLeagues.IntKeyValue;
using TTLeaguesMatchPlayer     = Smab.TTInfo.TTLeagues.Models.TTLeagues.MatchPlayer;
using TTLeaguesPlayerAverage   = Smab.TTInfo.TTLeagues.Models.TTLeagues.PlayerAverage;
using TTLeaguesPlayerGame      = Smab.TTInfo.TTLeagues.Models.TTLeagues.PlayerGame;
using TTLeaguesPlayerRanking   = Smab.TTInfo.TTLeagues.Models.TTLeagues.PlayerRanking;
using TTLeaguesPlayerResult    = Smab.TTInfo.TTLeagues.Models.TTLeagues.PlayerResult;
using TTLeaguesPlayerStats     = Smab.TTInfo.TTLeagues.Models.TTLeagues.PlayerStats;
using TTLeaguesStringKeyValue  = Smab.TTInfo.TTLeagues.Models.TTLeagues.StringKeyValue;
using TTLeaguesTeamStatsPlayer = Smab.TTInfo.TTLeagues.Models.TTLeagues.TeamStatsPlayer;
using TTLeaguesDivision        = Smab.TTInfo.TTLeagues.Models.TTLeagues.Division;
using TTLeaguesTeam            = Smab.TTInfo.TTLeagues.Models.TTLeagues.Team;
using TTLeaguesMatch           = Smab.TTInfo.TTLeagues.Models.TTLeagues.Match;
using TTLeaguesMatchTeamInfo   = Smab.TTInfo.TTLeagues.Models.TTLeagues.MatchTeamInfo;
using TTLeaguesLeague          = Smab.TTInfo.TTLeagues.Models.League;
using TT365League              = Smab.TTInfo.TT365.Models.TT365.League;
using TT365Division            = Smab.TTInfo.TT365.Models.TT365.Division;
using TT365Team                = Smab.TTInfo.TT365.Models.TT365.Team;
using TT365Fixture             = Smab.TTInfo.TT365.Models.TT365.Fixture;

namespace Smab.TTInfo.TTLeaguesAdapter;

public static class TT365ToTTLeaguesExtensions
{
	public static TTLeaguesDivision ToTTLeaguesDivision(this TT365Division division, int competitionId)
	{
		return new TTLeaguesDivision(
			Id: int.TryParse(division.Id, out int id) ? id : 0,
			Name: division.Name,
			Description: string.Empty,
			CompetitionId: competitionId,
			Competition: null!,
			UserId: string.Empty,
			Ordering: 0,
			MiniOrdering: null!,
			Status: 0,
			Details: string.Empty,
			Updated: DateTimeOffset.MinValue,
			StartDate: null!,
			Reserves: null!,
			GamesPerSet: null!,
			GameFormat: null!,
			FormatId: null!,
			Format: null!,
			PointId: null!,
			Point: null!,
			FixtureFormat: null!,
			PlayEachOther: null,
			Deleted: null!,
			BaseRank: null!,
			PlayAll: null!,
			SplitRounds: null!,
			MiniStartingRound: null!,
			MiniDivision: false,
			PreviousLinkId: null!,
			PreviousLink: null!,
			HalfGeneration: null!,
			TableNos: string.Empty,
			TableAllocation: null,
			HasEntrants: false,
			EntrantCount: division.Teams.Count,
			Days: []
		);
	}

	public static TTLeaguesLeague ToTTLeaguesLeague(this TT365League league)
	{
		return new TTLeaguesLeague(
			TTInfoId: league.Id.ToString(),
			TenantsHost: null,
			WebsitesHost: null,
			CurrentCompetitions: [],
			ArchivedCompetitions: [] // Needs mapping from TT365 seasons
		);
	}

	public static TTLeaguesMatch ToTTLeaguesMatch(this TT365Fixture fixture, int homeId = 0, int awayId = 0, int leagueId = 0, int divisionId = 0, int competitionId = 0)
	{
		return new TTLeaguesMatch(
			Id: 0,
			Home: new TTLeaguesMatchTeamInfo(
				Id: homeId,
				UserId: string.Empty,
				TeamId: homeId,
				Name: fixture.HomeTeam,
				ShortName: fixture.HomeTeam,
				Members: null,
				Score: null,
				Reserves: null,
				Type: 0,
				Points: null,
				Captain: string.Empty,
				ClubId: null,
				CaptainId: string.Empty,
				DisplayName: fixture.HomeTeam
			),
			Away: new TTLeaguesMatchTeamInfo(
				Id: awayId,
				UserId: string.Empty,
				TeamId: awayId,
				Name: fixture.AwayTeam,
				ShortName: fixture.AwayTeam,
				Members: null,
				Score: null,
				Reserves: null,
				Type: 0,
				Points: null,
				Captain: string.Empty,
				ClubId: null,
				CaptainId: string.Empty,
				DisplayName: fixture.AwayTeam
			),
			Date: fixture.Date.ToDateTime(TimeOnly.MinValue),
			Time: null,
			Week: null,
			Name: fixture.Description,
			Venue: fixture.Venue,
			GroupingId: null,
			CompetitionId: competitionId,
			DivisionId: divisionId,
			PreviousLinkId: null,
			Submitted: null,
			Approved: null,
			Rejected: null,
			Overridden: null,
			SubmittedBy: null,
			ApprovedBy: null,
			RejectedBy: null,
			OverriddenBy: null,
			VenueId: null,
			Forfeit: null,
			ForfeitReason: string.Empty,
			ForfeitId: null,
			AbandonedReason: string.Empty,
			Abandoned: null,
			LeagueId: leagueId,
			ClubId: null,
			CountyId: null,
			Competition: string.Empty,
			Updated: DateTimeOffset.MinValue,
			Manual: false,
			Published: null,
			Archived: null,
			Potm: null,
			PotmType: null,
			Entry: 0,
			HasReport: false,
			HasComments: false,
			ResultDisplay: 0,
			PlayAll: false,
			HasResults: false,
			ScoreUp: 0,
			Round: null,
			TableNo: null,
			DayId: null,
			HomeScores: [],
			AwayScores: [],
			HomeName: fixture.HomeTeam,
			AwayName: fixture.AwayTeam,
			Number: null,
			Stage: null,
			RoundModel: null,
			HomeHandicap: null,
			AwayHandicap: null,
			Bye: null
		);
	}

	public static TTLeaguesMatchPlayer ToTTLeaguesMatchPlayer(this Smab.TTInfo.TT365.Models.TT365.MatchPlayer matchPlayer)
	{
		return new TTLeaguesMatchPlayer(
			EntrantId: null,
			UserId: string.Empty,
			Ordering: null,
			Name: matchPlayer.Name,
			PrintoutName: matchPlayer.Name,
			Id: matchPlayer.Id,
			Fixed: null,
			PlayerId: matchPlayer.Id,
			Forfeit: null!,
			ForfeitReason: string.Empty,
			Type: null,
			Scratch: null!,
			ScratchType: null!,
			ScratchReason: string.Empty,
			TeamId: null,
			MembershipNo: null!,
			DoublesPairOrdering: null
		);
	}

	public static TTLeaguesMatchTeamInfo ToTTLeaguesMatchTeamInfo(this TT365Team team)
	{
		return new TTLeaguesMatchTeamInfo(
			Id: int.TryParse(team.Id, out int id) ? id : 0,
			UserId: string.Empty,
			TeamId: int.TryParse(team.Id, out int tid) ? tid : null,
			Name: team.Name,
			ShortName: team.ShortName,
			Members: null,
			Score: null,
			Reserves: null,
			Type: 0,
			Points: null,
			Captain: team.Captain,
			ClubId: team.ClubId,
			CaptainId: string.Empty,
			DisplayName: team.Name
		);
	}

	public static TTLeaguesPlayerResult ToTTLeaguesPlayerResult(this Smab.TTInfo.TT365.Models.TT365.PlayerResult result)
	{
		return new TTLeaguesPlayerResult(
			Team: new TTLeaguesIntKeyValue(0, result.PlayerTeamName),
			Results: [],
			Opponent: new TTLeaguesStringKeyValue(result.Opponent.Name, result.Opponent.Id.ToString()),
			Date: new DateTimeOffset(result.Date.ToDateTime(TimeOnly.MinValue)),
			Games: result.Games.Select(g => new TTLeaguesPlayerGame(g.Score1, g.Score2)).ToImmutableList(),
			Type: 0,
			SetId: result.Id,
			For: result.GameScore.Score1,
			Against: result.GameScore.Score2,
			Versus: new TTLeaguesIntKeyValue(result.Opponent.Id, result.Opponent.Name),
			MatchId: result.Id,
			DivisionId: 0,
			CompetitionId: 0,
			Score: null,
			Rank: result.RankingDiff
		);
	}

	public static TTLeaguesPlayerStats ToTTLeaguesPlayerStats(this Player player)
	{
		return new TTLeaguesPlayerStats(
			Id: player.Id.ToString(),
			Competition: new TTLeaguesIntKeyValue(0, string.Empty),
			Division: new TTLeaguesIntKeyValue(0, string.Empty),
			Team: new TTLeaguesIntKeyValue(0, string.Empty),
			Club: new TTLeaguesIntKeyValue(0, string.Empty),
			Results: player.PlayerResults.Select(r => r.ToTTLeaguesPlayerResult()).ToImmutableList(),
			Average: new TTLeaguesPlayerAverage(player.Played, (int)(player.Played * player.WinPercentage / 100.0)),
			Name: player.Name,
			LocalRanking: new TTLeaguesPlayerRanking(player.LeagueRanking, null, "League"),
			NationalRanking: new TTLeaguesPlayerRanking(player.NationalRanking, null, "National"),
			Form: [],
			Rankings: []
		);
	}

	public static TTLeaguesTeam ToTTLeaguesTeam(this TT365Team team, int? divisionId = null, int? competitionId = null)
	{
		return new TTLeaguesTeam(
			Id: int.TryParse(team.Id, out int id) ? id : null,
			CompetitionId: competitionId,
			Competition: string.Empty,
			Name: team.Name,
			Club: null,
			ClubId: team.ClubId,
			Captain: null,
			ShortName: team.ShortName,
			Description: string.Empty,
			CaptainId: string.Empty,
			Night: null,
			Time: null,
			VenueId: null,
			Play: null,
			Venue: null,
			BadgeId: null,
			Abbreviation: string.Empty,
			Ordering: null,
			ReservesOverride: null,
			Locked: null,
			Deleted: null,
			DisplayName: team.Name,
			RetentionId: null,
			Pool: null,
			Members: []
		);
	}

	public static TTLeaguesTeamStatsPlayer ToTTLeaguesTeamStatsPlayer(this Player player)
	{
		return new TTLeaguesTeamStatsPlayer(
			Id: player.Id.ToString(),
			Matches: [],
			Average: player.WinPercentage,
			Name: player.Name,
			Played: player.Played,
			Won: (int)(player.Played * player.WinPercentage / 100.0),
			Potm: int.TryParse(player.PoMAwards, out int potm) ? potm : 0
		);
	}
}
