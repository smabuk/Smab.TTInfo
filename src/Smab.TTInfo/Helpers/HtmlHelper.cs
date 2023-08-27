namespace Smab.Shared.Helpers;

public static class HtmlHelper
{
	public static bool HasClass(this string classString, string className)
	{
		string[] classValues = classString.Split(" ");

		return classValues.Contains(className, StringComparer.InvariantCultureIgnoreCase);
	}
}
