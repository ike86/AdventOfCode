using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AoC18.Day04
{
    public class Tests
    {
        [Fact]
        public void Test01()
        {
            var records = ReposeRecord("[1518-11-01 00:00] Guard #10 begins shift");

            var guardRecord = records.Of(11, 01);
        }

        private GuardRecords ReposeRecord(string v)
        {
            return new GuardRecords();
        }

        private class GuardRecords
        {
            internal object Of(int v1, int v2)
            {
                return null;
            }
        }
    }
}
