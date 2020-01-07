using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC19
{
    class Program
    {
        private readonly Func<int> readInput;

        public Program(int[] code, Func<int> readInput)
        {
            this.Code = code;
            this.readInput = readInput;
        }

        public int[] Code { get; }

        public void Run()
        {
            var instructionPointer = 0;

            while (true)
            {
                var intruction = new Instruction(Code, instructionPointer, readInput);

                ExecutionResult result;
                try
                {
                    result = intruction.Execute();
                    Code[intruction.AddressOfResult] = result.Value;
                }
                catch (ProgramHalted)
                {
                    break;
                }

                instructionPointer += result.InstructionPointerOffset;
            }
        }

        private class Instruction
        {
            private readonly int[] code;
            private readonly int instructionPointer;
            private readonly IOperation operation;

            public Instruction(IEnumerable<int> code, int instructionPointer, Func<int> readInput)
            {
                this.code = EnsureHasTrailingPositions(code);
                this.instructionPointer = instructionPointer;
                operation = Create(OpCode, readInput);
            }

            public int AddressOfResult => code[instructionPointer + 3];

            private int OpCode => code[instructionPointer + 0];

            public ExecutionResult Execute()
            {
                return
                    new ExecutionResult(
                        operation.Execute(GetArguments()),
                        operation.InstructionPointerOffset);
            }

            private int[] EnsureHasTrailingPositions(IEnumerable<int> code)
            {
                var c = code.ToArray();
                if (c.Count() % 4 != 0)
                {
                    return c.Concat(new[] { 0, 0, 0 }).ToArray();
                }

                return c;
            }

            private static IOperation Create(int opCode, Func<int> readInput)
            {
                if (opCode == 1) return new Addition();
                else if (opCode == 2) return new Multiplication();
                else if (opCode == 3) return new Input(readInput);
                else if (opCode == 99) return new Halting();

                throw new InvalidOperationException(opCode.ToString());
            }

            private IEnumerable<Func<int>> GetArguments()
            {
                if (operation.NumberOfParameters == 0)
                    return Enumerable.Empty<Func<int>>();

                return Enumerable
                    .Range(1, operation.NumberOfParameters)
                    .Select(i => NthOperand(i));
            }

            private Func<int> NthOperand(int n) => () => code[AddressOfNthOperand(n)];

            private int AddressOfNthOperand(int n) => code[instructionPointer + n];

            private interface IOperation
            {
                int NumberOfParameters { get; }

                int InstructionPointerOffset { get; }

                int Execute(IEnumerable<Func<int>> arguments);
            }

            private class Addition : IOperation
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

            private class Multiplication : IOperation
            {
                public int InstructionPointerOffset => 4;

                public int NumberOfParameters => 2;

                public int Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    return args[0]() * args[1]();
                }
            }

            private class Halting : IOperation
            {
                public int InstructionPointerOffset => 0;

                public int NumberOfParameters => 0;

                public int Execute(IEnumerable<Func<int>> arguments) => throw new ProgramHalted();
            }

            private class Input : IOperation
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
        }

        private class ExecutionResult
        {
            public ExecutionResult(
                int value,
                int instructionPointerOffset)
            {
                Value = value;
                InstructionPointerOffset = instructionPointerOffset;
            }

            public int Value { get; }

            public int InstructionPointerOffset { get; }
        }

        private class ProgramHalted : Exception { }
    }
}