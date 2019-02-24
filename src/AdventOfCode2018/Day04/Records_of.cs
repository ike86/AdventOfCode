using System;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AoC18.Day04
{
    public class Records_of
    {
        [Theory, AutoData]
        public void _shift_beginning_has_guard_id_of_the_guard(
            int year, int month, int day, int minute, int guardId)
        {
            var records = ReposeRecord(
                $"[{year}-{month}-{day} 00:{minute}] Guard #{guardId} begins shift");

            var id = records.Of(minute).GuardId;

            id.Should().Be(guardId);
        }

        [Theory, AutoData]
        public void time_after_shift_beginning_has_guard_id_of_the_guard(
            int year, int month, int day, int minute, int guardId)
        {
            var records = ReposeRecord(
                $"[{year}-{month}-{day} 00:{minute}] Guard #{guardId} begins shift");

            var id = records.Of(minute + 1).GuardId;

            id.Should().Be(guardId);
        }

        [Theory, AutoData]
        public void time_before_shift_beginning_has_no_guard_id(
            int year, int month, int day, int minute, int guardId)
        {
            var records = ReposeRecord(
                $"[{year}-{month}-{day} 00:{minute}] Guard #{guardId} begins shift");

            var id = records.Of(minute - 1).GuardId;

            id.Should().BeNull();
        }

        [Theory, AutoData]
        public void fallen_asleep_has_guard_id_of_the_guard(
            int year, int month, int day, int minute, int guardId, int delta)
        {
            var records = ReposeRecord(
                $"[{year}-{month}-{day} 00:{minute}] Guard #{guardId} begins shift",
                $"[{year}-{month}-{day} 00:{minute + delta}] falls asleep");

            var id = records.Of(minute + delta).GuardId;

            id.Should().Be(guardId);
        }

        private GuardRecords ReposeRecord(params string[] v)
        {
            return new GuardRecords(v);
        }

        private class GuardRecords
        {
            private GuardRecord guardRecord;

            public GuardRecords(params string[] v)
            {
                this.guardRecord =
                    new GuardRecord
                    {
                        GuardId = int.Parse(
                            v.Last().Split(new[] { ' ', '#' }, StringSplitOptions.RemoveEmptyEntries)
                            [3]),
                        From = int.Parse(
                            v.Last().Split(new[] { ':', ']' }, StringSplitOptions.RemoveEmptyEntries)
                            [1])
                    };
            }

            internal GuardRecord Of(int minute)
            {
                if (this.guardRecord.From <= minute)
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