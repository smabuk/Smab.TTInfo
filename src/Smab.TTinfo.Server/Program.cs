using Smab.Calendar;

using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<ITT365Reader, TT365Reader>(
	tt => new(
		league: "Reading",
		season: "Senior_2021-22"
		)
	{
		CacheFolder = @"DevData",
		UseTestFiles = builder.Environment.IsDevelopment()
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.MapGet("/calendar/{LeagueName}/{TeamName}", async (string LeagueName, string TeamName, string Command, ITT365Reader _tt365, HttpContext context) =>
{
	TeamName = TeamName.Replace("_", " ");
	FixturesView? tt365FixtureView = await _tt365.GetFixturesByTeamName(TeamName, LeagueName);
	if (tt365FixtureView is null || tt365FixtureView.Fixtures is null)
	{
		return Results.NotFound();
	}

	TimeZoneInfo gmtZone = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) switch
	{
		true => TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"),
		false => TimeZoneInfo.FindSystemTimeZoneById("GMT")
	};

	IcalCalendar ical = new IcalCalendar
	{
		Name = $"{LeagueName} fixtures - {TeamName}",
		Description = "Fixtures and results of matches for the {LeagueName} league"
	};

	ical.Events = new List<VEvent>();
	foreach (Fixture fixture in tt365FixtureView.Fixtures)
	{
		VEvent fixtureEvent = new VEvent
		{
			UID = $"RDTTA {fixture.HomeTeam} vs {fixture.AwayTeam}",
			Summary = $"🏓 {fixture.HomeTeam} vs {fixture.AwayTeam}",
			Location = fixture.Venue,
			DateStart = TimeZoneInfo.ConvertTimeToUtc(fixture.Date, gmtZone).AddHours(19).AddMinutes(30), // All matches by default start at 7:30pm
			DateEnd = TimeZoneInfo.ConvertTimeToUtc(fixture.Date, gmtZone).AddHours(22).AddMinutes(30),
			Priority = VEvent.PriorityLevel.Normal,
			Transparency = VEvent.TransparencyType.TRANSPARENT,
			Categories = "Table tennis,OLOP Table Tennis Club",
			Description = $"\n"
		};

		if (fixture.Venue.ToUpperInvariant().Contains("CURZON")
			|| fixture.Venue.ToUpperInvariant().Contains("RBL")) // 7pm start time
		{
			fixtureEvent.DateStart = fixtureEvent.DateStart.AddMinutes(-30);
		}

		if (!string.IsNullOrEmpty(TeamName)) // If looking at a particular team add BUSY and 1hr REMINDER
		{
			fixtureEvent.Transparency = VEvent.TransparencyType.OPAQUE;
			fixtureEvent.Alarms = new List<VAlarm>
				{
					new VAlarm
					{
						Trigger = new System.TimeSpan(0, 0, 60, 0),
						Action = VAlarm.ActionType.DISPLAY,
						Description = "Reminder"
					}
				};
		}

		if (fixture.IsCompleted)
		{
			if (fixture.ForHome > fixture.ForAway)
			{
				fixtureEvent.Description += $"\nWIN:  {fixture.HomeTeam.ToUpper()}";
				fixtureEvent.Description += $"\nLOSS: {fixture.AwayTeam}";
			}
			else if (fixture.ForHome < fixture.ForAway)
			{
				fixtureEvent.Description += $"\nLOSS: {fixture.HomeTeam}";
				fixtureEvent.Description += $"\nWIN:  {fixture.AwayTeam.ToUpper()}";
			}
			else if (fixture.ForHome == fixture.ForAway)
			{
				fixtureEvent.Description += $"\nDRAW: {fixture.HomeTeam} and {fixture.AwayTeam}";
			}
			fixtureEvent.Description += $"\nScore: {fixture.Score}";
		}

		ical.Events.Add(fixtureEvent);
	}

	// Different ways of returning the information
	switch (Command?.ToUpper())
	{
		case "TEXT":
			return Results.Content(ical.ToString(), "text/plain");
		case "CONTENT":
			context.Response.Headers.Add("content-disposition", $"inline;filename={LeagueName}Fixtures.ics");
			return Results.Content(ical.ToString(), "text/calendar", System.Text.Encoding.UTF8);
		case "FILE":
			return Results.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName}Fixtures.ics");
		case "JSON":
			return Results.Json(ical);
		case "NEG":
			return Results.Content(ical.ToString());
		default:
			break;
	}

	return Results.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName}Fixtures.ics");


});

app.Run();
