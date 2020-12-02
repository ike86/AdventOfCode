using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day01
    {
        private const string Example = @"1721
979
366
299
675
1456";

        [Fact]
        public void Part_1_example()
        {
            var expenseReport = ExpenseReport.Parse(Example);

            var fix = expenseReport.GetFix();

            fix.Should().Be(1721 * 299);
        }
        
        [Fact]
        public void Test_puzzle_input()
        {
            var expenseReport =
                ExpenseReport.Parse(PuzzleInput.ForDay01);

            var fix = expenseReport.GetFix();

            fix.Should().Be(786811);
        }
        
        [Fact]
        public void Part_2_example()
        {
            var expenseReport = ExpenseReport.Parse(Example);

            var fix = expenseReport.GetFix3();

            fix.Should().Be(979 * 366 * 675);
        }
    }

    public class ExpenseReport
    {
        private readonly IEnumerable<int> _expenses;

        private ExpenseReport(IEnumerable<int> expenses)
        {
            _expenses = expenses.ToArray();
        }

        public static ExpenseReport Parse(string raw)
        {
            var expenses =
                raw.Split(Environment.NewLine)
                    .Select(line => int.Parse(line));
            return new ExpenseReport(expenses);
        }

        public int GetFix()
        {
            var t =
                _expenses.SelectMany(
                        el => _expenses.Select(er => (left: el, right: er)))
                .First(tuple => tuple.left + tuple.right == 2020);
            return t.left * t.right;
        }

        public int GetFix3()
        {
            foreach (var e1 in _expenses)
            {
                foreach (var e2 in _expenses)
                {
                    foreach (var e3 in _expenses)
                    {
                        if (e1 + e2 + e3 == 2020)
                            return e1 * e2 * e3;
                    }
                }
            }

            throw new FixNotFoundException();
        }
    }

    public class FixNotFoundException : Exception
    {
    }
}