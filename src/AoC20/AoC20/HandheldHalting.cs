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

        [Fact]
        public void Executing_Noop_adds_it_to_executed()
        {
            new BootCode(Exmaple).Execute(1).ExecutedInstructions.FirstOrDefault()
                .Should().BeOfType(typeof(Noop));
        }
        
        [Fact]
        public void NextInstruction_is_the_first_one_by_default()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.NextInstruction
                .Should().Be(bootCode.Instructions.First());
        }
        
        [Fact]
        public void Executing_Noop_moves_next_instruction_forward()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.Execute(1).NextInstruction
                .Should().Be(bootCode.Instructions.Skip(1).First());
        }
        
        [Fact]
        public void Accumulator_is_zero_by_default()
        {
            new BootCode(Exmaple).Accumulator
                .Should().Be(0);
        }
        
        [Fact]
        public void Executing_Accumulate_changes_accumulator_by_its_value()
        {
            var bootCode = new BootCode(Exmaple);
            bootCode.Execute(2).Accumulator
                .Should().Be(bootCode.Accumulator + 1);
        }
        
        [Fact]
        public void Executing_Accumulate_adds_it_to_executed()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.Execute(2).ExecutedInstructions
                .Should().ContainEquivalentOf(
                    bootCode.Instructions.Skip(1).First(),
                    opt => opt.RespectingRuntimeTypes());
        }
        
        [Fact]
        public void Executing_Accumulate_moves_instruction_pointer_forward()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.Execute(2).NextInstruction
                .Should().Be(bootCode.Instructions.Skip(2).First());
        }
        
        [Fact]
        public void Executing_Jump_moves_instruction_pointer_by_its_offset()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.Execute(3).NextInstruction
                .Should().Be(bootCode.Instructions.Skip(6).First());
        }
        
        [Fact]
        public void Executing_Jump_adds_it_to_executed()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.Execute(3).ExecutedInstructions
                .Should().ContainEquivalentOf(
                    bootCode.Instructions.Skip(2).First(),
                    opt => opt.RespectingRuntimeTypes());
        }
        
        [Fact]
        public void Executing_with_infinite_loop_protection_halts_with_5_as_accumulator()
        {
            new BootCode(Exmaple).ExecuteWithInfiniteLoopProtection().Accumulator
                .Should().Be(5);
        }
        
        [Fact]
        public void Solve_puzzle()
        {
            new BootCode(PuzzleInput.ForDay08).ExecuteWithInfiniteLoopProtection().Accumulator
                .Should().Be(1859);
        }
        
        [Fact]
        public void Solve_puzzle_part_2()
        {
            var fixAttempts = GetFixAttemptsOf(PuzzleInput.ForDay08);
            
            fixAttempts.Select(x => x.ExecuteWithInfiniteLoopProtection())
                .Should().Contain(x => x.Terminated);
        }

        private IEnumerable<BootCode> GetFixAttemptsOf(string raw)
        {
            yield return new BootCode(raw);
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

    public static class InstructionExtensions
    {
        public static TResult Convert<TResult>(
            this IInstruction instruction,
            Func<Noop, TResult> noopCase,
            Func<Accumulate, TResult> accumulateCase,
            Func<Jump, TResult> jumpCase)
        {
            return instruction switch
            {
                Noop n => noopCase(n),
                Accumulate a => accumulateCase(a),
                Jump j => jumpCase(j),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }

    public class BootCode
    {
        public BootCode(string raw) : this(ParseInstructions(raw))
        {
        }
        
        public BootCode(IEnumerable<IInstruction> instructions)
        {
            Instructions = instructions.ToArray();
            ExecutedInstructions = Enumerable.Empty<IInstruction>();
        }

        private BootCode(BootCode old, int offset)
        {
            Instructions = old.Instructions;
            ExecutedInstructions = old.ExecutedInstructions.Append(old.NextInstruction);
            NextInstructionPointer = old.NextInstructionPointer + offset;
            Accumulator = old.Accumulator;
            Terminated = old.Terminated;
        }

        public IEnumerable<IInstruction> Instructions { get; }
        
        public IEnumerable<IInstruction> ExecutedInstructions { get; private set; }

        public int NextInstructionPointer { get; private set; }

        public IInstruction NextInstruction => Instructions.ElementAt(NextInstructionPointer);

        public int Accumulator { get; private set; }

        public bool Terminated { get; private set; }

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

        public BootCode Execute(int numberOfInstructions) =>
            Execute(numberOfInstructions, withInfiniteLoopProtection: false);

        public BootCode ExecuteWithInfiniteLoopProtection() => Execute(0, withInfiniteLoopProtection: true);

        public BootCode Execute(int numberOfInstructions, bool withInfiniteLoopProtection)
        {
            var bootCode = this;
            for (var i = 0; i < numberOfInstructions || withInfiniteLoopProtection; i++)
            {
                if (withInfiniteLoopProtection
                    && bootCode.ExecutedInstructions.Any(ins => ins == bootCode.NextInstruction))
                {
                    return bootCode;
                }

                bootCode =
                    bootCode.NextInstruction.Convert(
                        noop => new BootCode(bootCode, offset: 1),
                        accumulate =>
                            new BootCode(bootCode, offset: 1)
                            {
                                Accumulator = bootCode.Accumulator + accumulate.Change,
                            },
                        jump => new BootCode(bootCode, jump.Offset));
            }

            return new BootCode(bootCode, offset: 0) {Terminated = true};
        }
    }
}