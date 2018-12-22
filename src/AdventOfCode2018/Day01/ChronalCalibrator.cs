using System;

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

            var frequencyToken = frequencyChanges.Substring(1);
            return int.Parse(frequencyToken);
        }
    }
}