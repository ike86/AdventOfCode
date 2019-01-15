using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AoC18.Day04
{
    public class Tests
    {
        [Fact]
        public void Test01()
        {
            var records = ReposeRecord("[1518-11-01 00:00] Guard #10 begins shift");

            var id = records.Of(11, 01).GuardId;

            id.Should().Be(10);
        }

        private GuardRecords ReposeRecord(string v)
        {
            return new GuardRecords(v);
        }

        private class GuardRecords
        {
            private GuardRecord guardRecord;

            public GuardRecords(string v)
            {
                this.guardRecord =
                    new GuardRecord
                    {
                        GuardId = int.Parse(
                            v.Split(new[] { ' ', '#' }, StringSplitOptions.RemoveEmptyEntries)
                            [3])
                    };
            }

            internal GuardRecord Of(int v1, int v2)
            {
                return this.guardRecord;
            }

            internal class GuardRecord
            {
                public int GuardId { get; internal set; }
            }
        }
    }
}
