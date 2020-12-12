using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day08
    {
        private const string Exmaple =
            @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";

        [Fact]
        public void Can_parse_Noop()
        {
            new BootCode(Exmaple).Instructions
                .Should().BeEquivalentTo(
                    new Instruction("nop", 0));
        }
    }

    public class Instruction
    {
        public Instruction(string instructionCode, int i)
        {
            InstructionCode = instructionCode;
            I = i;
        }

        public string InstructionCode { get; }

        public int I { get; }
    }

    public class BootCode
    {
        public BootCode(string raw)
        {
            var tokens =
                raw.Split(Environment.NewLine)
                .First()
                .Split(' ');
            Instructions = new[] { new Instruction(tokens[0], int.Parse(tokens[1])) };
        }

        public IEnumerable<Instruction> Instructions { get; }
    }
}