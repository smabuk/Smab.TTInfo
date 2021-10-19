using System.Text.Json;
using System.Text.Json.Serialization;

namespace Smab.Shared.Helpers;

public sealed class JsonDateOnlyConverter : JsonConverter<DateOnly>
{
	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> DateOnly.FromDateTime(reader.GetDateTime());

	public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}

public sealed class JsonTimeOnlyConverter : JsonConverter<TimeOnly>
{
	public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> TimeOnly.FromDateTime(reader.GetDateTime());

	public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString("HH:mm:ss"));
}
