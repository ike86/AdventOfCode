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
            new BootCode(Exmaple).Instructions.Take(1)
                .Should().BeEquivalentTo(
                    new Instruction("nop", 0));
        }
        
        [Fact]
        public void Can_parse_Accumulate()
        {
            new BootCode(Exmaple).Instructions.Skip(1)
                .Should().BeEquivalentTo(
                    new Instruction("acc", 1));
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
            
            var tokens2 =
                raw.Split(Environment.NewLine)
                    .Skip(1)
                    .First()
                    .Split(' ');
            Instructions =
                new[]
                    {
                        new Instruction(tokens[0], int.Parse(tokens[1])),
                        new Instruction(tokens2[0], int.Parse(tokens2[1])),
                };
        }

        public IEnumerable<Instruction> Instructions { get; }
    }
}