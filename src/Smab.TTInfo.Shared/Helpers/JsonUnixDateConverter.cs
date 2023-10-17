using System.Text.Json.Serialization;

namespace Smab.Shared.Helpers;

public sealed class JsonUnixDateConverter : JsonConverter<DateTime>
{
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> reader.GetDouble().FromUnixDate();

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		=> writer.WriteNumberValue((value.Year == 1) ? 0 : (value - Unix.Epoch).TotalSeconds);
}

public sealed class JsonUnixDateConverterWithNulls : JsonConverter<DateTime?>
{
	public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> reader.GetDouble().FromUnixDate();

	public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
	{
		if (value is null) {
			writer.WriteNullValue();
		}

		if (value.HasValue) {
			if (value.Value.Year == 1) {
				writer.WriteNullValue();
			} else {
				writer.WriteNumberValue((value.Value - Unix.Epoch).TotalSeconds);
			}
		}
	}
}

internal static class Unix
{
	internal static readonly DateTime Epoch = new(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0, kind: DateTimeKind.Utc);
}

internal static class DoubleExtensions
{
	public static DateTime FromUnixDate(this double? unixDate)
	{
		return unixDate switch
		{
			> 99999999999 => Unix.Epoch.AddSeconds(unixDate / 1000 ?? 0.0), // UNIX Epoch milliseconds
			_             => Unix.Epoch.AddSeconds(unixDate ?? 0.0),        // UNIX Epoch seconds
		};
	}

public static DateTime FromUnixDate(this double unixDate)
	{
		return unixDate switch
		{
			> 99999999999 => Unix.Epoch.AddSeconds(unixDate / 1000), // UNIX Epoch milliseconds
			_             => Unix.Epoch.AddSeconds(unixDate),        // UNIX Epoch seconds
		};
	}
}
