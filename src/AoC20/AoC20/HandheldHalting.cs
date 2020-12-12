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
            new BootCode(Exmaple).Instructions.Skip(1).Take(1)
                .Should().BeEquivalentTo(
                    new Instruction("acc", 1));
        }
        
        [Fact]
        public void Can_parse_Jump()
        {
            new BootCode(Exmaple).Instructions.Skip(2).Take(1)
                .Should().BeEquivalentTo(
                    new Instruction("jmp", 4));
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
            Instructions =
                raw.Split(Environment.NewLine)
                    .Select(line => line.Split(' '))
                    .Select(tokens => new Instruction(tokens[0], int.Parse(tokens[1])))
                    .ToArray();
        }

        public IEnumerable<Instruction> Instructions { get; }
    }
}