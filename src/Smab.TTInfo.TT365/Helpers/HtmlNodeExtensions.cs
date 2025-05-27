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

	public static HtmlNode? GetSingleNodeByClass(this HtmlNode node, string className)
		=> node.SelectSingleNode($"div[@class='{className}']");

	public static HtmlNode? GetFirstNodeByClass(this HtmlNode node, string className)
		=> node.SelectSingleNode($"//div[@class='{className}']");

	public static HtmlNode? GetSingleNodeById(this HtmlNode node, string id)
		=> node.SelectSingleNode($"div[@id='{id}']");

	public static HtmlNode? GetFirstNodeById(this HtmlNode node, string id)
		=> node.SelectSingleNode($"//div[@id='{id}']");
}
