using System;
using System.Linq;

namespace AoC19
{
    class Computer
    {
        private readonly Func<int> readInput;
        private readonly Action<int> writeOutput;

        public Computer(string programCode, Func<int> readInput, Action<int> writeOutput)
        {
            Memory =
                programCode
                .Split(',')
                .Select(int.Parse)
                .ToArray();
            this.readInput = readInput;
            this.writeOutput = writeOutput;
        }

        public int[] Memory { get; private set; }

        public int Run()
        {
            var program = new Program(Memory, readInput, writeOutput);
            program.Run();
            return program.Code[0];
        }
    }
}