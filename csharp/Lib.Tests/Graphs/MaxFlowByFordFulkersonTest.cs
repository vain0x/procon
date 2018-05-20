using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Procon
{
    public sealed class MaxFlowByFordFulkersonTest
    {
        [Fact]
        public void Test()
        {
            var ff = new MaxFlowByFordFulkerson(6);
            ff.Insert(0, 1, 1);
            ff.Insert(0, 2, 9);
            ff.Insert(0, 3, 3);
            ff.Insert(0, 4, 3);
            ff.Insert(1, 3, 9);
            ff.Insert(2, 4, 3);
            ff.Insert(3, 1, 9);
            ff.Insert(3, 5, 9);
            ff.Insert(4, 5, 1);

            ff.MaxFlow(0, 5).Is(5);

            // Flow 1. 0 -> 3 -> 5 (+4)
            // Flow 2. 0 -> 2 -> 4 -> 5 (+1)
        }
    }
}
