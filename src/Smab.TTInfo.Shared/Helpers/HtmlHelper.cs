namespace Smab.Shared.Helpers;

/// <summary>
/// Helper methods for working with HTML markkup.
/// </summary>
public static class HtmlHelper
{
	/// <summary>
	/// Determines whether the specified class string contains the given class name (case-insensitive).
	/// </summary>
	/// <param name="classString">The string containing space-separated class names.</param>
	/// <param name="className">The class name to search for.</param>
	/// <returns>True if the class name is present; otherwise, false.</returns>
	public static bool HasClass(this string classString, string className)
	{
		string[] classValues = classString.Split(" ");

		return classValues.Contains(className, StringComparer.InvariantCultureIgnoreCase);
	}
}
