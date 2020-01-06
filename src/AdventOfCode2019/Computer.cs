using System;
using System.Linq;

namespace AoC19
{
    class Computer
    {
        private readonly Func<int> readInput;

        public Computer(string programCode, Func<int> readInput)
        {
            Memory =
                programCode
                .Split(',')
                .Select(int.Parse)
                .ToArray();
            this.readInput = readInput;
        }

        public int[] Memory { get; private set; }

        public int Run()
        {
            var program = new Program(Memory, readInput);
            program.Run();
            return program.Code[0];
        }
    }
}