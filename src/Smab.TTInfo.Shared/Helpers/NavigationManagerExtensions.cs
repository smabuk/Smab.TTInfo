using Microsoft.AspNetCore.Components;

namespace Smab.TTInfo.Shared.Helpers;

public static class NavigationManagerExtensions
{
	public static bool IsPage(this NavigationManager navigationManager, string subroute)
		=> navigationManager.Uri.Contains(subroute);
}
