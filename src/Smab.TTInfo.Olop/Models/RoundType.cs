using static Smab.TTInfo.Olop.Models.RoundType;

namespace Smab.TTInfo.Olop.Models;

public enum RoundType
{
	Roundof32 = 1,
	Roundof16 = 2,
	QuarterFinal = 3,
	SemiFinal = 4,
	Final = 5
}

public static partial class RoundTypeExtensions
{
	extension(RoundType roundType)
	{
		public int ToRoundNo() => roundType switch
		{
			Roundof32 => 1,
			Roundof16 => 2,
			QuarterFinal => 3,
			SemiFinal => 4,
			Final => 5,
			_ => throw new ArgumentOutOfRangeException(nameof(roundType), roundType, null)
		};

		public int ToMatchCount() => roundType switch
		{
			Roundof32 => 16,
			Roundof16 => 8,
			QuarterFinal => 4,
			SemiFinal => 2,
			Final => 1,
			_ => throw new ArgumentOutOfRangeException(nameof(roundType), roundType, null)
		};

		public string ToDisplayString() => roundType switch
		{
			Roundof32 => "Round of 32",
			Roundof16 => "Round of 16",
			QuarterFinal => "Quarter Final",
			SemiFinal => "Semi Final",
			Final => "Final",
			_ => throw new ArgumentOutOfRangeException(nameof(roundType), roundType, null)
		};
	}
}
