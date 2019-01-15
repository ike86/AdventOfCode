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

        [Fact]
        public void time_after_shift_beginning_has_guard_id_of_the_guard()
        {
            var records = ReposeRecord("[1518-11-03 00:05] Guard #10 begins shift");

            var id = records.Of(00, 05 +1).GuardId;

            id.Should().Be(10);
        }

        [Fact]
        public void time_before_shift_beginning_has_no_guard_id()
        {
            var records = ReposeRecord("[1518-11-03 00:05] Guard #10 begins shift");

            var id = records.Of(00, 05 - 1).GuardId;

            id.Should().BeNull();
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
                            [3]),
                        From = int.Parse(
                            v.Split(new[] { ':', ']' }, StringSplitOptions.RemoveEmptyEntries)
                            [1])
                    };
            }

            internal GuardRecord Of(int v1, int v2)
            {
                if(this.guardRecord.From <= v2)
                    return this.guardRecord;

                return new GuardRecord { GuardId = null };
            }

            internal class GuardRecord
            {
                public int? GuardId { get; internal set; }

                public int From { get; internal set; }
            }
        }
    }
}