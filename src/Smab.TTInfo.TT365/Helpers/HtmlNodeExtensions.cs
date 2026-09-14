using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Helpers;

internal static class HtmlNodeExtensions
{
	extension(HtmlNode node)
	{
		public int GetIntValue(string className)
		=> int.Parse(node
			.SelectSingleNode(@$"td[contains(@class, '{className}')]")?
			.InnerText ?? "0"
			);

		public int GetIntValue(int columnIndex)
		=> int.TryParse(node
			.SelectSingleNode(@$"td[{columnIndex}]")?
			.InnerText,
			out int value)
		? value
		: 0;

		public int? GetIntValueOrDefault(string className, int? defaultValue = null)
			=> int.TryParse(node
				.SelectSingleNode(@$"td[contains(@class, '{className}')]")?
				.InnerText,
				out int value)
			? value
			: defaultValue;

		public int? GetIntValueOrDefaultInSpan(int columnIndex, int? defaultValue = null)
			=> int.TryParse(node
				.SelectSingleNode(@$"td[{columnIndex}]/span")?
				.InnerText,
				out int value)
			? value
			: defaultValue;

		public string GetStringValue(string className)
			=> node
			.SelectSingleNode(@$"td[contains(@class, '{className}')]")?
			.InnerText
			.Trim() ?? "";

		public HtmlNode? GetSingleNodeByClass(string className)
			=> node.SelectSingleNode($"div[@class='{className}']");

		public HtmlNode? GetFirstNodeByClass(string className)
			=> node.SelectSingleNode($"//div[@class='{className}']");

		public HtmlNode? GetSingleNodeById(string id)
			=> node.SelectSingleNode($"div[@id='{id}']");

		public HtmlNode? GetFirstNodeById(string id)
			=> node.SelectSingleNode($"//div[@id='{id}']");
	}
}
