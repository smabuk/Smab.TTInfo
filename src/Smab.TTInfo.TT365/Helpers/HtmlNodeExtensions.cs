using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Helpers;

internal static class HtmlNodeExtensions
{
	public static int GetIntValue(this HtmlNode node, string className)
		=> int.Parse(node
			.SelectSingleNode(@$"td[contains(@class, '{className}')]")?
			.InnerText ?? "0"
			);

	public static int? GetIntValueOrDefault(this HtmlNode node, string className, int? defaultValue = null)
		=> int.TryParse(node
			.SelectSingleNode(@$"td[contains(@class, '{className}')]")?
			.InnerText ?? "0",
			out int value)
		? value
		: defaultValue;

	public static string GetStringValue(this HtmlNode node, string className)
		=> node
		.SelectSingleNode(@$"td[contains(@class, '{className}')]")?
		.InnerText
		.Trim() ?? "";
}
