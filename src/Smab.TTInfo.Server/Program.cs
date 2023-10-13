using System.Reflection;

using Smab.TTInfo.Server.Components;
using Smab.TTInfo.Server.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
/*	.AddJsonOptions(options => options.JsonSerializerOptions.AddDateOnlyAndTimeOnlyConverters())*/
;

builder.Services.AddLocalization();
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();

builder.Services.Configure<TTInfoOptions>(builder.Configuration.GetSection("TTInfo"));

builder.Services.AddScoped<ITT365Reader, TT365Reader>();
builder.Services.AddScoped<TTLeaguesReader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	//app.UseWebAssemblyDebugging();
} else {
	_ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	_ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddAdditionalAssemblies(typeof(Smab.TTInfo.TT365.Pages.LeagueSummary).Assembly)
	.AddAdditionalAssemblies(typeof(Smab.TTInfo.TTLeagues.Pages.LeagueSummary).Assembly);

app.MapHealthChecks("/healthz");

app.UseRequestLocalization(
	new RequestLocalizationOptions()
		.SetDefaultCulture("en-GB")
		.AddSupportedCultures("en-GB")
		.AddSupportedUICultures(cultures)
	);

app.MapCalendarEndPoints();
app.MapGroup("/tt")
	.MapTTEndPoints();

app.Run();


static partial class Program
{
	public static string SiteName { get; set; } = "🏓 Info";
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

	private static readonly string[] cultures = CultureInfo
		.GetCultures(CultureTypes.AllCultures)
		.Select(c => c.Name)
		.ToArray();

	private static string VersionWithoutGuid(string version)
	{
		int indexOfPlus = version.IndexOf('+');
		return (indexOfPlus > 0) ? version[..indexOfPlus] : version;
	}
}
