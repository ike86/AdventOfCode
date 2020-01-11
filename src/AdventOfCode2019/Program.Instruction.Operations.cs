using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC19
{
    partial class Program
    {
        private partial class Instruction
        {
            private interface IOperation
            {
                int NumberOfParameters { get; }

                int InstructionPointerOffset { get; }
            }

            private interface IFunction : IOperation
            {
                int Execute(IEnumerable<Func<int>> arguments);
            }

            private interface IAction : IOperation
            {
                void Execute(IEnumerable<Func<int>> arguments);
            }

            private class Addition : IFunction
            {
                public int InstructionPointerOffset => 4;

                public int NumberOfParameters => 2;

                // the number of operation arguments is still hard-coded,
                // which forces us to use lazy argument resolution in order to not fail on Halt
                public int Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    return args[0]() + args[1]();
                }
            }

            private class Multiplication : IFunction
            {
                public int InstructionPointerOffset => 4;

                public int NumberOfParameters => 2;

                public int Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    return args[0]() * args[1]();
                }
            }

            private class Halting : IAction
            {
                public int InstructionPointerOffset => 0;

                public int NumberOfParameters => 0;

                public void Execute(IEnumerable<Func<int>> arguments) => throw new ProgramHalted();
            }

            private class Input : IFunction
            {
                private readonly Func<int> readInput;

                public Input(Func<int> readInput)
                {
                    this.readInput = readInput;
                }

                public int InstructionPointerOffset => 2;

                public int NumberOfParameters => 0;

                public int Execute(IEnumerable<Func<int>> arguments) => readInput();
            }

            private class Output : IAction
            {
                private readonly Action<int> writeOutput;

                public Output(Action<int> writeOutput)
                {
                    this.writeOutput = writeOutput;
                }

                public int InstructionPointerOffset => 2;

                public int NumberOfParameters => 1;

                public void Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    writeOutput(args[0]());
                }
            }
        }
    }
}