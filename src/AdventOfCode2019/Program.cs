using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC19
{
    class Program
    {
        private readonly Func<int> readInput;
        private readonly Action<int> writeOutput;

        public Program(int[] code, Func<int> readInput, Action<int> writeOutput)
        {
            this.Code = code;
            this.readInput = readInput;
            this.writeOutput = writeOutput;
        }

        public int[] Code { get; }

        public void Run()
        {
            var instructionPointer = 0;

            while (true)
            {
                var intruction =
                    new Instruction(Code, instructionPointer, readInput, writeOutput);

                ExecutionResult result;
                try
                {
                    result = intruction.Execute();
                    if (result is ActionExecutionResult)
                    {
                    }
                    else
                    {
                        Code[intruction.AddressOfResult] = result.Value;
                    }
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

            public Instruction(
                IEnumerable<int> code,
                int instructionPointer,
                Func<int> readInput,
                Action<int> writeOutput)
            {
                this.code = code.ToArray();
                this.instructionPointer = instructionPointer;
                operation = Create(OpCode, readInput, writeOutput);
            }

            public int AddressOfResult => code[instructionPointer + operation.NumberOfParameters + 1];

            private int OpCode => code[instructionPointer + 0];

            public ExecutionResult Execute()
            {
                if (operation is Output)
                {
                    _ = operation.Execute(GetArguments());
                    return
                        new ActionExecutionResult(operation.InstructionPointerOffset);
                }

                return
                    new ExecutionResult(
                        operation.Execute(GetArguments()),
                        operation.InstructionPointerOffset);
            }

            private static IOperation Create(
                int opCode,
                Func<int> readInput,
                Action<int> writeOutput)
            {
                if (opCode == 1) return new Addition();
                else if (opCode == 2) return new Multiplication();
                else if (opCode == 3) return new Input(readInput);
                else if (opCode == 4) return new Output(writeOutput);
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

            private class Output : IOperation
            {
                private readonly Action<int> writeOutput;

                public Output(Action<int> writeOutput)
                {
                    this.writeOutput = writeOutput;
                }

                public int InstructionPointerOffset => 2;

                public int NumberOfParameters => 1;

                public int Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    writeOutput(args[0]());

                    return -1;
                }
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

        private class ActionExecutionResult : ExecutionResult
        {
            public ActionExecutionResult(int instructionPointerOffset)
                : base(default, instructionPointerOffset)
            {

            }
        }

        private class ProgramHalted : Exception { }
    }
}