using System.Globalization;
using System.Text.Json.Serialization;

namespace Smab.TTInfo.Models.___TTLeagues;

public enum DisplayNameEnum { BourneEndLions, CippenhamNormans, CippenhamSaxons, CippenhamSpartans, CippenhamTitans, CippenhamTrojans, CippenhamVikings, CookhamSocialPumas, Free, NwcaBluejays, NwcaSeahawks, OldGordsHarriers, OlopBears, StokePogesBees };

public enum Competition { MaidenheadTableTennisLeague20232024 };

public enum MatchName { DivisionOne, DivisionTwo };

public enum Venue { BourneEndJuniorSportsClub, BoyneHillCricketClub, CippenhamTableTennisCentre, CookhamSocialClub, Empty, NewWindsorCommunityAssociation, OlopTtc, StokePogesTableTennisClub };

internal static class Converter
{
	public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
	{
		Converters =
		{
			DisplayNameEnumConverter.Singleton,
			CompetitionConverter.Singleton,
			MatchNameConverter.Singleton,
			VenueConverter.Singleton,
			new DateOnlyConverter(),
			new TimeOnlyConverter(),
			IsoDateTimeOffsetConverter.Singleton
		},
	};
}

internal class DisplayNameEnumConverter : JsonConverter<DisplayNameEnum>
{
	public override bool CanConvert(Type t) => t == typeof(DisplayNameEnum);

	public override DisplayNameEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"Bourne End Lions" => DisplayNameEnum.BourneEndLions,
			"Cippenham Normans" => DisplayNameEnum.CippenhamNormans,
			"Cippenham Saxons" => DisplayNameEnum.CippenhamSaxons,
			"Cippenham Spartans" => DisplayNameEnum.CippenhamSpartans,
			"Cippenham Titans" => DisplayNameEnum.CippenhamTitans,
			"Cippenham Trojans" => DisplayNameEnum.CippenhamTrojans,
			"Cippenham Vikings" => DisplayNameEnum.CippenhamVikings,
			"Cookham Social Pumas" => DisplayNameEnum.CookhamSocialPumas,
			"Free" => DisplayNameEnum.Free,
			"NWCA Bluejays" => DisplayNameEnum.NwcaBluejays,
			"NWCA Seahawks" => DisplayNameEnum.NwcaSeahawks,
			"OLOP Bears" => DisplayNameEnum.OlopBears,
			"Old Gords Harriers" => DisplayNameEnum.OldGordsHarriers,
			"Stoke Poges Bees" => DisplayNameEnum.StokePogesBees,
			_ => throw new Exception("Cannot unmarshal type DisplayNameEnum"),
		};
	}

	public override void Write(Utf8JsonWriter writer, DisplayNameEnum value, JsonSerializerOptions options)
	{
		switch (value) {
			case DisplayNameEnum.BourneEndLions:
				JsonSerializer.Serialize(writer, "Bourne End Lions", options);
				return;
			case DisplayNameEnum.CippenhamNormans:
				JsonSerializer.Serialize(writer, "Cippenham Normans", options);
				return;
			case DisplayNameEnum.CippenhamSaxons:
				JsonSerializer.Serialize(writer, "Cippenham Saxons", options);
				return;
			case DisplayNameEnum.CippenhamSpartans:
				JsonSerializer.Serialize(writer, "Cippenham Spartans", options);
				return;
			case DisplayNameEnum.CippenhamTitans:
				JsonSerializer.Serialize(writer, "Cippenham Titans", options);
				return;
			case DisplayNameEnum.CippenhamTrojans:
				JsonSerializer.Serialize(writer, "Cippenham Trojans", options);
				return;
			case DisplayNameEnum.CippenhamVikings:
				JsonSerializer.Serialize(writer, "Cippenham Vikings", options);
				return;
			case DisplayNameEnum.CookhamSocialPumas:
				JsonSerializer.Serialize(writer, "Cookham Social Pumas", options);
				return;
			case DisplayNameEnum.Free:
				JsonSerializer.Serialize(writer, "Free", options);
				return;
			case DisplayNameEnum.NwcaBluejays:
				JsonSerializer.Serialize(writer, "NWCA Bluejays", options);
				return;
			case DisplayNameEnum.NwcaSeahawks:
				JsonSerializer.Serialize(writer, "NWCA Seahawks", options);
				return;
			case DisplayNameEnum.OlopBears:
				JsonSerializer.Serialize(writer, "OLOP Bears", options);
				return;
			case DisplayNameEnum.OldGordsHarriers:
				JsonSerializer.Serialize(writer, "Old Gords Harriers", options);
				return;
			case DisplayNameEnum.StokePogesBees:
				JsonSerializer.Serialize(writer, "Stoke Poges Bees", options);
				return;
		}
		throw new Exception("Cannot marshal type DisplayNameEnum");
	}

	public static readonly DisplayNameEnumConverter Singleton = new();
}

internal class CompetitionConverter : JsonConverter<Competition>
{
	public override bool CanConvert(Type t) => t == typeof(Competition);

