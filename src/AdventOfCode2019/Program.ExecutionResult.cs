namespace AoC19
{
    partial class Program
    {
        private interface IExecutionResult
        {
            int InstructionPointerOffset { get; }
        }

        private class FunctionExecutionResult : IExecutionResult
        {
            public FunctionExecutionResult(
                int value,
                int address,
                int instructionPointerOffset)
            {
                Value = value;
                Address = address;
                InstructionPointerOffset = instructionPointerOffset;
            }

            public int Value { get; }
            public int Address { get; }
            public int InstructionPointerOffset { get; }
        }

        private class ActionExecutionResult : IExecutionResult
        {
            public ActionExecutionResult(int instructionPointerOffset)
            {
                InstructionPointerOffset = instructionPointerOffset;
            }

            public int InstructionPointerOffset { get; }
        }
    }
}