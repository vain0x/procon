using Xunit;

public sealed class RepeatedPermutationsAlgorithmTest
{
    public sealed class Test_AllRepeatedPermutations
    {
        [Fact]
        public void Test_edge_cases()
        {
            new[] { 0, 1 }.AllRepeatedPermutations(0).IsSeq(new[] { new int[0] });
            new int[0].AllRepeatedPermutations(1).IsSeq(new int[0][]);
        }

        [Fact]
        public void Test_source_3_count_2()
        {
            var expected = new[]
            {
                new[] { 0, 0 },
                new[] { 0, 1 },
                new[] { 0, 2 },
                new[] { 1, 0 },
                new[] { 1, 1 },
                new[] { 1, 2 },
                new[] { 2, 0 },
                new[] { 2, 1 },
                new[] { 2, 2 },
            };

            new[] { 0, 1, 2 }.AllRepeatedPermutations(2).IsSeq(expected);
        }

        [Fact]
        public void Test_source_2_count_3()
        {
            var expected = new[]
            {
                new[] { 0, 0, 0 },
                new[] { 0, 0, 1 },
                new[] { 0, 1, 0 },
                new[] { 0, 1, 1 },
                new[] { 1, 0, 0 },
                new[] { 1, 0, 1 },
                new[] { 1, 1, 0 },
                new[] { 1, 1, 1 },
            };

            new[] { 0, 1 }.AllRepeatedPermutations(3).IsSeq(expected);
        }
    }
}
