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
            new BootCode(Exmaple).Instructions.First()
                .Should().BeOfType(typeof(Noop));
        }

        [Fact]
        public void Can_parse_Accumulate()
        {
            new BootCode(Exmaple).Instructions.Skip(1).Take(1)
                .Should().BeEquivalentTo(new Accumulate(1));
        }

        [Fact]
        public void Can_parse_Jump()
        {
            new BootCode(Exmaple).Instructions.Skip(2).Take(1)
                .Should().BeEquivalentTo(new Jump(4));
        }
    }

    public interface IInstruction
    {
    }

    public class Noop : IInstruction
    {
    }

    public class Accumulate : IInstruction
    {
        public Accumulate(int change)
        {
            Change = change;
        }

        public int Change { get; }
    }

    public class Jump : IInstruction
    {
        public Jump(int offset)
        {
            Offset = offset;
        }

        public int Offset { get; }
    }

    public class BootCode
    {
        public BootCode(string raw)
        {
            Instructions = ParseInstructions(raw);
        }

        public IEnumerable<IInstruction> Instructions { get; }
        
        private static IInstruction[] ParseInstructions(string raw)
        {
            return raw.Split(Environment.NewLine)
                .Select(line => line.Split(' '))
                .Select(ParseInstruction)
                .ToArray();
        }

        private static IInstruction ParseInstruction(string[] tokens)
        {
            return tokens[0] switch
            {
                "nop" => new Noop(),
                "jmp" => new Jump(int.Parse(tokens[1])),
                "acc" => new Accumulate(int.Parse(tokens[1])),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}