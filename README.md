![SharperHacks logo](https://raw.githubusercontent.com/SharperHacks-org/Assets/main/Images/SHLLC-Logo.png)
# THIS PROJECT MOVED TO [codeberg.org](https://codeberg.org/SharperHacks-org/JsonHelpers).
# THIS REPO WILL BE REMOVED SOON.
## JasonHelpers Library for DotNet.
## SharperHacks.CoreLibs.JasonHelpers

Some useful JSON utility bits wrapping System.Text.Json.

Licensed under the Apache License, Version 2.0. See [LICENSE](LICENSE).

Contact: joseph@sharperhacks.org

Nuget: https://www.nuget.org/packages/SharperHacks.CoreLibs.JsonHelpers

### Targets
- net8.0
- net9.0
- net10.0

### Classes

#### JsonConfiguration

Provides static JsonSerializerOptions instances, lazily initialized for compact 
and pretty printed Json. All lazy initializers are thread safe.

```
  /// Creates or returns JsonSerializerOptions and TextEncoderSettings
  /// configured for compact Json.
  public static JsonSerializerOptions Compact { get; }

  /// Creates or returns default JsonSerializerOptions.
  public static JsonSerializerOptions Default { get; }

  /// Creates or returns JsonSerializerOptions and TextEncoderSettings
  /// configured for pretty Json.
  public static JsonSerializerOptions PrettyPrint { get; }

  /// Creates or returns JsonSerializerOptions and TextEncoderSettings 
  /// configured for compact unescaped (unsafe for http) Json.
  public static JsonSerializerOptions UnsafeCompact { get; }

  /// Creates or returns JsonSerializerOptions and TextEncoderSettings
  /// configured for pretty unescaped (unsafe for http) Json.
  public static JsonSerializerOptions UnsafePrettyPrint { get; }
```

#### JsonConversions

Static wrappers around JsonSerializer.Serialize(...) and JsonSerializer.Deserialize(...),
that apply one of the four "standard" SerializerOptions provided by JsoConfiguration.

```
  /// Convert T to JSON string.
  public static string ToJsonString<T>(T instance, bool prettyPrint = false, bool safe = true) {...}

  /// Convert T to JSON UTF8 byte array.
  public static byte[] ToJsonUtf8Bytes<T>(T instance, bool prettyPrint = false, bool safe = true) {...}

  /// Convert T to JSON UTF8 Span{byte}.
  public static Span<byte> ToJsonUtf8Span<T>(T instance, bool prettyPrint = false, bool safe = true) {...}

  /// Convert T to JSON UTF8 Span{byte}.
  public static Span<byte> ToJsonUtf8Span<T>(T instance, bool prettyPrint = false, bool safe = true) {...}

  /// Create instance of T from JSON string.
  public static T? FromJson<T>(string json) {...}

  /// Create instance of T from JSON byte array.
  public static T? FromJson<T>(byte[] utf8json) {...}

  /// Create instance of T from JSON Span{byte}.
  public static T? FromJson<T>(Span<byte> utf8json)
```




