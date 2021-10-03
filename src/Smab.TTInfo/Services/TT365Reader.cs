using HtmlAgilityPack;

namespace Smab.TTInfo;

public partial class TT365Reader : ITT365Reader
{
	public string League { get; set; }
	public string Season { get; set; }
	public bool UseTestFiles { get; set; } = false;
	public string CacheFolder = @"Cache";
	public int CacheHours = 12;

	System.Globalization.CultureInfo gbCulture = new("en-GB");

	public TT365Reader(string league, string season)
	{
		ArgumentNullException.ThrowIfNull(league);
		ArgumentNullException.ThrowIfNull(season);
		League = league;
		Season = season;
	}


	private class TeamInfo
	{
		public int Id { get; set; }
		public string Name { get; set; } = "";
		public int ClubId { get; set; }
		public string Division { get; set; } = "";
	}

	private Dictionary<string, TeamInfo> GetTeamInfoForSeason(string season)
	{
		return season switch
		{
			"Senior_2017-18" => new()
			{
				{
					"Curzon Club A",
					new TeamInfo() { Id = 40015, ClubId = 1174, Name = "Curzon Club A", Division = "Division 2" }
				},
				{
					"Curzon Club B",
					new TeamInfo() { Id = 40036, ClubId = 1174, Name = "Curzon Club B", Division = "Division 4" }
				},
				{
					"Curzon Club C",
					new TeamInfo() { Id = 40039, ClubId = 1174, Name = "Curzon Club C", Division = "Division 4" }
				},
				{
					"Kingfisher A",
					new TeamInfo() { Id = 40010, ClubId = 16, Name = "Kingfisher A", Division = "Division 1" }
				},
				{
					"Kingfisher B",
					new TeamInfo() { Id = 40011, ClubId = 16, Name = "Kingfisher B", Division = "Division 1" }
				},
				{
					"Kingfisher C",
					new TeamInfo() { Id = 40012, ClubId = 16, Name = "Kingfisher C", Division = "Division 1" }
				},
				{
					"Kingfisher D",
					new TeamInfo() { Id = 40013, ClubId = 16, Name = "Kingfisher D", Division = "Division 1" }
				},
				{
					"Kingfisher E",
					new TeamInfo() { Id = 40022, ClubId = 16, Name = "Kingfisher E", Division = "Division 1" }
				},
				{
					"Kingfisher F",
					new TeamInfo() { Id = 40023, ClubId = 16, Name = "Kingfisher F", Division = "Division 2" }
				},
				{
					"Kingfisher G",
					new TeamInfo() { Id = 40024, ClubId = 16, Name = "Kingfisher G", Division = "Division 2" }
				},
				{
					"Kingfisher H",
					new TeamInfo() { Id = 40032, ClubId = 16, Name = "Kingfisher H", Division = "Division 3" }
				},
				{
					"Kingfisher I",
					new TeamInfo() { Id = 40033, ClubId = 16, Name = "Kingfisher I", Division = "Division 3" }
				},
				{
					"Kingfisher J",
					new TeamInfo() { Id = 40034, ClubId = 16, Name = "Kingfisher J", Division = "Division 3" }
				},
				{
					"Kingfisher K",
					new TeamInfo() { Id = 40044, ClubId = 16, Name = "Kingfisher K", Division = "Division 4" }
				},
				{
					"Kingfisher L",
					new TeamInfo() { Id = 40053, ClubId = 16, Name = "Kingfisher L", Division = "Division 5" }
				},
				{
					"Milestone A",
					new TeamInfo() { Id = 40026, ClubId = 1698, Name = "Milestone A", Division = "Division 3" }
				},
				{
					"Milestone B",
					new TeamInfo() { Id = 40037, ClubId = 1698, Name = "Milestone B", Division = "Division 3" }
				},
				{
					"Milestone C",
					new TeamInfo() { Id = 40047, ClubId = 1698, Name = "Milestone C", Division = "Division 5" }
				},
				{
					"Our Lady Of Peace A",
					new TeamInfo() { Id = 40008, ClubId = 411, Name = "Our Lady Of Peace A", Division = "Division 1" }
				},
				{
					"Our Lady Of Peace B",
					new TeamInfo() { Id = 40009, ClubId = 411, Name = "Our Lady Of Peace B", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace C",
					new TeamInfo() { Id = 40021, ClubId = 411, Name = "Our Lady Of Peace C", Division = "Division 3" }
				},
				{
					"Our Lady Of Peace D",
					new TeamInfo() { Id = 40031, ClubId = 411, Name = "Our Lady Of Peace D", Division = "Division 4" }
				},
				{
					"Our Lady Of Peace E",
					new TeamInfo() { Id = 40043, ClubId = 411, Name = "Our Lady Of Peace E", Division = "Division 4" }
				},
				{
					"Our Lady of Peace F",
					new TeamInfo() { Id = 40052, ClubId = 411, Name = "Our Lady of Peace F", Division = "Division 5" }
				},
				{
					"Pangbourne WMC",
					new TeamInfo() { Id = 40014, ClubId = 1839, Name = "Pangbourne WMC", Division = "Division 2" }
				},
				{
					"Reading FC A - Community Trust",
					new TeamInfo() { Id = 40035, ClubId = 1957, Name = "Reading FC A - Community Trust", Division = "Division 4" }
				},
				{
					"Reading FC B - Biscuit Men",
					new TeamInfo() { Id = 40045, ClubId = 1957, Name = "Reading FC B - Biscuit Men", Division = "Division 5" }
				},
				{
					"Saiyan Brothers",
					new TeamInfo() { Id = 40062, ClubId = 4975, Name = "Saiyan Brothers", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard A",
					new TeamInfo() { Id = 40006, ClubId = 2124, Name = "Sonning Common and Peppard A", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard B",
					new TeamInfo() { Id = 40007, ClubId = 2124, Name = "Sonning Common and Peppard B", Division = "Division 2" }
				},
				{
					"Sonning Common and Peppard C",
					new TeamInfo() { Id = 40018, ClubId = 2124, Name = "Sonning Common and Peppard C", Division = "Division 2" }
				},
				{
					"Sonning Common and Peppard D",
					new TeamInfo() { Id = 40019, ClubId = 2124, Name = "Sonning Common and Peppard D", Division = "Division 2" }
				},
				{
					"Sonning Common And Peppard E",
					new TeamInfo() { Id = 40020, ClubId = 2124, Name = "Sonning Common And Peppard E", Division = "Division 3" }
				},
				{
					"Sonning Common and Peppard F",
					new TeamInfo() { Id = 40042, ClubId = 2124, Name = "Sonning Common and Peppard F", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard G",
					new TeamInfo() { Id = 40051, ClubId = 2124, Name = "Sonning Common and Peppard G", Division = "Division 5" }
				},
				{
					"Sonning Sports A",
					new TeamInfo() { Id = 40025, ClubId = 2125, Name = "Sonning Sports A", Division = "Division 3" }
				},
				{
					"Sonning Sports B",
					new TeamInfo() { Id = 40046, ClubId = 2125, Name = "Sonning Sports B", Division = "Division 5" }
				},
				{
					"Springfield A",
					new TeamInfo() { Id = 40027, ClubId = 2146, Name = "Springfield A", Division = "Division 3" }
				},
				{
					"Springfield B",
					new TeamInfo() { Id = 40038, ClubId = 2146, Name = "Springfield B", Division = "Division 4" }
				},
				{
					"Springfield C",
					new TeamInfo() { Id = 40048, ClubId = 2146, Name = "Springfield C", Division = "Division 5" }
				},
				{
					"Springfield D",
					new TeamInfo() { Id = 40049, ClubId = 2146, Name = "Springfield D", Division = "Division 5" }
				},
				{
					"Tidmarsh A",
					new TeamInfo() { Id = 40005, ClubId = 599, Name = "Tidmarsh A", Division = "Division 1" }
				},
				{
					"Tidmarsh B",
					new TeamInfo() { Id = 40017, ClubId = 599, Name = "Tidmarsh B", Division = "Division 2" }
				},
				{
					"Tidmarsh C",
					new TeamInfo() { Id = 40030, ClubId = 599, Name = "Tidmarsh C", Division = "Division 2" }
				},
				{
					"Tidmarsh D",
					new TeamInfo() { Id = 40041, ClubId = 599, Name = "Tidmarsh D", Division = "Division 4" }
				},
				{
					"Tidmarsh E",
					new TeamInfo() { Id = 40050, ClubId = 599, Name = "Tidmarsh E", Division = "Division 5" }
				},
				{
					"Tilehurst Methodists A",
					new TeamInfo() { Id = 40016, ClubId = 2327, Name = "Tilehurst Methodists A", Division = "Division 2" }
				},
				{
					"Tilehurst Methodists B",
					new TeamInfo() { Id = 40028, ClubId = 2327, Name = "Tilehurst Methodists B", Division = "Division 3" }
				},
				{
					"Tilehurst Methodists C",
					new TeamInfo() { Id = 40029, ClubId = 2327, Name = "Tilehurst Methodists C", Division = "Division 4" }
				},
				{
					"Tilehurst Methodists D",
					new TeamInfo() { Id = 40040, ClubId = 2327, Name = "Tilehurst Methodists D", Division = "Division 5" }
				}
			},
			"Senior_2018-19" => new()
			{
				{
					"Curzon Club A",
					new TeamInfo() { Id = 48249, ClubId = 1174, Name = "Curzon Club A", Division = "Division 1" }
				},
				{
					"Curzon Club B",
					new TeamInfo() { Id = 48286, ClubId = 1174, Name = "Curzon Club B", Division = "Division 2" }
				},
				{
					"Curzon Club C",
					new TeamInfo() { Id = 48270, ClubId = 1174, Name = "Curzon Club C", Division = "Division 3" }
				},
				{
					"Kennet Valley Free Church A",
					new TeamInfo() { Id = 48284, ClubId = 5378, Name = "Kennet Valley Free Church A", Division = "Division 1" }
				},
				{
					"Kennet Valley Free Church B",
					new TeamInfo() { Id = 48293, ClubId = 5378, Name = "Kennet Valley Free Church B", Division = "Division 4" }
				},
				{
					"Kingfisher A",
					new TeamInfo() { Id = 48243, ClubId = 16, Name = "Kingfisher A", Division = "Division 1" }
				},
				{
					"Kingfisher B",
					new TeamInfo() { Id = 48244, ClubId = 16, Name = "Kingfisher B", Division = "Division 1" }
				},
				{
					"Kingfisher C",
					new TeamInfo() { Id = 48245, ClubId = 16, Name = "Kingfisher C", Division = "Division 1" }
				},
				{
					"Kingfisher D",
					new TeamInfo() { Id = 48246, ClubId = 16, Name = "Kingfisher D", Division = "Division 1" }
				},
				{
					"Kingfisher E",
					new TeamInfo() { Id = 48247, ClubId = 16, Name = "Kingfisher E", Division = "Division 2" }
				},
				{
					"Kingfisher F",
					new TeamInfo() { Id = 48257, ClubId = 16, Name = "Kingfisher F", Division = "Division 2" }
				},
				{
					"Kingfisher G",
					new TeamInfo() { Id = 48265, ClubId = 16, Name = "Kingfisher G", Division = "Division 3" }
				},
				{
					"Kingfisher H",
					new TeamInfo() { Id = 48266, ClubId = 16, Name = "Kingfisher H", Division = "Division 3" }
				},
				{
					"Kingfisher I",
					new TeamInfo() { Id = 48267, ClubId = 16, Name = "Kingfisher I", Division = "Division 4" }
				},
				{
					"Milestone A",
					new TeamInfo() { Id = 48285, ClubId = 1698, Name = "Milestone A", Division = "Division 1" }
				},
				{
					"Milestone B - Saiyan Brothers",
					new TeamInfo() { Id = 48260, ClubId = 1698, Name = "Milestone B - Saiyan Brothers", Division = "Division 2" }
				},
				{
					"Milestone C",
					new TeamInfo() { Id = 48635, ClubId = 1698, Name = "Milestone C", Division = "Division 4" }
				},
				{
					"Our Lady Of Peace A",
					new TeamInfo() { Id = 48242, ClubId = 411, Name = "Our Lady Of Peace A", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace B",
					new TeamInfo() { Id = 48256, ClubId = 411, Name = "Our Lady Of Peace B", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace C",
					new TeamInfo() { Id = 48264, ClubId = 411, Name = "Our Lady Of Peace C", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace D",
					new TeamInfo() { Id = 48275, ClubId = 411, Name = "Our Lady Of Peace D", Division = "Division 3" }
				},
				{
					"Our Lady Of Peace E",
					new TeamInfo() { Id = 48276, ClubId = 411, Name = "Our Lady Of Peace E", Division = "Division 4" }
				},
				{
					"Pangbourne WMC",
					new TeamInfo() { Id = 48248, ClubId = 1839, Name = "Pangbourne WMC", Division = "Division 2" }
				},
				{
					"Reading FC A - Community Trust",
					new TeamInfo() { Id = 48268, ClubId = 1957, Name = "Reading FC A - Community Trust", Division = "Division 3" }
				},
				{
					"Reading FC B - Biscuit Men",
					new TeamInfo() { Id = 48277, ClubId = 1957, Name = "Reading FC B - Biscuit Men", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard A",
					new TeamInfo() { Id = 48241, ClubId = 2124, Name = "Sonning Common and Peppard A", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard B",
					new TeamInfo() { Id = 48253, ClubId = 2124, Name = "Sonning Common and Peppard B", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard C",
					new TeamInfo() { Id = 48254, ClubId = 2124, Name = "Sonning Common and Peppard C", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard D",
					new TeamInfo() { Id = 48255, ClubId = 2124, Name = "Sonning Common and Peppard D", Division = "Division 2" }
				},
				{
					"Sonning Common And Peppard E",
					new TeamInfo() { Id = 48634, ClubId = 2124, Name = "Sonning Common And Peppard E", Division = "Division 3" }
				},
				{
					"Sonning Common and Peppard F",
					new TeamInfo() { Id = 48274, ClubId = 2124, Name = "Sonning Common and Peppard F", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard G",
					new TeamInfo() { Id = 48283, ClubId = 2124, Name = "Sonning Common and Peppard G", Division = "Division 4" }
				},
				{
					"Sonning Sports A",
					new TeamInfo() { Id = 40025, ClubId = 48258, Name = "Sonning Sports A", Division = "Division 2" }
				},
				{
					"Sonning Sports B",
					new TeamInfo() { Id = 40046, ClubId = 48290, Name = "Sonning Sports B", Division = "Division 3" }
				},
				{
					"Sonning Sports C",
					new TeamInfo() { Id = 40046, ClubId = 48294, Name = "Sonning Sports C", Division = "Division 4" }
				},
				{
					"Springfield A",
					new TeamInfo() { Id = 48261, ClubId = 2146, Name = "Springfield A", Division = "Division 2" }
				},
				{
					"Springfield B",
					new TeamInfo() { Id = 48271, ClubId = 2146, Name = "Springfield B", Division = "Division 3" }
				},
				{
					"Springfield C",
					new TeamInfo() { Id = 48288, ClubId = 2146, Name = "Springfield C", Division = "Division 3" }
				},
				{
					"Springfield D",
					new TeamInfo() { Id = 48281, ClubId = 2146, Name = "Springfield D", Division = "Division 4" }
				},
				{
					"Springfield E",
					new TeamInfo() { Id = 48292, ClubId = 2146, Name = "Springfield E", Division = "Division 4" }
				},
				{
					"Tidmarsh A",
					new TeamInfo() { Id = 48240, ClubId = 599, Name = "Tidmarsh A", Division = "Division 1" }
				},
				{
					"Tidmarsh B",
					new TeamInfo() { Id = 48251, ClubId = 599, Name = "Tidmarsh B", Division = "Division 1" }
				},
				{
					"Tidmarsh C",
					new TeamInfo() { Id = 48289, ClubId = 599, Name = "Tidmarsh C", Division = "Division 3" }
				},
				{
					"Tidmarsh D",
					new TeamInfo() { Id = 48273, ClubId = 599, Name = "Tidmarsh D", Division = "Division 3" }
				},
				{
					"Tidmarsh E",
					new TeamInfo() { Id = 48282, ClubId = 599, Name = "Tidmarsh E", Division = "Division 4" }
				},
				{
					"Tilehurst Methodists A",
					new TeamInfo() { Id = 48250, ClubId = 2327, Name = "Tilehurst Methodists A", Division = "Division 2" }
				},
				{
					"Tilehurst Methodists B",
					new TeamInfo() { Id = 48262, ClubId = 2327, Name = "Tilehurst Methodists B", Division = "Division 3" }
				},
				{
					"Tilehurst Methodists C",
					new TeamInfo() { Id = 48272, ClubId = 2327, Name = "Tilehurst Methodists C", Division = "Division 4" }
				}
			},
			"Senior_2019-20" => new()
			{
				{
					"Curzon Club A",
					new TeamInfo() { Id = 56825, ClubId = 1174, Name = "Curzon Club A", Division = "Division 1" }
				},
				{
					"Curzon Club B",
					new TeamInfo() { Id = 56847, ClubId = 1174, Name = "Curzon Club B", Division = "Division 3" }
				},
				{
					"Curzon Club C",
					new TeamInfo() { Id = 56852, ClubId = 1174, Name = "Curzon Club C", Division = "Division 3" }
				},
				{
					"Kennet Valley Free Church A",
					new TeamInfo() { Id = 56826, ClubId = 5378, Name = "Kennet Valley Free Church A", Division = "Division 1" }
				},
				{
					"Kennet Valley Free Church B",
					new TeamInfo() { Id = 56860, ClubId = 5378, Name = "Kennet Valley Free Church B", Division = "Division 4" }
				},
				{
					"Kingfisher A",
					new TeamInfo() { Id = 56827, ClubId = 16, Name = "Kingfisher A", Division = "Division 1" }
				},
				{
					"Kingfisher B",
					new TeamInfo() { Id = 56830, ClubId = 16, Name = "Kingfisher B", Division = "Division 1" }
				},
				{
					"Kingfisher C",
					new TeamInfo() { Id = 56833, ClubId = 16, Name = "Kingfisher C", Division = "Division 1" }
				},
				{
					"Kingfisher D",
					new TeamInfo() { Id = 56835, ClubId = 16, Name = "Kingfisher D", Division = "Division 2" }
				},
				{
					"Kingfisher E",
					new TeamInfo() { Id = 56837, ClubId = 16, Name = "Kingfisher E", Division = "Division 2" }
				},
				{
					"Kingfisher F",
					new TeamInfo() { Id = 56842, ClubId = 16, Name = "Kingfisher F", Division = "Division 3" }
				},
				{
					"Kingfisher G",
					new TeamInfo() { Id = 56857, ClubId = 16, Name = "Kingfisher G", Division = "Division 3" }
				},
				{
					"Kingfisher H",
					new TeamInfo() { Id = 56858, ClubId = 16, Name = "Kingfisher H", Division = "Division 4" }
				},
				{
					"Milestone A",
					new TeamInfo() { Id = 56870, ClubId = 1698, Name = "Milestone A", Division = "Division 2" }
				},
				{
					"Milestone B",
					new TeamInfo() { Id = 56844, ClubId = 1698, Name = "Milestone B", Division = "Division 3" }
				},
				{
					"Milestone C",
					new TeamInfo() { Id = 56868, ClubId = 1698, Name = "Milestone C", Division = "Division 4" }
				},
				{
					"Our Lady Of Peace A",
					new TeamInfo() { Id = 56836, ClubId = 411, Name = "Our Lady Of Peace A", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace B",
					new TeamInfo() { Id = 56841, ClubId = 411, Name = "Our Lady Of Peace B", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace C",
					new TeamInfo() { Id = 56846, ClubId = 411, Name = "Our Lady Of Peace C", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace D",
					new TeamInfo() { Id = 56853, ClubId = 411, Name = "Our Lady Of Peace D", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace E",
					new TeamInfo() { Id = 56864, ClubId = 411, Name = "Our Lady Of Peace E", Division = "Division 3" }
				},
				{
					"Our Lady Of Peace F",
					new TeamInfo() { Id = 56871, ClubId = 411, Name = "Our Lady Of Peace F", Division = "Division 4" }
				},
				{
					"Our Lady Of Peace Premier",
					new TeamInfo() { Id = 56869, ClubId = 411, Name = "Our Lady Of Peace Premier", Division = "Division 1" }
				},
				{
					"Pangbourne WMC",
					new TeamInfo() { Id = 56838, ClubId = 1839, Name = "Pangbourne WMC", Division = "Division 2" }
				},
				{
					"Reading FC A - The Royals",
					new TeamInfo() { Id = 56848, ClubId = 1957, Name = "Reading FC A - The Royals", Division = "Division 3" }
				},
				{
					"Reading FC B - Biscuit Men",
					new TeamInfo() { Id = 56861, ClubId = 1957, Name = "Reading FC B - Biscuit Men", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard A",
					new TeamInfo() { Id = 56828, ClubId = 2124, Name = "Sonning Common and Peppard A", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard B",
					new TeamInfo() { Id = 56831, ClubId = 2124, Name = "Sonning Common and Peppard B", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard C",
					new TeamInfo() { Id = 56834, ClubId = 2124, Name = "Sonning Common and Peppard C", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard D",
					new TeamInfo() { Id = 56840, ClubId = 2124, Name = "Sonning Common and Peppard D", Division = "Division 2" }
				},
				{
					"Sonning Common And Peppard E",
					new TeamInfo() { Id = 56859, ClubId = 2124, Name = "Sonning Common And Peppard E", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard F",
					new TeamInfo() { Id = 56866, ClubId = 2124, Name = "Sonning Common and Peppard F", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard G",
					new TeamInfo() { Id = 56867, ClubId = 2124, Name = "Sonning Common and Peppard G", Division = "Division 4" }
				},
				{
					"Sonning Sports A",
					new TeamInfo() { Id = 56843, ClubId = 48258, Name = "Sonning Sports A", Division = "Division 2" }
				},
				{
					"Sonning Sports B",
					new TeamInfo() { Id = 56849, ClubId = 48290, Name = "Sonning Sports B", Division = "Division 3" }
				},
				{
					"Sonning Sports C",
					new TeamInfo() { Id = 56862, ClubId = 48294, Name = "Sonning Sports C", Division = "Division 4" }
				},
				{
					"Springfield A",
					new TeamInfo() { Id = 56845, ClubId = 2146, Name = "Springfield A", Division = "Division 1" }
				},
				{
					"Springfield B",
					new TeamInfo() { Id = 56850, ClubId = 2146, Name = "Springfield B", Division = "Division 2" }
				},
				{
					"Springfield C",
					new TeamInfo() { Id = 56855, ClubId = 2146, Name = "Springfield C", Division = "Division 3" }
				},
				{
					"Springfield D",
					new TeamInfo() { Id = 56863, ClubId = 2146, Name = "Springfield D", Division = "Division 4" }
				},
				{
					"Tidmarsh A",
					new TeamInfo() { Id = 56829, ClubId = 599, Name = "Tidmarsh A", Division = "Division 1" }
				},
				{
					"Tidmarsh B",
					new TeamInfo() { Id = 56832, ClubId = 599, Name = "Tidmarsh B", Division = "Division 1" }
				},
				{
					"Tidmarsh C",
					new TeamInfo() { Id = 56856, ClubId = 599, Name = "Tidmarsh C", Division = "Division 3" }
				},
				{
					"Tidmarsh D",
					new TeamInfo() { Id = 56854, ClubId = 599, Name = "Tidmarsh D", Division = "Division 3" }
				},
				{
					"Tidmarsh E",
					new TeamInfo() { Id = 56865, ClubId = 599, Name = "Tidmarsh E", Division = "Division 4" }
				},
				{
					"Tilehurst Methodists A",
					new TeamInfo() { Id = 56839, ClubId = 2327, Name = "Tilehurst Methodists A", Division = "Division 2" }
				},
				{
					"Tilehurst Methodists B",
					new TeamInfo() { Id = 56851, ClubId = 2327, Name = "Tilehurst Methodists B", Division = "Division 3" }
				}
			},
			"Senior_2021-22" => new()
			{
				{
					"Kingfisher A",
					new TeamInfo() { Id = 60297, ClubId = 16, Name = "Kingfisher A", Division = "Division 1" }
				},
				{
					"Kingfisher B",
					new TeamInfo() { Id = 60300, ClubId = 16, Name = "Kingfisher B", Division = "Division 1" }
				},
				{
					"Kingfisher C",
					new TeamInfo() { Id = 60303, ClubId = 16, Name = "Kingfisher C", Division = "Division 1" }
				},
				{
					"Kingfisher D",
					new TeamInfo() { Id = 60312, ClubId = 16, Name = "Kingfisher D", Division = "Division 1" }
				},
				{
					"Kingfisher E",
					new TeamInfo() { Id = 60315, ClubId = 16, Name = "Kingfisher E", Division = "Division 2" }
				},
				{
					"Kingfisher F",
					new TeamInfo() { Id = 60323, ClubId = 16, Name = "Kingfisher F", Division = "Division 2" }
				},
				{
					"Kingfisher G",
					new TeamInfo() { Id = 60324, ClubId = 16, Name = "Kingfisher G", Division = "Division 2" }
				},
				{
					"Kingfisher H",
					new TeamInfo() { Id = 60330, ClubId = 16, Name = "Kingfisher H", Division = "Division 3" }
				},
				{
					"Kingfisher I",
					new TeamInfo() { Id = 60334, ClubId = 16, Name = "Kingfisher I", Division = "Division 3" }
				},
				{
					"Kingfisher J",
					new TeamInfo() { Id = 60338, ClubId = 16, Name = "Kingfisher J", Division = "Division 4" }
				},
				{
					"Milestone A",
					new TeamInfo() { Id = 60310, ClubId = 1698, Name = "Milestone A", Division = "Division 3" }
				},
				{
					"Milestone B",
					new TeamInfo() { Id = 60337, ClubId = 1698, Name = "Milestone B", Division = "Division 4" }
				},
				{
					"Our Lady Of Peace A",
					new TeamInfo() { Id = 60305, ClubId = 411, Name = "Our Lady Of Peace A", Division = "Division 1" }
				},
				{
					"Our Lady Of Peace B",
					new TeamInfo() { Id = 60309, ClubId = 411, Name = "Our Lady Of Peace B", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace C",
					new TeamInfo() { Id = 60311, ClubId = 411, Name = "Our Lady Of Peace C", Division = "Division 2" }
				},
				{
					"Our Lady Of Peace D",
					new TeamInfo() { Id = 60313, ClubId = 411, Name = "Our Lady Of Peace D", Division = "Division 3" }
				},
				{
					"Our Lady Of Peace E",
					new TeamInfo() { Id = 60322, ClubId = 411, Name = "Our Lady Of Peace E", Division = "Division 3" }
				},
				{
					"Our Lady Of Peace F",
					new TeamInfo() { Id = 60329, ClubId = 411, Name = "Our Lady Of Peace F", Division = "Division 4" }
				},
				{
					"Pangbourne WMC",
					new TeamInfo() { Id = 60306, ClubId = 1839, Name = "Pangbourne WMC", Division = "Division 3" }
				},
				{
					"Reading FC A - The Royals",
					new TeamInfo() { Id = 60317, ClubId = 1957, Name = "Reading FC A - The Royals", Division = "Division 2" }
				},
				{
					"Reading FC B - Biscuit Men",
					new TeamInfo() { Id = 60325, ClubId = 1957, Name = "Reading FC B - Biscuit Men", Division = "Division 4" }
				},
				{
					"Sonning Common and Peppard A",
					new TeamInfo() { Id = 60298, ClubId = 2124, Name = "Sonning Common and Peppard A", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard B",
					new TeamInfo() { Id = 60301, ClubId = 2124, Name = "Sonning Common and Peppard B", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard C",
					new TeamInfo() { Id = 60304, ClubId = 2124, Name = "Sonning Common and Peppard C", Division = "Division 1" }
				},
				{
					"Sonning Common and Peppard D",
					new TeamInfo() { Id = 60314, ClubId = 2124, Name = "Sonning Common and Peppard D", Division = "Division 2" }
				},
				{
					"Sonning Common And Peppard E",
					new TeamInfo() { Id = 60327, ClubId = 2124, Name = "Sonning Common And Peppard E", Division = "Division 3" }
				},
				{
					"Sonning Common and Peppard F",
					new TeamInfo() { Id = 60328, ClubId = 2124, Name = "Sonning Common and Peppard F", Division = "Division 4" }
				},
				{
					"Sonning Sports A",
					new TeamInfo() { Id = 60307, ClubId = 48258, Name = "Sonning Sports A", Division = "Division 2" }
				},
				{
					"Sonning Sports B",
					new TeamInfo() { Id = 60318, ClubId = 48290, Name = "Sonning Sports B", Division = "Division 4" }
				},
				{
					"Sonning Sports C",
					new TeamInfo() { Id = 60326, ClubId = 48294, Name = "Sonning Sports C", Division = "Division 4" }
				},
				{
					"Springfield",
					new TeamInfo() { Id = 60335, ClubId = 2146, Name = "Springfield", Division = "Division 3" }
				},
				{
					"Tidmarsh A",
					new TeamInfo() { Id = 60299, ClubId = 599, Name = "Tidmarsh A", Division = "Division 1" }
				},
				{
					"Tidmarsh B",
					new TeamInfo() { Id = 60302, ClubId = 599, Name = "Tidmarsh B", Division = "Division 2" }
				},
				{
					"Tidmarsh C",
					new TeamInfo() { Id = 60321, ClubId = 599, Name = "Tidmarsh C", Division = "Division 2" }
				},
				{
					"Tidmarsh D",
					new TeamInfo() { Id = 60320, ClubId = 599, Name = "Tidmarsh D", Division = "Division 3" }
				},
				{
					"Tilehurst Methodists A",
					new TeamInfo() { Id = 60308, ClubId = 2327, Name = "Tilehurst Methodists A", Division = "Division 2" }
				},
				{
					"Tilehurst Methodists B",
					new TeamInfo() { Id = 60319, ClubId = 2327, Name = "Tilehurst Methodists B", Division = "Division 4" }
				},
				{
					"Tilehurst RBL A",
					new TeamInfo() { Id = 60331, ClubId = 2327, Name = "Tilehurst RBL A", Division = "Division 1" }
				},
				{
					"Tilehurst RBL B",
					new TeamInfo() { Id = 60333, ClubId = 2327, Name = "Tilehurst RBL B", Division = "Division 3" }
				},
				{
					"Tilehurst RBL C",
					new TeamInfo() { Id = 60336, ClubId = 2327, Name = "Tilehurst RBL C", Division = "Division 4" }
				}
			},
			_ => throw new ArgumentException(),
		};
	}
}
