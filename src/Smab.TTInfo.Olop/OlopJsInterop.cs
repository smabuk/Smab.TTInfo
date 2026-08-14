using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Smab.TTInfo.Olop;
// The associated JavaScript module is loaded on demand when first needed.
//
// This class should be registered as scoped DI service and then injected into Blazor
// components for use.

public class OlopJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable {
	private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
			"import", "./_content/Smab.TTInfo.Olop/olopJsInterop.js").AsTask());

	public async ValueTask<string> CopyToClipboard(ElementReference reference) {
		var module = await moduleTask.Value;
		return await module.InvokeAsync<string>("copyText", reference);
	}

	public async ValueTask DisposeAsync() {
		if (moduleTask.IsValueCreated) {
			var module = await moduleTask.Value;
			try {
				await module.DisposeAsync();

			}
			catch (Exception) {
			}

			GC.SuppressFinalize(this);
		}
	}
}
