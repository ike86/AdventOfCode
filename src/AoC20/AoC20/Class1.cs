using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Class1
    {
        [Fact]
        public void Test()
        {
            var expenseReport =
                ExpenseReport.Parse(
@"1721
979
366
299
675
1456");

            var fix = expenseReport.GetFix();

            fix.Should().Be(1721 * 299);
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
    }
}