	public override Competition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value == "Maidenhead Table Tennis League 2023 - 2024"
			? Competition.MaidenheadTableTennisLeague20232024
			: throw new Exception("Cannot unmarshal type Competition");
	}

	public override void Write(Utf8JsonWriter writer, Competition value, JsonSerializerOptions options)
	{
		if (value == Competition.MaidenheadTableTennisLeague20232024) {
			JsonSerializer.Serialize(writer, "Maidenhead Table Tennis League 2023 - 2024", options);
			return;
		}
		throw new Exception("Cannot marshal type Competition");
	}

	public static readonly CompetitionConverter Singleton = new();
}

internal class MatchNameConverter : JsonConverter<MatchName>
{
	public override bool CanConvert(Type t) => t == typeof(MatchName);

	public override MatchName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"Division One" => MatchName.DivisionOne,
			"Division Two" => MatchName.DivisionTwo,
			_ => throw new Exception("Cannot unmarshal type MatchName"),
		};
	}

	public override void Write(Utf8JsonWriter writer, MatchName value, JsonSerializerOptions options)
	{
		switch (value) {
			case MatchName.DivisionOne:
				JsonSerializer.Serialize(writer, "Division One", options);
				return;
			case MatchName.DivisionTwo:
				JsonSerializer.Serialize(writer, "Division Two", options);
				return;
		}
		throw new Exception("Cannot marshal type MatchName");
	}

	public static readonly MatchNameConverter Singleton = new();
}

internal class VenueConverter : JsonConverter<Venue>
{
	public override bool CanConvert(Type t) => t == typeof(Venue);

	public override Venue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"" => Venue.Empty,
			"Bourne End Junior Sports Club" => Venue.BourneEndJuniorSportsClub,
			"Boyne Hill Cricket Club" => Venue.BoyneHillCricketClub,
			"Cippenham Table Tennis Centre" => Venue.CippenhamTableTennisCentre,
			"Cookham Social Club" => Venue.CookhamSocialClub,
			"New Windsor Community Association" => Venue.NewWindsorCommunityAssociation,
			"OLOP TTC" => Venue.OlopTtc,
			"Stoke Poges Table Tennis Club" => Venue.StokePogesTableTennisClub,
			_ => throw new Exception("Cannot unmarshal type Venue"),
		};
	}

	public override void Write(Utf8JsonWriter writer, Venue value, JsonSerializerOptions options)
	{
		switch (value) {
			case Venue.Empty:
				JsonSerializer.Serialize(writer, "", options);
				return;
			case Venue.BourneEndJuniorSportsClub:
				JsonSerializer.Serialize(writer, "Bourne End Junior Sports Club", options);
				return;
			case Venue.BoyneHillCricketClub:
				JsonSerializer.Serialize(writer, "Boyne Hill Cricket Club", options);
				return;
			case Venue.CippenhamTableTennisCentre:
				JsonSerializer.Serialize(writer, "Cippenham Table Tennis Centre", options);
				return;
			case Venue.CookhamSocialClub:
				JsonSerializer.Serialize(writer, "Cookham Social Club", options);
				return;
			case Venue.NewWindsorCommunityAssociation:
				JsonSerializer.Serialize(writer, "New Windsor Community Association", options);
				return;
			case Venue.OlopTtc:
				JsonSerializer.Serialize(writer, "OLOP TTC", options);
				return;
			case Venue.StokePogesTableTennisClub:
				JsonSerializer.Serialize(writer, "Stoke Poges Table Tennis Club", options);
				return;
		}
		throw new Exception("Cannot marshal type Venue");
	}

	public static readonly VenueConverter Singleton = new();
}

public class DateOnlyConverter : JsonConverter<DateOnly>
{
	private readonly string serializationFormat;
	public DateOnlyConverter() : this(null) { }

	public DateOnlyConverter(string? serializationFormat)
	{
		this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
	}

	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return DateOnly.Parse(value!);
	}

	public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString(serializationFormat));
}

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
	private readonly string serializationFormat;

	public TimeOnlyConverter() : this(null) { }

	public TimeOnlyConverter(string? serializationFormat)
	{
		this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
	}

	public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return TimeOnly.Parse(value!);
	}

	public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString(serializationFormat));
}

internal class IsoDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
	public override bool CanConvert(Type t) => t == typeof(DateTimeOffset);

	private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

	private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
	private string? _dateTimeFormat;
	private CultureInfo? _culture;

	public DateTimeStyles DateTimeStyles
	{
		get => _dateTimeStyles;
		set => _dateTimeStyles = value;
	}

	public string? DateTimeFormat
	{
		get => _dateTimeFormat ?? string.Empty;
		set => _dateTimeFormat = (string.IsNullOrEmpty(value)) ? null : value;
	}

	public CultureInfo Culture
	{
		get => _culture ?? CultureInfo.CurrentCulture;
		set => _culture = value;
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		string text;


		if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
			|| (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal) {
			value = value.ToUniversalTime();
		}

		text = value.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);

		writer.WriteStringValue(text);
	}

	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string? dateText = reader.GetString();

		return string.IsNullOrEmpty(dateText) is false
			? !string.IsNullOrEmpty(_dateTimeFormat)
				? DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles)
				: DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles)
			: default;
	}


	public static readonly IsoDateTimeOffsetConverter Singleton = new();
}
