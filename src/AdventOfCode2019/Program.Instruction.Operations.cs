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

            private interface IActionWithSideEffect : IOperation
            {
                Assignment Execute(IEnumerable<Func<int>> arguments);
            }

            private class Assignment
            {
                public int Value { get; internal set; }
                public int Address { get; internal set; }
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

            private class JumpIfTrue : IAction
            {
                private readonly int instructionPointer;

                public JumpIfTrue(int instructionPointer)
                {
                    this.instructionPointer = instructionPointer;
                }

                public int NumberOfParameters => 2;

                public int InstructionPointerOffset { get; private set; } = 3;

                public void Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    if (args[0]() != 0)
                    {
                        InstructionPointerOffset = args[1]() - instructionPointer;
                    }
                }
            }

            private class JumpIfFalse : IAction
            {
                private readonly int instructionPointer;

                public JumpIfFalse(int instructionPointer)
                {
                    this.instructionPointer = instructionPointer;
                }

                public int NumberOfParameters => 2;

                public int InstructionPointerOffset { get; private set; } = 3;

                public void Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    if (args[0]() == 0)
                    {
                        InstructionPointerOffset = args[1]() - instructionPointer;
                    }
                }
            }

            private class LessThan : IActionWithSideEffect
            {
                public int InstructionPointerOffset => 4;

                public int NumberOfParameters => 3;

                public Assignment Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    int result;
                    if (args[0]() < args[1]())
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }

                    return
                        new Assignment
                        {
                            Value = result,
                            Address = args[2](),
                        };
                }
            }

            private class Equals : IActionWithSideEffect
            {
                public int NumberOfParameters => 3;

                public int InstructionPointerOffset => 4;

                public Assignment Execute(IEnumerable<Func<int>> arguments)
                {
                    var args = arguments.ToArray();
                    if (args[0]() == args[1]())
                    {
                        return
                            new Assignment
                            {
                                Value = 1,
                                Address = args[2](),
                            };
                    }
                    else
                    {
                        return
                            new Assignment
                            {
                                Value = 0,
                                Address = args[2](),
                            };
                    }
                }
            }
        }
    }
}