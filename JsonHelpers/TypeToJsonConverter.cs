// Copyright and trademark notices at the end of this file.

using SharperHacks.CoreLibs.Constraints;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharperHacks.CoreLibs.JsonHelpers;

/// <summary>
/// Json support for Type values.
/// </summary>
/// <remarks>
/// Apply '[JsonConverter(typeof(TypeToJsonConverter))]' to properties of type Type.
/// See: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to#register-a-custom-converter
/// </remarks>
public class TypeToJsonConverter : JsonConverter<Type>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override Type? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var stringToConvert = reader.GetString();

        Verify.IsNotNull(stringToConvert);

        return Type.GetType(stringToConvert);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(
        Utf8JsonWriter writer,
        Type value,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(value.FullName);
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
