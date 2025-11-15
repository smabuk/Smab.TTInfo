using System.Reflection;

using Smab.TTInfo.Server.Components;
using Smab.TTInfo.Server.EndPoints;
using Smab.TTInfo.TT365;
using Smab.TTInfo.TTLeagues;
using Smab.TTInfo.TTClubs.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents(options =>
	{
		options.DetailedErrors = builder.Environment.IsDevelopment();
		options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
		options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
		options.MaxBufferedUnacknowledgedRenderBatches = 10;
	});

builder.Services.AddLocalization();
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();

// Add output caching for improved performance
builder.Services.AddOutputCache();

// Add response compression for static assets
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

builder.Services
	.AddTT365Service()
	.AddTTLeaguesService()
	.AddTTClubsService();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	_ = app.UseDeveloperExceptionPage();
	//app.UseWebAssemblyDebugging();
}
else
{
	_ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	_ = app.UseHsts();
	_ = app.UseResponseCompression();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.MapStaticAssets();

// Request localization should come before UseAntiforgery
app.UseRequestLocalization(
	new RequestLocalizationOptions()
		.SetDefaultCulture("en-GB")
		.AddSupportedCultures("en-GB")
		.AddSupportedUICultures(cultures)
);

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddAdditionalAssemblies(typeof(Smab.TTInfo.TT365._Imports).Assembly)
	.AddAdditionalAssemblies(typeof(Smab.TTInfo.TTClubs._Imports).Assembly)
	.AddAdditionalAssemblies(typeof(Smab.TTInfo.TTLeagues._Imports).Assembly);

app.MapHealthChecks("/healthz");

app.MapCalendarEndPoints();
app.MapGroup("/tt")
	.MapTTEndPoints();

app.Run();


static partial class Program
{
	public static string SiteName { get; set; } = "🏓 TT Info";
	public static string Name { get; set; } = typeof(Program).Assembly
							.GetName().Name ?? "No assembly name";
	public static Version Version { get; set; } = typeof(Program).Assembly
							.GetName().Version ?? new();
	public static string ProductVersion { get; set; } =
		VersionWithoutGuid(typeof(Program).Assembly
							.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
							.InformationalVersion ?? ""
		);
	public static string FrameworkVersion { get; } = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

	private static readonly string[] cultures = [.. CultureInfo
		.GetCultures(CultureTypes.AllCultures)
		.Select(c => c.Name)];

	private static string VersionWithoutGuid(string version)
	{
		int indexOfPlus = version.IndexOf('+');
		return (indexOfPlus > 0) ? version[..indexOfPlus] : version;
	}
}
