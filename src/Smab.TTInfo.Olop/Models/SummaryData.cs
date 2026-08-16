namespace Smab.TTInfo.Olop.Models;

public record SummaryData(
	string Title,
	List<int> ValidWeeks,
	Dictionary<NamedPlayer, SummaryPlayerDetails> PlayerDetails
);

public static partial class SummaryDataExtensions
{
	extension(SummaryData data)
	{
		public SummaryPlayerDetails GetSummaryPlayerDetails(NamedPlayer player) => data.PlayerDetails[player];
	}
}
