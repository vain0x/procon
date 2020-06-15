using Xunit;

namespace Procon
{
    public sealed class UnionFindTest
    {
        [Fact]
        public void Test_initial_state()
        {
            var n = 10;
            var uf = new UnionFind(n);
            for (var i = 0; i < n; i++)
            {
                uf.Root(i).Is(i);
            }
        }

        [Fact]
        public void Test_single_group_case()
        {
            var n = 10;

            var uf = new UnionFind(n);
            for (var i = 0; i + 1 < n; i++)
            {
                uf.Merge(i, i + 1);
            }

            var r = uf.Root(0);

            for (var i = 1; i < n; i++)
            {
                uf.Root(i).Is(r);
                uf.Connects(r, uf.Root(i)).Is(true);
            }
        }

        [Fact]
        public void Test_two_group_case()
        {
            var n = 10;

            var uf = new UnionFind(n);
            for (var i = 0; i < n; i++)
            {
                uf.Merge(i, i % 2);
            }

            (uf.Root(0) % 2).Is(0);
            (uf.Root(1) % 2).Is(1);

            for (var i = 0; i < n; i++)
            {
                uf.Root(i).Is(uf.Root(i % 2));
                uf.Connects(i % 2, i).Is(true);
            }
        }
    }
}
