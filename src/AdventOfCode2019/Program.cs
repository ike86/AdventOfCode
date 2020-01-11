using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC19
{
    partial class Program
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

                IExecutionResult result;
                try
                {
                    result = intruction.Execute();
                    if (result is FunctionExecutionResult er)
                    {
                        Code[intruction.AddressOfResult] = er.Value;
                    }
                }
                catch (ProgramHalted)
                {
                    break;
                }

                instructionPointer += result.InstructionPointerOffset;
            }
        }

        private partial class Instruction
        {
            private readonly int[] code;
            private readonly int instructionPointer;
            private readonly OperationCode opCode;
            private readonly IOperation operation;

            public Instruction(
                IEnumerable<int> code,
                int instructionPointer,
                Func<int> readInput,
                Action<int> writeOutput)
            {
                this.code = code.ToArray();
                this.instructionPointer = instructionPointer;
                opCode = new OperationCode(this.code[instructionPointer + 0]);

                operation = Create(OpCode, readInput, writeOutput);
            }

            public int AddressOfResult => code[instructionPointer + operation.NumberOfParameters + 1];

            private int OpCode => opCode.Value;

            private class OperationCode
            {
                public OperationCode(int opCode)
                {
                    Value = opCode;
                }

                public int Value { get; set; }
            }

            public IExecutionResult Execute()
            {
                if (operation is IAction action)
                {
                    action.Execute(GetArguments());
                    return
                        new ActionExecutionResult(operation.InstructionPointerOffset);
                }
                else if (operation is IFunction function)
                {
                    return
                        new FunctionExecutionResult(
                            function.Execute(GetArguments()),
                            operation.InstructionPointerOffset);
                }

                throw new InvalidOperationException($"{OpCode} is not supported.");
            }

            private static IOperation Create(
                int opCode,
                Func<int> readInput,
                Action<int> writeOutput)
            {
                opCode = opCode % 100;

                if (opCode == 1) return new Addition();
                else if (opCode == 2) return new Multiplication();
                else if (opCode == 3) return new Input(readInput);
                else if (opCode == 4) return new Output(writeOutput);
                else if (opCode == 99) return new Halting();

                throw new InvalidOperationException($"Opcode {opCode} is not supported");
            }

            private IEnumerable<Func<int>> GetArguments()
            {
                if (operation.NumberOfParameters == 0)
                    return Enumerable.Empty<Func<int>>();

                var parameterModes =
                    ((OpCode - (OpCode % 100)) / 100).ToString()
                    .Reverse()
                    .ToArray();
                parameterModes =
                    parameterModes
                    .Concat(
                        Enumerable.Range(0, operation.NumberOfParameters - parameterModes.Count())
                        .Select(_ => '0'))
                    .ToArray();

                return Enumerable
                    .Range(1, operation.NumberOfParameters)
                    .Zip(parameterModes, (i, m) => (i, parameterMode: m))
                    .Select(t => NthOperand(t.i, t.parameterMode));
            }

            private Func<int> NthOperand(int n, char parameterMode)
            {
                if(parameterMode == '1')
                    return () => code[instructionPointer + n];
                else if (parameterMode == '0')
                    return () => code[AddressOfNthOperand(n)];

                throw new InvalidOperationException(
                    $"Parameter mode {parameterMode} is not supported.");
            }

            private int AddressOfNthOperand(int n) => code[instructionPointer + n];
        }

        private class ProgramHalted : Exception { }
    }
}