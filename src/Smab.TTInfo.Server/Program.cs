using System.Reflection;
using Smab.TTInfo.Server.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<ITT365Reader, TT365Reader>(
	tt => new()
	{
		CacheFolder = builder.Configuration.GetValue<string>("TTInfo:CacheFolder"),
		CacheHours = builder.Configuration.GetValue<int?>("TTInfo:CacheHours") ?? 6,
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

app.MapCalendarEndPoints();

app.Run();


static partial class Program
{
	public static string SiteName { get; set; } = "Table Tennis Info";
	public static string Name { get; set; } = typeof(Program).Assembly
							.GetName().Name ?? "No assembly name";
	public static Version Version { get; set; } = typeof(Program).Assembly
							.GetName().Version ?? new();
	public static string ProductVersion { get; set; } = typeof(Program).Assembly
							.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
							.InformationalVersion ?? "";

	public static string SmabTTInfoName { get; set; } = typeof(ITT365Reader).Assembly
							.GetName().Name ?? "No assembly name";
	public static string SmabTTInfoVersion { get; set; } = typeof(ITT365Reader).Assembly
							.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
							.InformationalVersion ?? "";



}