// Copyright and trademark notices at the end of this file.

using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

[assembly: InternalsVisibleTo("SharperHacks.CoreLibs.JsonHelpers.JsonHelpersUT")]
namespace SharperHacks.CoreLibs.JsonHelpers;

/// <summary>
/// Provides static JsonSerializerOptions instances, lazily initialized for compact
/// and pretty printed Json. All lazy initializers are thread safe.
/// </summary>
/// <remarks>
/// Each public property returns a reference to a mimimally configured JsonSerializerOptions 
/// instance, with Encoder, WriteIndented and Converters set appropriately. Modifying properties 
/// of the returned JsonSerializerOptions instance, modifies those propertied for all consumers 
/// of that configuration.
/// </remarks>
public static class JsoConfigurations // JsonSerialOptionsConfigurations is too long.
{
    private static object _lock = new object();

    private static JavaScriptEncoder? _safeEncoder;
    private static JavaScriptEncoder? _unsafeEncoder;

    private static JsonSerializerOptions? _compactSerializer;
    private static JsonSerializerOptions? _defaultSerializer;
    private static JsonSerializerOptions? _prettyPrintSerializer;
    private static JsonSerializerOptions? _unsafeCompactSerializer;
    private static JsonSerializerOptions? _unsafePrettyPrintSerializer;

    internal static JsonStringEnumConverter? _jsonEnumConverter;

    internal static JavaScriptEncoder SafeEncoder => _safeEncoder ??= JavaScriptEncoder.Default;
    internal static JavaScriptEncoder UnsafeEncoder => _unsafeEncoder ??= JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

    internal static JsonStringEnumConverter JsonEnumConverter => _jsonEnumConverter ??= new();

    /// <summary>
    /// Creates or returns JsonSerializerOptions and TextEncoderSettings
    /// configured for compact Json.
    /// </summary>
    public static JsonSerializerOptions Compact
    {
        get
        {
            if (_compactSerializer is null)
            {
                lock (_lock)
                {
                    _compactSerializer ??= new JsonSerializerOptions()
                    {
                        Encoder = SafeEncoder,
                        WriteIndented = false,
                        Converters = { JsonEnumConverter },
                    };
                }
            }

            return _compactSerializer;
        }
    }

    /// <summary>
    /// Creates or returns default JsonSerializerOptions.
    /// </summary>
    public static JsonSerializerOptions Default
    {
        get
        {
            if (_defaultSerializer is null)
            {
                lock (_lock)
                {
                    _defaultSerializer ??= new JsonSerializerOptions();
                }
            }

            return _defaultSerializer;
        }
    }

    /// <summary>
    /// Creates or returns JsonSerializerOptions and TextEncoderSettings
    /// configured for pretty Json.
    /// </summary>
    public static JsonSerializerOptions PrettyPrint
    {
        get
        {
            if (_prettyPrintSerializer is null)
            {
                lock (_lock)
                {
                    _prettyPrintSerializer ??= new JsonSerializerOptions()
                    {
                        Encoder = SafeEncoder,
                        WriteIndented = true,
                        Converters = { JsonEnumConverter },
                    };
                }
            }

            return _prettyPrintSerializer;
        }
    }

    /// <summary>
    /// Creates or returns JsonSerializerOptions and TextEncoderSettings 
    /// configured for compact unescaped (unsafe for http) Json.
    /// </summary>
    public static JsonSerializerOptions UnsafeCompact
    {
        get
        {
            if (_unsafeCompactSerializer is null)
            {
                lock (_lock)
                {
                    _unsafeCompactSerializer ??= new JsonSerializerOptions()
                    {
                        Encoder = UnsafeEncoder,
                        WriteIndented = false,
                        Converters = { JsonEnumConverter },
                    };
                }
            }

            return _unsafeCompactSerializer;
        }
    }

    /// <summary>
    /// Creates or returns JsonSerializerOptions and TextEncoderSettings
    /// configured for pretty unescaped (unsafe for http) Json.
    /// </summary>
    public static JsonSerializerOptions UnsafePrettyPrint
    {
        get
        {
            if (_unsafePrettyPrintSerializer == null)
            {
                lock (_lock)
                {
                    _unsafePrettyPrintSerializer ??= new JsonSerializerOptions()
                    {
                        Encoder = UnsafeEncoder,
                        WriteIndented = true,
                        Converters = { JsonEnumConverter },
                    };
                }
            }

            return _unsafePrettyPrintSerializer;
        }
    }
}

// Copyright Joseph W Donahue and Sharper Hacks LLC (US-WA)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// SharperHacks is a trademark of Sharper Hacks LLC (US-Wa), and may not be
// applied to distributions of derivative works, without the express written
// permission of a registered officer of Sharper Hacks LLC (US-WA).
