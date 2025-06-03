namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a websites host in TTLeagues, including contact, configuration, and menu details.
/// </summary>
/// <param name="TenantId">The tenant identifier.</param>
/// <param name="Title">The title of the website.</param>
/// <param name="Description">The website description.</param>
/// <param name="About">The about section content.</param>
/// <param name="Contact">The contact information.</param>
/// <param name="Instagram">The Instagram handle.</param>
/// <param name="Twitter">The Twitter handle.</param>
/// <param name="Facebook">The Facebook handle.</param>
/// <param name="WhatsApp">The WhatsApp contact.</param>
/// <param name="Email">The email address.</param>
/// <param name="Mobile">The mobile number.</param>
/// <param name="UpdatedById">The identifier of the user who last updated.</param>
/// <param name="Updated">The last updated date.</param>
/// <param name="Created">The creation date.</param>
/// <param name="Images">The list of images.</param>
/// <param name="Links">The list of links.</param>
/// <param name="Menu">The website menu.</param>
/// <param name="Config">The website configuration.</param>
/// <param name="EmailIntro">The email introduction text.</param>
/// <param name="EmailBody">The email body text.</param>
internal sealed record WebsitesHost(
	int    TenantId,
	string Title,
	string Description,
	string About,
	string Contact,
	string Instagram,
	string Twitter,
	string Facebook,
	string WhatsApp,
	string Email,
	string Mobile,
	string UpdatedById,
	DateTimeOffset Updated,
	DateTimeOffset Created,
	ImmutableList<object> Images,
	ImmutableList<object> Links,
	WebsitesHostMenu      Menu,
	WebsitesHostConfig    Config,
	string EmailIntro,
	string EmailBody
);

