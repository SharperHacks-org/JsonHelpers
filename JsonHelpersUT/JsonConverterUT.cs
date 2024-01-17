// Copyright and trademark notices at bottom of file.

using SharperHacks.CoreLibs.JsonHelpers;

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SharperHacks.CoreLibs.Telemetry.UnitTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class JsonConverterUT
{
    public record SimpleRecord
    {
        public string StringValue { get; init; }

        public int IntValue { get; init; }

        public SimpleRecord(string stringValue, int intValue)
        {
            StringValue = stringValue;
            IntValue = intValue;
        }
    }

    internal static SimpleRecord _sr1 = new("SR1", 41);

    internal static SimpleRecord _sr2 = new("SR2", 43);

    public record ComplexRecord
    {
        public List<int> IntList { get; init; } = new(new[] { 1, 2, 3, });

        public Dictionary<string, object> Dictionary { get; init; }
            = new()
            {
                {"One", 1},
                {"Two", 2},
                {"Three", "1+2"}
            };

        public SimpleRecord Cr1 { get; init; } = _sr1;

        public SimpleRecord Cr2 { get; init; } = _sr2;
    }

    internal static ComplexRecord _cr1 = new();

    [TestMethod]
    public void ToJsonStringDefaults()
    {
        var sr1Json = JsonConversions.ToJsonString(_sr1);
        var cr1Json = JsonConversions.ToJsonString(_cr1);

        Console.WriteLine("Strings:");
        Console.WriteLine($"{sr1Json}");
        Console.WriteLine($"{cr1Json}");

        Assert.AreEqual(_sr1, JsonConversions.FromJson<SimpleRecord>(sr1Json));

        var cr1RoundTrip = JsonConversions.FromJson<ComplexRecord>(cr1Json);
        Assert.IsNotNull( cr1RoundTrip );

        int idx = 0;
        foreach(var item in cr1RoundTrip.IntList )
        {
            Assert.AreEqual(item, _cr1.IntList[idx++]);
        }

        foreach(var item in _cr1.Dictionary )
        {
            Assert.IsTrue(cr1RoundTrip.Dictionary.ContainsKey(item.Key));
            Assert.AreEqual(item.Value.ToString(), cr1RoundTrip.Dictionary[item.Key].ToString());
        }
    }

    [TestMethod]
    public void ToJsonUtf8BytesDefaults()
    {
        var sr1Json = JsonConversions.ToJsonUtf8Bytes(_sr1);
        var cr1Json = JsonConversions.ToJsonUtf8Bytes(_cr1);

        Console.WriteLine("Strings:");
        Console.WriteLine($"{Encoding.UTF8.GetString(sr1Json)}");
        Console.WriteLine($"{Encoding.UTF8.GetString(cr1Json)}");

        Assert.AreEqual(_sr1, JsonConversions.FromJson<SimpleRecord>(sr1Json));

        var cr1RoundTrip = JsonConversions.FromJson<ComplexRecord>(cr1Json);
        Assert.IsNotNull(cr1RoundTrip);

        int idx = 0;
        foreach (var item in cr1RoundTrip.IntList)
        {
            Assert.AreEqual(_cr1.IntList[idx++], item);
        }

        foreach (var item in _cr1.Dictionary)
        {
            Assert.IsTrue(cr1RoundTrip.Dictionary.ContainsKey(item.Key));
            Assert.AreEqual(item.Value.ToString(), cr1RoundTrip.Dictionary[item.Key].ToString());
        }
    }

    [TestMethod]
    public void ToJsonUtf8SpanDefaults()
    {
        var sr1Json = JsonConversions.ToJsonUtf8Span(_sr1);
        var cr1Json = JsonConversions.ToJsonUtf8Span(_cr1);

        Console.WriteLine("Strings:");
        Console.WriteLine(Encoding.UTF8.GetString(sr1Json));
        Console.WriteLine(Encoding.UTF8.GetString(cr1Json));

        Assert.AreEqual(_sr1, JsonConversions.FromJson<SimpleRecord>(sr1Json));

        var cr1RoundTrip = JsonConversions.FromJson<ComplexRecord>(cr1Json);
        Assert.IsNotNull(cr1RoundTrip);

        int idx = 0;
        foreach (var item in cr1RoundTrip.IntList)
        {
            Assert.AreEqual(_cr1.IntList[idx++], item);
        }

        foreach (var item in _cr1.Dictionary)
        {
            Assert.IsTrue(cr1RoundTrip.Dictionary.ContainsKey(item.Key));
            Assert.AreEqual(item.Value.ToString(), cr1RoundTrip.Dictionary[item.Key].ToString());
        }
    }

    [TestMethod]
    [DataRow(true, false)]
    [DataRow(true, true)]
    [DataRow(false, false)]
    [DataRow(false, true)]
    public void Coverage(bool prettyPrint, bool safe)
    {
        var sr1JsonString = JsonConversions.ToJsonString(_sr1, prettyPrint, safe);
        var sr1JsonBytes = JsonConversions.ToJsonUtf8Bytes(_sr1, prettyPrint, safe);
        var sr1JsonSpan = JsonConversions.ToJsonUtf8Span(_sr1, prettyPrint, safe);

        var sr1JsonStringRt = JsonConversions.FromJson<SimpleRecord>(sr1JsonString);
        var sr1JsonBytesRt = JsonConversions.FromJson<SimpleRecord>(sr1JsonBytes);
        var sr1JsonSpanRt = JsonConversions.FromJson<SimpleRecord>(sr1JsonSpan);

        Console.WriteLine("Strings:");
        Console.WriteLine(sr1JsonString);
        Console.WriteLine(Encoding.UTF8.GetString(sr1JsonBytes));
        Console.WriteLine(Encoding.UTF8.GetString(sr1JsonSpan));

        Console.WriteLine("Round trip:");
        Console.WriteLine(JsonConversions.ToJsonString(sr1JsonStringRt));
        Console.WriteLine(JsonConversions.ToJsonString(sr1JsonBytesRt));
        Console.WriteLine(JsonConversions.ToJsonString(sr1JsonSpanRt));

        Assert.AreEqual(_sr1, sr1JsonStringRt);
        Assert.AreEqual(_sr1, sr1JsonBytesRt);
        Assert.AreEqual(_sr1, sr1JsonSpanRt);
    }

    [TestMethod]
    public void DictionaryString()
    {
        var d = new Dictionary<string, object>();

        for (int counter = 0; counter < 3; counter++)
        {
            d.Add(counter.ToString(), counter);
        }

        d.Add("SimpleRecord", _sr1);

        var json = JsonConversions.ToJsonString(d);

        Console.WriteLine($"String: {json}");

        Assert.IsTrue(json.StartsWith("{\"0\":0,"));
        Assert.IsTrue(json.Contains("\"1\":1,"));
        Assert.IsTrue(json.Contains("\"2\":2,"));
        Assert.IsTrue(json.Contains(",\"SimpleRecord\":{"));
        Assert.IsTrue(json.Contains("{\"StringValue\":\"SR1\","));
        Assert.IsTrue(json.EndsWith("IntValue\":41}}"));
    }

#if false // Turns out the options don't seem to appreciably affect the deserializer.
    [TestMethod]
    [DataRow(true, false)]
    [DataRow(true, true)]
    [DataRow(false, false)]
    [DataRow(false, true)]
    public void FromJson_ReadsAny_ToJson_Output(bool prettyPrint, bool safe)
    {
        var jsonString = JsonConversions.ToJsonString(_sr1, prettyPrint, safe);
        var simpleRecord = JsonConversions.FromJson<SimpleRecord>(jsonString);
        Assert.AreEqual(_sr1, simpleRecord);
    }

    [TestMethod]
    [DataRow(true, false, true, false)]
    [DataRow(true, false, false, false)]
    [DataRow(true, false, false, true)]
    [DataRow(true, false, true, true)]

    [DataRow(false, false, true, false)]
    [DataRow(false, false, false, false)]
    [DataRow(false, false, false, true)]
    [DataRow(false, false, true, true)]

    [DataRow(false, true, true, false)]
    [DataRow(false, true, false, false)]
    [DataRow(false, true, false, true)]
    [DataRow(false, true, true, true)]

    [DataRow(true, true, true, false)]
    [DataRow(true, true, false, false)]
    [DataRow(true, true, false, true)]
    [DataRow(true, true, true, true)]

    public void FromJsonWithOptions_ToJson_OuputWithOptions(
        bool toJsonPrettyPrint, bool toJsonSafe,
        bool fromJsonPrettyPrint, bool fromJsonSafe)
    {
        var jsonString = JsonConversions.ToJsonString(_sr2, toJsonPrettyPrint, toJsonSafe);
        var simpleRecord = JsonConversions.FromJson<SimpleRecord>(
            jsonString, fromJsonPrettyPrint, fromJsonSafe);
        Assert.AreEqual(_sr2, simpleRecord);
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
