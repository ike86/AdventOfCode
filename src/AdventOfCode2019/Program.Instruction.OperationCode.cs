using System.Collections.Generic;
using System.Linq;

namespace AoC19
{
    partial class Program
    {
        private partial class Instruction
        {
            private class OperationCode
            {
                private readonly int opCode;

                public OperationCode(int opCode)
                {
                    this.opCode = opCode;
                }

                public int Value => opCode % 100;

                public IEnumerable<ParameterMode> ParameterModes =>
                    ((opCode - Value) / 100).ToString()
                    .Reverse()
                    .Select(ToParameterMode)
                    .ToArray();

                private ParameterMode ToParameterMode(char ch)
                {
                    return (ParameterMode)int.Parse(ch.ToString());
                }
            }
        }
    }
}