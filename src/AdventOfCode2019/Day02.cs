using System;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AoC19
{
    public class Day02
    {
        /*
        --- Day 2: 1202 Program Alarm ---

        An Intcode program is a list of integers separated by commas (like 1,0,0,3,99).
        To run one, start by looking at the first integer (called position 0).
        Here, you will find an opcode - either 1, 2, or 99.
        The opcode indicates what to do;
        for example, 99 means that the program is finished and should immediately halt.
        Encountering an unknown opcode means something went wrong.

        Opcode 1 adds together numbers read from two positions and stores the result in a third position.
        The three integers immediately after the opcode tell you these three positions
        - the first two indicate the positions from which you should read the input values,
        and the third indicates the position at which the output should be stored.

        For example, if your Intcode computer encounters 1,10,20,30,
        it should read the values at positions 10 and 20, add those values,
        and then overwrite the value at position 30 with their sum.
        */

        [Fact]
        public void Run_can_add_and_save_result_to_the_same_line()
        {
            Run("1,1,2,0,99,0,0,0").Should().Be(3);
        }

        /*
        Opcode 2 works exactly like opcode 1, except it multiplies the two inputs instead of adding them.
        Again, the three integers after the opcode indicate where the inputs and outputs are, not their values.
        */

        [Fact]
        public void Run_can_multiply_and_save_result_to_the_same_line()
        {
            Run("2,1,2,0,99,0,0,0").Should().Be(2);
        }

        /*

        Once you're done processing an opcode, move to the next one by stepping forward 4 positions.

        For example, suppose you have the following program:

        1,9,10,3,2,3,11,0,99,30,40,50
        For the purposes of illustration, here is the same program split into multiple lines:

        1,9,10,3,
        2,3,11,0,
        99,
        30,40,50
        The first four integers, 1,9,10,3, are at positions 0, 1, 2, and 3.
        Together, they represent the first opcode (1, addition),
        the positions of the two inputs (9 and 10), and the position of the output (3).
        To handle this opcode, you first need to get the values at the input positions:
        position 9 contains 30, and position 10 contains 40. Add these numbers together to get 70.
        Then, store this value at the output position; here, the output position (3) is at position 3,
        so it overwrites itself. Afterward, the program looks like this:

        1,9,10,70,
        2,3,11,0,
        99,
        30,40,50
        Step forward 4 positions to reach the next opcode, 2.
        This opcode works just like the previous, but it multiplies instead of adding.
        The inputs are at positions 3 and 11; these positions contain 70 and 50 respectively.
        Multiplying these produces 3500; this is stored at position 0:

        3500,9,10,70,
        2,3,11,0,
        99,
        30,40,50
        Stepping forward 4 more positions arrives at opcode 99, halting the program.
        */

        [Fact]
        public void Run_executes_multiple_lines()
        {
            Run(
                @"1,9,10,3,
                2,3,11,0,
                99,
                30,40,50")
                .Should().Be(3500);
        }

        [Fact]
        public void Run_can_halt()
        {
            Run("99,10,20,0").Should().Be(99);
        }

        private int Run(string programCode)
        {
            return new Computer(programCode).Run();
        }

        /*

        Here are the initial and final states of a few more small programs:

        1,0,0,0,99 becomes 2,0,0,0,99 (1 + 1 = 2).
        2,3,0,3,99 becomes 2,3,0,6,99 (3 * 2 = 6).
        2,4,4,5,99,0 becomes 2,4,4,5,99,9801 (99 * 99 = 9801).
        1,1,1,4,99,5,6,0,99 becomes 30,1,1,4,2,5,6,0,99.
        */

        [Theory]
        [InlineData("1,0,0,0,99", 0, 2)]
        [InlineData("2,3,0,3,99", 3, 6)]
        [InlineData("2,4,4,5,99,0", 5, 9801)]
        [InlineData("1,1,1,4,99,5,6,0,99", 0, 30)]
        public void Run_works_on_a_few_more_small_programs(
            string programCode,
            int addressOfExpected,
            int expected)
        {
            var computer = new Computer(programCode);

            _ = computer.Run();

            computer.Memory[addressOfExpected].Should().Be(expected);
        }

        /*
        Once you have a working computer,
        the first step is to restore the gravity assist program (your puzzle input)
        to the "1202 program alarm" state it had just before the last computer caught fire.
        To do this, before running the program,
        replace position 1 with the value 12 and replace position 2 with the value 2.
        What value is left at position 0 after the program halts?
         */
        private static string MyPuzzleInput = "1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,13,1,19,1,5,19,23,2,10,23,27,1,27,5,31,2,9,31,35,1,35,5,39,2,6,39,43,1,43,5,47,2,47,10,51,2,51,6,55,1,5,55,59,2,10,59,63,1,63,6,67,2,67,6,71,1,71,5,75,1,13,75,79,1,6,79,83,2,83,13,87,1,87,6,91,1,10,91,95,1,95,9,99,2,99,13,103,1,103,6,107,2,107,6,111,1,111,2,115,1,115,13,0,99,2,0,14,0";

        [Fact]
        public void Run_solves_day2_part1_puzzle()
        {
            var computer = new Computer(MyPuzzleInput);
            computer.Memory[1] = 12;
            computer.Memory[2] = 2;

            computer.Run().Should().Be(4714701);
        }

        /*
        Intcode programs are given as a list of integers;
        these values are used as the initial state for the computer's memory.
        When you run an Intcode program, make sure to start by initializing memory to the program's values.
        A position in memory is called an address (for example, the first value in memory is at "address 0").

        // TODO uncovered terminology
        Opcodes (like 1, 2, or 99) mark the beginning of an instruction.
        The values used immediately after an opcode, if any, are called the instruction's parameters.
        For example, in the instruction 1,2,3,4, 1 is the opcode; 2, 3, and 4 are the parameters.
        The instruction 99 contains only an opcode and has no parameters.

        The address of the current instruction is called the instruction pointer; it starts at 0.
        After an instruction finishes, the instruction pointer increases by the number of values in the instruction;
        until you add more instructions to the computer,
        this is always 4 (1 opcode + 3 parameters) for the add and multiply instructions.
        (The halt instruction would increase the instruction pointer by 1, but it halts the program instead.)
        */

        [Theory, AutoData]
        public void Array_AsEnumerable_ToArray_not_returns_the_original_array_but_a_copy(int[] array)
        {
            array.AsEnumerable().ToArray().Should().NotBeSameAs(array);
        }

        /*
        To complete the gravity assist, you need to determine what pair of inputs produces the output 19690720."

        The inputs should still be provided to the program by replacing the values at addresses 1 and 2,
        just like before. In this program, the value placed in address 1 is called the noun,
        and the value placed in address 2 is called the verb.
        Each of the two input values will be between 0 and 99, inclusive.

        Once the program has halted, its output is available at address 0, also just like before.
        Each time you try a pair of inputs, make sure you first reset the computer's memory
        to the values in the program (your puzzle input) - in other words,
        don't reuse memory from a previous attempt.

        Find the input noun and verb that cause the program to produce the output 19690720.
        What is 100 * noun + verb? (For example, if noun=12 and verb=2, the answer would be 1202.)
        */

        [Fact]
        public void Pair_of_inputs_producing_19690720_should_be()
        {
            var (noun, verb) = PairOfInputsProducing(19690720, MyPuzzleInput2);

            (100 * noun + verb).Should().Be(5121);
        }

        private (int noun, int verb) PairOfInputsProducing(int expectedResult, string programCode)
        {
            for (int noun = 0; noun < 99; noun++)
            {
                for (int verb = 0; verb < 99; verb++)
                {
                    var computer = new Computer(programCode);
                    computer.Memory[1] = noun;
                    computer.Memory[2] = verb;
                    var result = computer.Run();
                    if (result == expectedResult)
                    {
                        return (noun, verb);
                    }
                }
            }

            throw new ArgumentException();
        }

        private const string MyPuzzleInput2 = "1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,13,1,19,1,5,19,23,2,10,23,27,1,27,5,31,2,9,31,35,1,35,5,39,2,6,39,43,1,43,5,47,2,47,10,51,2,51,6,55,1,5,55,59,2,10,59,63,1,63,6,67,2,67,6,71,1,71,5,75,1,13,75,79,1,6,79,83,2,83,13,87,1,87,6,91,1,10,91,95,1,95,9,99,2,99,13,103,1,103,6,107,2,107,6,111,1,111,2,115,1,115,13,0,99,2,0,14,0";
    }
}