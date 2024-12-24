// Copyright and trademark notices at bottom of file.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SharperHacks.CoreLibs.JsonHelpers.JsonHelpersUT;

[ExcludeFromCodeCoverage]
[TestClass]
public class TypeToJsonConverterUT
{
    private Type? _typeWritten;
    private string? _asWritten;

    public void CanWriteJsonString()
    {
        _typeWritten = typeof(int);
        var test = new Test(_typeWritten);

        _asWritten = JsonConversions.ToJsonString(test);

        Console.WriteLine($"Type to be written: {_typeWritten.FullName}");
        Console.WriteLine($"_asWritten: {_asWritten!}");

        Assert.AreEqual("{\"Type\":\"System.Int32\"}", _asWritten);
    }

    [TestMethod]
    public void CanReadJsonString()
    {
        CanWriteJsonString();
        Assert.IsNotNull( _typeWritten );
        Assert.IsNotNull( _asWritten );

        var testRecord = JsonConversions.FromJson<Test>(_asWritten);

        Assert.IsNotNull(testRecord);
        Assert.AreEqual(_typeWritten, testRecord.Type);
    }
}

[ExcludeFromCodeCoverage]
public record Test
{
    [JsonConverter(typeof(TypeToJsonConverter))]
    public Type Type { get; set; }

    public Test(Type type) => Type = type;
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
