using Xunit;

namespace Procon
{
    public sealed class UnionFindForestTest
    {
        [Fact]
        public void Test_initial_state()
        {
            var n = 10;
            var uff = new UnionFindForest(n);
            for (var i = 0; i < n; i++)
            {
                uff.Root(i).Is(i);
            }
        }

        [Fact]
        public void Test_single_group_case()
        {
            var n = 10;

            var uff = new UnionFindForest(n);
            for (var i = 0; i + 1 < n; i++)
            {
                uff.Merge(i, i + 1);
            }

            var r = uff.Root(0);

            for (var i = 1; i < n; i++)
            {
                uff.Root(i).Is(r);
                uff.Connects(r, uff.Root(i)).Is(true);
            }
        }

        [Fact]
        public void Test_two_group_case()
        {
            var n = 10;

            var uff = new UnionFindForest(n);
            for (var i = 0; i < n; i++)
            {
                uff.Merge(i, i % 2);
            }

            (uff.Root(0) % 2).Is(0);
            (uff.Root(1) % 2).Is(1);

            for (var i = 0; i < n; i++)
            {
                uff.Root(i).Is(uff.Root(i % 2));
                uff.Connects(i % 2, i).Is(true);
            }
        }
    }
}
