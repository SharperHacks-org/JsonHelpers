// Copyright and trademark notices at the end of this file.

using SharperHacks.CoreLibs.Constraints;

using System.Text.Json;

namespace SharperHacks.CoreLibs.JsonHelpers;

// ToDo: Is this class name going to be a problem?

/// <summary>
/// Static wrappers around JsonSerializer.Serialize(...) and JsonSerializer.Deserialize(...),
/// that apply one of four "standard" SerializerOptions.
/// </summary>
public static class JsonConversions
{
    /// <summary>
    /// Convert T to JSON string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="prettyPrint"></param>
    /// <param name="safe"></param>
    /// <returns>A json string.</returns>
    public static string ToJsonString<T>(T instance, bool prettyPrint = false, bool safe = true)
    {
        var options = prettyPrint
            ? (safe ? JsoConfigurations.PrettyPrint : JsoConfigurations.UnsafePrettyPrint)
            : (safe ? JsoConfigurations.Compact : JsoConfigurations.UnsafeCompact);
        return JsonSerializer.Serialize(instance, options);
    }

    /// <summary>
    /// Convert T to JSON UTF8 byte array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="prettyPrint"></param>
    /// <param name="safe"></param>
    /// <returns></returns>
    public static byte[] ToJsonUtf8Bytes<T>(T instance, bool prettyPrint = false, bool safe = true)
    {
        var options = prettyPrint
            ? (safe ? JsoConfigurations.PrettyPrint : JsoConfigurations.UnsafePrettyPrint)
            : (safe ? JsoConfigurations.Compact : JsoConfigurations.UnsafeCompact);
        return JsonSerializer.SerializeToUtf8Bytes(instance, options);
    }

    /// <summary>
    /// Convert T to JSON UTF8 Span{byte}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="prettyPrint"></param>
    /// <param name="safe"></param>
    /// <returns></returns>
    public static Span<byte> ToJsonUtf8Span<T>(T instance, bool prettyPrint = false, bool safe = true)
    {
        var options = prettyPrint
            ? (safe ? JsoConfigurations.PrettyPrint : JsoConfigurations.UnsafePrettyPrint)
            : (safe ? JsoConfigurations.Compact : JsoConfigurations.UnsafeCompact);
        return JsonSerializer.SerializeToUtf8Bytes(instance, options);
    }

    /// <summary>
    /// Create instance of T from JSON string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T? FromJson<T>(string json)
    {
        Verify.IsNotNullOrEmpty(json);
        return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// Create instance of T from JSON byte array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="utf8json"></param>
    /// <returns></returns>
    public static T? FromJson<T>(byte[] utf8json)
    {
        Verify.IsNotNull(utf8json);
        return JsonSerializer.Deserialize<T>(utf8json);
    }

    /// <summary>
    /// Create instance of T from JSON Span{byte}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="utf8json"></param>
    /// <returns></returns>
    public static T? FromJson<T>(Span<byte> utf8json)
    {
        Verify.IsGreaterThan(utf8json.Length, 0);
        return JsonSerializer.Deserialize<T>(utf8json);
    }

#if false // ToDo: Do we need this? Seems pointless, given the range of options have zero effect.
          // See my comment on the associated unit test.
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="prettyPrint"></param>
    /// <param name="safe"></param>
    /// <returns></returns>
    public static T? FromJson<T>(string json, bool prettyPrint = false, bool safe = true) 
    { 
        Verify.IsNotNullOrEmpty(json);

        var options = prettyPrint
            ? (safe ? JsoConfigurations.PrettyPrint : JsoConfigurations.UnsafePrettyPrint)
            : (safe ? JsoConfigurations.Compact : JsoConfigurations.UnsafeCompact);

        return JsonSerializer.Deserialize<T>(json, options);
    }
#endif
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
