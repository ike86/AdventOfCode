using System;
using FluentAssertions;
using Xunit;

namespace AoC18.Day04
{
    public class Records_of
    {
        [Fact]
        public void _shift_beginning_has_guard_id_of_the_guard()
        {
            var records = ReposeRecord("[1518-11-03 00:05] Guard #10 begins shift");

            var id = records.Of(00, 05).GuardId;

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