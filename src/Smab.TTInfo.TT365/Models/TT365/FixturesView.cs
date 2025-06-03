namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a view model for displaying a collection of fixtures with associated metadata and options.
/// </summary>
/// <remarks>This record is designed to encapsulate the data required to render a fixtures view, including a
/// caption, a URL for navigation, configurable options, and a collection of fixtures. The <see cref="Options"/>
/// property allows customization of the view's behavior, while the <see cref="Fixtures"/> property holds the fixtures 
/// to be displayed.</remarks>
public record FixturesView(
	string Caption,
	string URL,
	FixturesViewOptions Options,
	ICollection<Fixture>? Fixtures
);
