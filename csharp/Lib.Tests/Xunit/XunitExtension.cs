using System.Collections.Generic;

namespace Xunit
{
    public static class XunitExtension
    {
        public static void Is(this double actual, double expected, int precision) =>
            Assert.Equal(expected, actual, precision);

        public static void Is(this string actual, string expected) =>
            Assert.Equal(expected, actual);

        public static void Is<T>(this T actual, T expected) =>
            Assert.Equal(expected, actual);

        public static void IsSeq<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
            Assert.Equal(expected, actual);
    }
}
