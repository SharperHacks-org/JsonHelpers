// Copyright and trademark notices at the end of this file.

using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharperHacks.CoreLibs.JsonHelpers;

#if false // ToDo: Find out why this isn't needed.
/// <summary>
/// JSON serialization factory for `[Flags]` based `enum's` as `string[]`
/// </summary>
/// <see href="https://stackoverflow.com/a/59430729/5219886">based on this model</see>
public class EnumWithFlagsJsonConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// Constructor
    /// </summary>
    public EnumWithFlagsJsonConverterFactory() { }

    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
    {
        // https://github.com/dotnet/runtime/issues/42602#issue-706711292
        return typeToConvert.IsEnum && typeToConvert.IsDefined(typeof(FlagsAttribute), false);
    }

    /// <inheritdoc/>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(EnumWithFlagsJsonConverter<>).MakeGenericType(typeToConvert);
        var result = (JsonConverter?)Activator.CreateInstance(converterType);
        Verify.IsNotNull(result);
        return result;
    }
}
#endif

/// <summary>
/// JSON serialization for `[Flags]` based `enum's` as `string[]`
/// </summary>
/// <see href="https://github.com/dotnet/runtime/issues/31081#issuecomment-848697673">based on this model</see>
public class EnumWithFlagsJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, System.Enum
{
    private readonly Dictionary<TEnum, string> _enumToString = new Dictionary<TEnum, string>();
    private readonly Dictionary<string, TEnum> _stringToEnum = new Dictionary<string, TEnum>();

    /// <summary>
    /// Constructor.
    /// </summary>
    public EnumWithFlagsJsonConverter()
    {
        var type = typeof(TEnum);
        var values = System.Enum.GetValues<TEnum>();

        foreach (var value in values)
        {
            var enumMember = type.GetMember(value.ToString())[0];
            var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false)
              .Cast<EnumMemberAttribute>()
              .FirstOrDefault();

            _stringToEnum.Add(value.ToString(), value);

            if (attr?.Value != null)
            {
                _enumToString.Add(value, attr.Value);
                _stringToEnum.Add(attr.Value, value);
            }
            else
            {
                _enumToString.Add(value, value.ToString());
            }
        }
    }

    /// <inheritdoc/>
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return default; // JwD
            case JsonTokenType.StartArray:
                TEnum ret = default; // JwD
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                        break;
                    var stringValue = reader.GetString();
                    if (_stringToEnum.TryGetValue(stringValue ?? string.Empty, out var _enumValue)) // JwD.
                    {
                        ret = Or(ret, _enumValue);
                    }
                }
                return ret;
            default:
                throw new JsonException();
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var values = System.Enum.GetValues<TEnum>();
        writer.WriteStartArray();
        foreach (var _value in values)
        {
            if (value.HasFlag(_value))
            {
                var v = Convert.ToInt64(_value, CultureInfo.InvariantCulture); // JwD: Invariant culture.
                if (v == 0)
                {
                    // handle "0" case which HasFlag matches to all values
                    // --> only write "0" case if it is the only value present
                    if (value.Equals(_value))
                    {
                        writer.WriteStringValue(_enumToString[_value]);
                    }
                }
                else
                {
                    writer.WriteStringValue(_enumToString[_value]);
                }
            }
        }
        writer.WriteEndArray();
    }

    /// <summary>
    /// Combine two enum flag values into single enum value.
    /// </summary>
    // <see href="https://stackoverflow.com/a/24172851/5219886">based on this SO</see>
    static TEnum Or(TEnum a, TEnum b)
    {
        // JwD: Changed to conditional expression and invariant culture.
        return Enum.GetUnderlyingType(a.GetType()) != typeof(ulong)
            ? (TEnum)Enum.ToObject(a.GetType(), Convert.ToInt64(a, CultureInfo.InvariantCulture) | Convert.ToInt64(b, CultureInfo.InvariantCulture))
            : (TEnum)Enum.ToObject(a.GetType(), Convert.ToUInt64(a, CultureInfo.InvariantCulture) | Convert.ToUInt64(b, CultureInfo.InvariantCulture));
    }
}

// Copyrighted by respective stackoverflow contributors.
// Ripped by Joseph W Donahue from https://stackoverflow.com/a/72762490/3150445, 2025/01/03,
// who added some additional xml doc comments and made it a bit more production ready (see 'JwD' comments above).
// Timeline: https://stackoverflow.com/posts/72762490/timeline indicates CC By-SA 4.0.
// Referenced timelines:
//  https://stackoverflow.com/posts/59430729/timeline indicates CC By-SA 4.0.
//  https://stackoverflow.com/posts/24172851/timeline indicates CC By-SA 3.0.
// The Github.com reference (https://github.com/dotnet/runtime/issues/31081#issuecomment-848697673) is
// a dotnet/runtime issues thread, so MIT license?
