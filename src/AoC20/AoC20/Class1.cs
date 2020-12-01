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
        
        [Fact]
        public void Test_puzzle_input()
        {
            var expenseReport =
                ExpenseReport.Parse(PuzzleInput);

            var fix = expenseReport.GetFix();

            fix.Should().Be(786811);
        }

        private const string PuzzleInput =
            @"1863
1750
1767
1986
1180
1719
1946
1866
1939
1771
1766
1941
1728
1322
1316
1775
1776
1742
1726
1994
1949
1318
1223
1741
1816
1111
1991
1406
1230
1170
1823
1792
1148
1953
1706
1724
1307
1844
1943
1862
1812
1286
1837
1785
1998
1938
1248
1822
1829
1903
1131
1826
1892
1143
1898
1981
1225
1980
1850
1885
324
289
1914
1249
1848
1995
1962
1875
1827
1931
1244
1739
1897
1687
1907
1867
1922
1972
1842
1757
1610
1945
1835
1894
1265
1872
1963
1712
891
1813
1800
1235
1879
1732
1522
1335
1936
1830
1772
1700
2005
1253
1836
1935
1137
1951
1849
1883
1192
1824
1918
1965
1759
1195
1882
1748
1168
1200
1761
1896
527
1769
1560
1947
1997
1461
1828
1801
1877
1900
1924
1782
1718
515
1814
1744
1126
1791
1149
1932
1690
1707
1808
1957
1313
1132
1942
1934
1798
2009
1708
1774
1710
1797
1747
959
1955
1717
1716
1290
1654
1857
1968
1874
1853
1175
1493
1425
1125
1973
1790
467
1804
987
1944
2001
1895
1917
1218
1147
1884
1819
1179
1859
620
1219
2008
1871
1852
1263
1751
1989
1381
1250
1754
1725
1665
1352
1805
1325";
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