using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2019
{
    public class Day01
    {
        /*
           The Elves quickly load you into a spacecraft and prepare to launch.

           At the first Go / No Go poll, every Elf is Go until the Fuel Counter-Upper.
           They haven't determined the amount of fuel required yet.

           Fuel required to launch a given module is based on its mass.
           Specifically, to find the fuel required for a module,
           take its mass, divide by three, round down, and subtract 2.

           For example:

           For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
           For a mass of 14, dividing by 3 and rounding down still yields 4,
           so the fuel required is also 2.
           For a mass of 1969, the fuel required is 654.
           For a mass of 100756, the fuel required is 33583.
           The Fuel Counter-Upper needs to know the total fuel requirement.
           To find it, individually calculate the fuel needed for the mass of each module
           (your puzzle input), then add together all the fuel values.

           What is the sum of the fuel requirements for all of the modules on your spacecraft?
         */

        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void Calculate_fuel_required_based_on_mass_of_a_module(int moduleMass, int fuelRequired)
        {
            FuelRequiredFor(moduleMass).Should().Be(fuelRequired);
        }

        private int FuelRequiredFor(int moduleMass)
        {
            return (moduleMass / 3) -2;
        }

        [Theory]
        [InlineData(MyInput)]
        public void Calculate_total_fuel_required(string massOfModules)
        {
            FuelRequiredFor(massOfModules).Should().Be(3390830);
        }

        private int FuelRequiredFor(string massOfModules)
        {
            return
                massOfModules
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(s => int.Parse(s))
                .Select(FuelRequiredFor)
                .Sum();
        }

        private const string MyInput =
            @"80740
103617
86598
135938
98650
84982
79253
122436
119679
89758
131375
112500
111603
112563
111174
114961
131423
144920
56619
94542
94533
116453
78286
70985
91005
116346
137141
90815
68806
61549
116078
53067
116991
58210
54878
98184
108532
130796
144893
137845
57481
133204
125789
99573
121718
73905
105746
134401
136337
74788
147758
128636
97457
84983
57520
125839
68230
106761
147436
95604
142427
82718
81692
138713
53145
90348
69312
139908
139836
91889
126399
130204
103643
70653
81236
99555
64461
128172
122914
71321
141502
124121
67214
64612
78519
69582
124489
95904
124274
66556
140500
112775
114855
57332
50072
79830
126844
67276
137841
108654";
    }
}
