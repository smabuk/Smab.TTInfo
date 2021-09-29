using Smab.TTInfo.Server.EndPoints;

Name = typeof(Program).Assembly.GetName().Name;
Version = typeof(Program).Assembly.GetName().Version;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<ITT365Reader, TT365Reader>(
	tt => new(
		league: builder.Configuration.GetValue<string>("TTInfo:League"),
		season: builder.Configuration.GetValue<string>("TTInfo:Season")
		)
	{
		CacheFolder = builder.Configuration.GetValue<string>("TTInfo:CacheFolder"),
		UseTestFiles = builder.Configuration.GetValue<bool?>("TTInfo:UseTestFiles") ?? builder.Environment.IsDevelopment()
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapGet("/calendar/{LeagueName}/{TeamName}", CalendarEndPoint.GetCalendarByTeam);

app.Run();


static partial class Program
{
	public static string SiteName { get; set; } = "Table Tennis Info";
	public static string? Name { get; set; }
	public static Version? Version{ get; set; }

}