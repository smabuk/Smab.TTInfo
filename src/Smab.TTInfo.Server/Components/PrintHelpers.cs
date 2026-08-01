using Microsoft.AspNetCore.Components;

namespace Smab.TTInfo.Server.Components;

/// <summary>
/// Helper methods for generating print-related components
/// </summary>
public class PrintHelpers
{
	/// <summary>
	/// Generates a RenderFragment containing CSS @page rules for print layout
	/// Can't put this in the css file as the at-rule page affects other razor pages
	/// </summary>
	/// <param name="paperSize">The paper size (e.g., "A4", "Letter")</param>
	/// <param name="orientation">The page orientation ("portrait" or "landscape")</param>
	/// <param name="marginSize">The margin size in appropriate units</param>
	/// <returns>A RenderFragment containing the style element</returns>
	public static RenderFragment PaperInfo(string? paperSize = "A4", string? orientation = "portrait", int? marginSize = 1)
	{
		return builder =>
		{
			builder.OpenElement(0, "style");
			builder.AddContent(1, $$"""
				@page {
					size: {{paperSize}} {{orientation}};
					margin: {{marginSize}};
				}
				@page :blank {
				  @top-center { content: "This page is unintentionally left blank" }
				}
				""");
			builder.CloseElement();
		};
	}
}
