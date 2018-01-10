using System.Linq;
using Xunit;
using static PageOfBob.Parsing.Compiled.StringRules.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public class ExtensionTests
    {
        [Fact]
        public void CanIterateAParser()
        {
            var parser = Text("abc").CompileParser("CanIterateAParser");

            var text = "abcabcabcabc";
            var list = parser.AsEnumerable(text).ToList();
            Assert.Equal(4, list.Count);
        }
    }
}
