namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	extension(string oneDriveExcelLink)
	{
		/// <summary>
		/// Opens an Excel package from a OneDrive link using the provided HttpClient.
		/// </summary>
		/// <param name="httpClient">The HttpClient to use for downloading the Excel file.</param>
		/// <returns>An ExcelPackage representing the downloaded Excel file.</returns>
		/// <exception cref="Exception">Thrown if the Excel file cannot be downloaded.</exception>
		public async Task<ExcelPackage> OpenExcelPackage(HttpClient httpClient)
		{
			ExcelPackage.License.SetNonCommercialPersonal("Simon Brookes");

			if (oneDriveExcelLink.StartsWith("http")) {
				using HttpResponseMessage response = await httpClient.GetAsync($"{oneDriveExcelLink}&download=1");

				if (response.IsSuccessStatusCode is false) {
					throw new Exception($"Failed to download Excel file from {oneDriveExcelLink}. Status code: {response.StatusCode}");
				}

				Stream stream = await response.Content.ReadAsStreamAsync();

				return new ExcelPackage(stream);
			}

			// If the link is not an HTTP link, treat it as a local file path
			if (!File.Exists(oneDriveExcelLink)) {
				throw new FileNotFoundException($"Local Excel file not found at {oneDriveExcelLink}");
			}

			return new ExcelPackage(new FileInfo(oneDriveExcelLink));
		}
	}
}
