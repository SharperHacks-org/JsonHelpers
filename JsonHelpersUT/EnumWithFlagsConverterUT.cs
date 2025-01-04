// Copyright and trademark notices at bottom of file.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SharperHacks.CoreLibs.JsonHelpers.JsonHelpersUT;

[ExcludeFromCodeCoverage]
[TestClass]
public class EnumWithFlagsConverterUT
{
    [TestMethod]
    public void SmokeIt()
    {
        var singleFlagged = new RecordContainingEnumFlags() { Flags = Flagged.Bit0 };
        var trippleFlagged = new RecordContainingEnumFlags()
            { Flags = Flagged.Bit0 | Flagged.Bit1 | Flagged.Bit2 };

        var js1 = JsonConversions.ToJsonString(singleFlagged);
        var js2 = JsonConversions.ToJsonString(trippleFlagged);

        Console.WriteLine(js1);
        Console.WriteLine(js2);

        Assert.AreEqual("{\"Flags\":[\"Bit0\"]}", js1);
        Assert.AreEqual("{\"Flags\":[\"Bit0\",\"Bit1\",\"Bit2\"]}", js2);

        var singleRoundTrip = JsonConversions.FromJson<RecordContainingEnumFlags>(js1);
        var trippleRountTrip = JsonConversions.FromJson<RecordContainingEnumFlags>(js2);

        Console.WriteLine(singleRoundTrip);
        Console.WriteLine(trippleRountTrip);

        Assert.AreEqual(singleFlagged, singleRoundTrip);
        Assert.AreEqual(trippleFlagged, trippleRountTrip);
    }
}

[Flags]
public enum Flagged
{
    Bit0 = 0x01,
    Bit1 = 0x02,
    Bit2 = 0x04
}

[ExcludeFromCodeCoverage]
public record RecordContainingEnumFlags
{
    [JsonConverter(typeof(EnumWithFlagsJsonConverter<Flagged>))]
    public Flagged Flags { get; set; }
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

