﻿namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TeamStatsMatch(
	int? Id,
	int? Won,
	int? Played,
	int? Form,
	DateTimeOffset? Date
);
