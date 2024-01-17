// Copyright and trademark notices at bottom of file.

using SharperHacks.CoreLibs.JsonHelpers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharperHacks.CoreLibs.Telemetry.UnitTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class JsoConfigurationsUT
{
    private JsonSerializerOptions _default = new JsonSerializerOptions();

    private void VerifyOptionSettings(
        JsonSerializerOptions options,
        JavaScriptEncoder? expectedEncoder,
        bool expectedWriteIndented,
        JsonConverter? expectedConverter)
    {
        Assert.AreEqual(expectedEncoder, options.Encoder);
        Assert.AreEqual(expectedWriteIndented, options.WriteIndented);
        if (expectedConverter is not null)
        {
            Assert.IsTrue(options.Converters.Contains(expectedConverter));
        }
    }

    [TestMethod]
    public void SmokeTest()
    {
        VerifyOptionSettings(
            JsoConfigurations.Compact,
            JsoConfigurations.SafeEncoder,
            false,
            JsoConfigurations._jsonEnumConverter);

        VerifyOptionSettings(
            JsoConfigurations.Default,
            _default.Encoder,
            _default.WriteIndented,
            null);

        VerifyOptionSettings(
            JsoConfigurations.PrettyPrint,
            JsoConfigurations.SafeEncoder,
            true,
            JsoConfigurations._jsonEnumConverter);

        VerifyOptionSettings(
            JsoConfigurations.UnsafeCompact,
            JsoConfigurations.UnsafeEncoder,
            false,
            JsoConfigurations._jsonEnumConverter);

        VerifyOptionSettings(
            JsoConfigurations.UnsafePrettyPrint,
            JsoConfigurations.UnsafeEncoder,
            true,
            JsoConfigurations._jsonEnumConverter);
    }

    // Originally created to test a prototype read only wrapper around
    // JsonSerializerOptions that turned out to be too slow. This should
    // be maintained to detect any major perf regression in SerializerOptions.
    [TestMethod]
    public void PerfTest()
    {
        const int iterations = 100000;
        var jsonStrings = new List<string>(10000);
        var localJso = new JsonSerializerOptions();

        var sw = new Stopwatch();

        // Warmup.
        for (int i = 0; i < iterations; i++)
        {
            jsonStrings.Add(JsonSerializer.Serialize(i, localJso));
        }
        jsonStrings.Clear();

        GC.Collect();
        sw.Start();
        for (int i = 0; i < iterations; i++)
        {
            jsonStrings.Add(JsonSerializer.Serialize(i, localJso));
        }
        sw.Stop();
        jsonStrings.Clear();

        var optionsTicks = sw.ElapsedTicks;
        Console.WriteLine($"Local JsonSerializerOptions elapsed time: {optionsTicks}");

        GC.Collect();
        sw.Restart();
        for (int i = 0; i < iterations; i++)
        {
            jsonStrings.Add(JsonSerializer.Serialize(i, new JsonSerializerOptions()));
        }
        sw.Stop();
        jsonStrings.Clear();

        var jsoTicks = sw.ElapsedTicks;
        Console.WriteLine($"new JsonSerializerOptions/iteration elapsed time: {jsoTicks}");

        GC.Collect();
        sw.Restart();
        for (int i = 0; i < iterations; i++)
        {
            jsonStrings.Add(JsonSerializer.Serialize(i, JsoConfigurations.Default));
        }
        sw.Stop();
        jsonStrings.Clear();

        var defaultTicks = sw.ElapsedTicks;
        Console.WriteLine($"{nameof(JsoConfigurations)}.Default elapsed time: {defaultTicks}");

        Console.WriteLine(
            "\nElapsed times are highly variable from run to run. " +
            "This test might fail randomly.");

        // The tick counts are highly variable across runs, but NET8 is faster than
        // NET7 is faster than NET6, in most cases.
        //
        // optionsTicks and defaultTicks keep swapping places for fastest, and are always
        // much smaller than creating a new JsonSerializerOptions instance on each iteration.
        // Under the hood, they are essentially the same, so that is expected.
#if NET6_0
        Assert.IsTrue(Math.Abs(optionsTicks - defaultTicks) < 300000 || defaultTicks < optionsTicks);
#endif
#if NET7_0
        Assert.IsTrue(Math.Abs(optionsTicks - defaultTicks) < 200000 || defaultTicks < optionsTicks);
#endif
#if NET8_0
        Assert.IsTrue(Math.Abs(optionsTicks - defaultTicks) < 170000 || defaultTicks < optionsTicks);
#endif

        Assert.IsTrue(optionsTicks < jsoTicks);
        Assert.IsTrue(defaultTicks < jsoTicks);
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

