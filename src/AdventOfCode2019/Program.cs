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
                        Code[er.Address] = er.Value;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new ProgramException(
                        $"Program running failed while executing {intruction.OperationName}.",
                        ex);
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

                operation = Create(instructionPointer, opCode, readInput, writeOutput);
            }

            public int AddressOfResult => code[instructionPointer + operation.NumberOfParameters + 1];

            public string OperationName => operation.GetType().Name;

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
                            value: function.Execute(GetArguments()),
                            address: AddressOfResult,
                            operation.InstructionPointerOffset);
                }
                else if (operation is IActionWithSideEffect actionWithSideEffect)
                {
                    var result = actionWithSideEffect.Execute(GetArguments());

                    return
                        new FunctionExecutionResult(
                            value: result.Value,
                            address: result.Address,
                            actionWithSideEffect.InstructionPointerOffset);
                }

                throw new InvalidOperationException($"{opCode.Value} is not supported.");
            }

            private static IOperation Create(
                int instructionPointer,
                OperationCode opCode,
                Func<int> readInput,
                Action<int> writeOutput)
            {
                return opCode.Value switch
                {
                    1 => new Addition(),
                    2 => new Multiplication(),
                    3 => new Input(readInput),
                    4 => new Output(writeOutput),
                    5 => new JumpIfTrue(instructionPointer),
                    6 => new JumpIfFalse(instructionPointer),
                    7 => new LessThan(),
                    99 => new Halting(),
                    _ => throw new InvalidOperationException(
                        $"Opcode {opCode.Value} is not supported"),
                };
            }

            private IEnumerable<Func<int>> GetArguments()
            {
                if (operation.NumberOfParameters == 0)
                    return Enumerable.Empty<Func<int>>();


                return Enumerable
                    .Range(1, operation.NumberOfParameters)
                    .Zip(GetParameterModesOrDefault(), (i, m) => (i, parameterMode: m))
                    .Select(t => NthOperand(t.i, t.parameterMode));
            }

            private ParameterMode[] GetParameterModesOrDefault()
            {
                return
                    opCode.ParameterModes
                    .Concat(
                        Enumerable.Range(
                            0,
                            operation.NumberOfParameters - opCode.ParameterModes.Count())
                        .Select(_ => default(ParameterMode)))
                    .ToArray();
            }

            private Func<int> NthOperand(int n, ParameterMode parameterMode)
            {
                if(parameterMode == ParameterMode.Immediate)
                    return () => code[instructionPointer + n];
                else if (parameterMode == ParameterMode.Position)
                    return () => code[AddressOfNthOperand(n)];

                throw new InvalidOperationException(
                    $"Parameter mode {parameterMode} is not supported.");
            }

            private int AddressOfNthOperand(int n) => code[instructionPointer + n];


            private enum ParameterMode
            {
                Position = 0,
                Immediate = 1,
            }
        }

        private class ProgramHalted : Exception { }
    }

    public class ProgramException : Exception
    {
        public ProgramException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}