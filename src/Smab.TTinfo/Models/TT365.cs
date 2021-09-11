using System.Diagnostics;

namespace Smab.TTInfo.Models;

public class TT365Models
{
    public enum FxturesViewType
    {
        Advanced = 1,
        Simple = 2,
        Chart = 3,
        Print = 4,
        Export = 5
    }

    //public enum ShowByWeekNo
    //{
    //    WeekNo = true,
    //    MatchNo = false
    //}


	public record ReadingTeam (Team Team, FixturesView FixturesView);

    public class FixturesView
    {
        public string Caption { get; set; } = "";
		public string URL { get; set; } = "";
		public FixturesViewOptions Options { get; set; } = new();
        public ICollection<Fixture>? Fixtures { get; set; }
    }

    public class FixturesViewOptions
    {
        public string Season { get; set; } = "Senior 2021-22";
        public string Division { get; set; } = "All Divisions";
        public string ClubId { get; set; } = "";
        public string TeamId { get; set; } = "";
        public string VenueId { get; set; } = "";
        public int ViewModeType { get; set; } = (int)TT365Models.FxturesViewType.Advanced;
        public bool HideCompletedFixtures { get; set; } = false;
        public bool MergeDivisions { get; set; } = true;
        //public bool ShowByWeekNo { get; set; } = TT365Models.ShowByWeekNo.WeekNo;
    }

    [DebuggerDisplay("Title: {Title,nq}")]
    public class FixtureWeek
    {
        public string Title { get; set; } = "";
	}

    [DebuggerDisplay("Fixture: {Date,nq} - {HomeTeam,nq} vs {AwayTeam,nq}")]
    public class Fixture
    {
		public string Division { get; set; } = "";
        public string Description { get; set; } = "";
		public DateTime Date { get; set; }
        public string HomeTeam { get; set; } = "";
		public string AwayTeam { get; set; } = "";
		public string Venue { get; set; } = "";
		public bool IsCompleted { get; set; } = false;
        public int ForHome { get; set; }
        public int ForAway { get; set; }
        public string PlayerOfTheMatch { get; set; } = "";
		public string CardURL { get; set; } = "";

		public string Score
        {
            get
            {
                if (IsCompleted)
                    return ForHome + " - " + ForAway;
                return string.Empty;
            }
        }
    }

    [DebuggerDisplay("CompletedFixture: {Date,nq} - {HomeTeam,nq} ({ForHome,nq}) vs ({ForAway,nq}) {AwayTeam,nq}")]
    public class CompletedFixture : Fixture
    {
    }

    [DebuggerDisplay("Name: {Name,nq}")]
    public class Team
    {
        public int Id { get; set; }
        public string URL { get; set; } = "";
		public string Caption { get; set; } = "";
		public string Name { get; set; } = "";
		public int ClubId { get; set; }
        public string Division { get; set; } = "";
		public string Captain { get; set; } = "";
		public ICollection<Player>? Players { get; set; }
        public ICollection<Fixture>? Fixtures { get; set; }
        public ICollection<Result>? Results { get; set; }
    }

    [DebuggerDisplay("Name: {Name,nq}")]
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
		public int Played { get; set; }
        public string WinPercentage { get; set; } = "";
		public string PoMAwards { get; set; } = "";
		public string Form { get; set; } = "";
		public int ClubRanking { get; set; }
        public int LeagueRanking { get; set; }
        public int CountyRanking { get; set; }
        public int RegionalRanking { get; set; }
        public int NationalRanking { get; set; }
    }

    [DebuggerDisplay("Result: {ScoreForHome,nq} : {ScoreForAway,nq}")]
    public class Result
    {
        public int Id { get; set; }
        public string Opposition { get; set; } = "";
		public string HomeOrAway { get; set; } = "";
		public DateTime Date { get; set; }
        public string ScoreForHome { get; set; } = "";
		public string ScoreForAway { get; set; } = "";
		public int Points { get; set; }
        public string PlayerOfTheMatch { get; set; } = "";
		public string CardURL { get; set; } = "";
	}
}
