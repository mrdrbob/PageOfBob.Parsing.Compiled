using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public class PerformanceTests
    {
        private readonly ITestOutputHelper output;

        public PerformanceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // Not really a test, but a way to track changes in performance
        [Fact]
        public void ParseCsv()
        {
            var assembly = Assembly.GetAssembly(typeof(PerformanceTests));
            string rawCsv;
            using (var stream = assembly.GetManifestResourceStream("PageOfBob.Parsing.Compiled.Tests.example.csv"))
            {
                rawCsv = new StreamReader(stream).ReadToEnd();
            }

            var parser = ExampleCsvParserString.ParseCsvLine();

            int timesToRun = 50;
            long totalTime = 0;
            var stopWatch = new Stopwatch();

            for(int x = 0; x < timesToRun; x++)
            {
                stopWatch.Start();
                var result  = parser.AsEnumerable(rawCsv).ToList();
                stopWatch.Stop();
                Assert.Equal(5000, result.Count);
                totalTime += stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();
            }

            float meanTime = totalTime / (float)timesToRun;
            output.WriteLine($"Mean time: {meanTime}");
        }

        [Fact]
        public void ParseCsvSpan()
        {
            var assembly = Assembly.GetAssembly(typeof(PerformanceTests));
            string rawCsv;
            using (var stream = assembly.GetManifestResourceStream("PageOfBob.Parsing.Compiled.Tests.example.csv"))
            {
                rawCsv = new StreamReader(stream).ReadToEnd();
            }

            var parser = ExampleCsvParserSpan.ParseCsvLine();

            int timesToRun = 50;
            long totalTime = 0;
            var stopWatch = new Stopwatch();

            for (int x = 0; x < timesToRun; x++)
            {
                stopWatch.Start();
                var result = parser.AsEnumerable(rawCsv).ToList();
                stopWatch.Stop();
                Assert.Equal(5000, result.Count);
                totalTime += stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();
            }

            float meanTime = totalTime / (float)timesToRun;
            output.WriteLine($"Mean time: {meanTime}");
        }
    }
}
