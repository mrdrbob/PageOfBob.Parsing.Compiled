using Xunit;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public static class TestExtensions
    {
        public static void AssertSuccess<T>(this IParser<T> parser, string input, T expectedOutput, int expectedPosition, int startPosition = 0)
        {
            bool success = parser.TryParse(input, out T result, out int position, startPosition);
            Assert.True(success);
            Assert.Equal(expectedOutput, result);
            Assert.Equal(expectedPosition, position);
        }

        public static void AssertSuccessNull<T>(this IParser<T> parser, string input, int expectedPosition, int startPosition = 0) where T : class
        {
            bool success = parser.TryParse(input, out T result, out int position, startPosition);
            Assert.True(success);
            Assert.Null(result);
            Assert.Equal(expectedPosition, position);
        }

        public static void AssertFailure<T>(this IParser<T> parser, string input, int expectedPosition, int startPosition = 0)
        {
            bool success = parser.TryParse(input, out T result, out int position, startPosition);
            Assert.False(success);
            Assert.Equal(expectedPosition, position);
        }
    }
}
