using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC19
{
    class Program
    {
        public Program(int[] code)
        {
            this.Code = code;
        }

        public int[] Code { get; }

        public void Run()
        {
            var instructionPointer = 0;

            while (true)
            {
                var intruction = new Instruction(Code, instructionPointer);

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

            public Instruction(IEnumerable<int> code, int instructionPointer)
            {
                this.code = EnsureHasTrailingPositions(code);
                this.instructionPointer = instructionPointer;
                operation = Create(OpCode);
            }

            public int AddressOfResult => code[instructionPointer + 3];

            private int AddressOfLeftOperand => code[instructionPointer + 1];

            private int AddressOfRightOperand => code[instructionPointer + 2];

            private Func<int> LeftOperand => () => code[AddressOfLeftOperand];

            private Func<int> RightOperand => () => code[AddressOfRightOperand];

            private int OpCode => code[instructionPointer + 0];

            public ExecutionResult Execute()
            {
                return
                    new ExecutionResult(
                        operation.Execute(LeftOperand, RightOperand),
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

            private static IOperation Create(int opCode)
            {
                if (opCode == 1) return new Addition();
                else if (opCode == 2) return new Multiplication();
                else if (opCode == 99) return new Halting();

                throw new InvalidOperationException(opCode.ToString());
            }

            private interface IOperation
            {
                int InstructionPointerOffset { get; }

                int Execute(Func<int> x, Func<int> y);
            }

            private class Addition : IOperation
            {
                public int InstructionPointerOffset => 4;

                // the number of operation arguments is still hard-coded,
                // which forces us to use lazy argument resolution in order to not fail on Halt
                public int Execute(Func<int> x, Func<int> y) => x() + y();
            }

            private class Multiplication : IOperation
            {
                public int InstructionPointerOffset => 4;

                public int Execute(Func<int> x, Func<int> y) => x() * y();
            }

            private class Halting : IOperation
            {
                public int InstructionPointerOffset => 0;

                public int Execute(Func<int> x, Func<int> y) => throw new ProgramHalted();
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