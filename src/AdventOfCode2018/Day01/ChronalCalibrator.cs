using System;
using System.Linq;

namespace AdventOfCode2018.Day01
{
    internal class ChronalCalibrator
    {
        public ChronalCalibrator()
        {
        }

        internal int Calibrate(string frequencyChanges)
        {
            if (frequencyChanges == string.Empty)
            {
                return 0;
            }

            var signToken = frequencyChanges.First();
            int sign = default;
            switch (signToken)
            {
                case '+':
                    sign = 1;
                    break;

                case '-':
                    sign = -1;
                    break;

                default:
                    break;
            }

            var frequencyToken = frequencyChanges.Substring(1);

            return int.Parse(frequencyToken) * sign;
        }
    }
}