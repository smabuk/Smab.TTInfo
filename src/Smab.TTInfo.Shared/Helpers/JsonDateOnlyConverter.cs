using System.Text.Json.Serialization;

namespace Smab.Shared.Helpers;

/// <summary>
/// Converts <see cref="DateOnly"/> values to and from JSON string representation.
/// </summary>
public sealed class JsonDateOnlyConverter : JsonConverter<DateOnly>
{
	/// <inheritdoc/>
	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> DateOnly.FromDateTime(reader.GetDateTime());

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}

/// <summary>
/// Converts <see cref="TimeOnly"/> values to and from JSON string representation.
/// </summary>
public sealed class JsonTimeOnlyConverter : JsonConverter<TimeOnly>
{
	/// <inheritdoc/>
	public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> TimeOnly.FromDateTime(reader.GetDateTime());

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString("HH:mm:ss"));
}

/// <summary>
/// Extension methods for adding DateOnly and TimeOnly converters to <see cref="JsonSerializerOptions"/>.
/// </summary>
public static class JsonConverterExtensions
{
	/// <summary>
	/// Adds <see cref="JsonDateOnlyConverter"/> and <see cref="JsonTimeOnlyConverter"/> to the specified options.
	/// </summary>
	public static void AddDateOnlyAndTimeOnlyConverters(this JsonSerializerOptions options)
	{
		options.Converters.Add(new JsonDateOnlyConverter());
		options.Converters.Add(new JsonTimeOnlyConverter());
	}
}
