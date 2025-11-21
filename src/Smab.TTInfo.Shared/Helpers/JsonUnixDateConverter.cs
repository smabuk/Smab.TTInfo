using System.Text.Json.Serialization;

namespace Smab.Shared.Helpers;

/// <summary>
/// Converts <see cref="DateTime"/> values to and from Unix time for JSON serialization.
/// </summary>
public sealed class JsonUnixDateConverter : JsonConverter<DateTime>
{
	/// <inheritdoc/>
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> reader.GetDouble().FromUnixDate();

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		=> writer.WriteNumberValue((value.Year == 1) ? 0 : (value - Unix.Epoch).TotalSeconds);
}

/// <summary>
/// Converts nullable <see cref="DateTime"/> values to and from Unix time for JSON serialization, supporting nulls.
/// </summary>
public sealed class JsonUnixDateConverterWithNulls : JsonConverter<DateTime?>
{
	/// <inheritdoc/>
	public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> reader.GetDouble().FromUnixDate();

	/// <inheritdoc/>
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

/// <summary>
/// Provides the Unix epoch constant.
/// </summary>
internal static class Unix
{
	internal static readonly DateTime Epoch = new(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0, kind: DateTimeKind.Utc);
}

/// <summary>
/// Extension methods for converting Unix time to <see cref="DateTime"/>.
/// </summary>
internal static class DoubleExtensions
{
	/// <summary>
	/// Converts a nullable double representing Unix time to a <see cref="DateTime"/>.
	/// </summary>
	public static DateTime FromUnixDate(this double? unixDate)
	{
		return unixDate switch
		{
			> 99999999999 => Unix.Epoch.AddSeconds((unixDate / 1000) ?? 0.0), // UNIX Epoch milliseconds
			_             => Unix.Epoch.AddSeconds(unixDate ?? 0.0),        // UNIX Epoch seconds
		};
	}

	/// <summary>
	/// Converts a double representing Unix time to a <see cref="DateTime"/>.
	/// </summary>
	public static DateTime FromUnixDate(this double unixDate)
	{
		return unixDate switch
		{
			> 99999999999 => Unix.Epoch.AddSeconds(unixDate / 1000), // UNIX Epoch milliseconds
			_             => Unix.Epoch.AddSeconds(unixDate),        // UNIX Epoch seconds
		};
	}
}
