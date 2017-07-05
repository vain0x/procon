using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Collections.Lists
{
    public sealed class PermutationAlgorithmTest
    {
        IComparer<X> DualComparer<X>()
        {
            return Comparer<X>.Create((l, r) => Comparer<X>.Default.Compare(r, l));
        }

        [Fact]
        public void Test_Permutations_odd_length()
        {
            var permutations =
                new int[][]
                {
                    new[] { 0, 1, 2 },
                    new[] { 0, 2, 1 },
                    new[] { 1, 0, 2 },
                    new[] { 1, 2, 0 },
                    new[] { 2, 0, 1 },
                    new[] { 2, 1, 0 },
                };

            Assert.Equal(permutations, Enumerable.Range(0, 3).Permutations());

            var descendingOrder = Enumerable.Range(0, 3).Permutations(DualComparer<int>());
            Assert.Equal(permutations.Reverse(), descendingOrder);
        }

        [Fact]
        public void Test_Permutations_even_length()
        {
            var permutations =
                new int[][]
                {
                    new[] { 0, 1, 2, 3 },
                    new[] { 0, 1, 3, 2 },
                    new[] { 0, 2, 1, 3 },
                    new[] { 0, 2, 3, 1 },
                    new[] { 0, 3, 1, 2 },
                    new[] { 0, 3, 2, 1 },
                    new[] { 1, 0, 2, 3 },
                    new[] { 1, 0, 3, 2 },
                    new[] { 1, 2, 0, 3 },
                    new[] { 1, 2, 3, 0 },
                    new[] { 1, 3, 0, 2 },
                    new[] { 1, 3, 2, 0 },
                    new[] { 2, 0, 1, 3 },
                    new[] { 2, 0, 3, 1 },
                    new[] { 2, 1, 0, 3 },
                    new[] { 2, 1, 3, 0 },
                    new[] { 2, 3, 0, 1 },
                    new[] { 2, 3, 1, 0 },
                    new[] { 3, 0, 1, 2 },
                    new[] { 3, 0, 2, 1 },
                    new[] { 3, 1, 0, 2 },
                    new[] { 3, 1, 2, 0 },
                    new[] { 3, 2, 0, 1 },
                    new[] { 3, 2, 1, 0 },
                };

            Assert.Equal(permutations, Enumerable.Range(0, 4).Permutations());
        }

        [Fact]
        public void Test_Permutations_no_duplication()
        {
            var permutations =
                new int[][]
                {
                    new[] { 0, 0, 1, 2 },
                    new[] { 0, 0, 2, 1 },
                    new[] { 0, 1, 0, 2 },
                    new[] { 0, 1, 2, 0 },
                    new[] { 0, 2, 0, 1 },
                    new[] { 0, 2, 1, 0 },
                    new[] { 1, 0, 0, 2 },
                    new[] { 1, 0, 2, 0 },
                    new[] { 1, 2, 0, 0 },
                    new[] { 2, 0, 0, 1 },
                    new[] { 2, 0, 1, 0 },
                    new[] { 2, 1, 0, 0 },
                };

            Assert.Equal(permutations, new[] { 0, 0, 1, 2 }.Permutations());
        }
    }
}
