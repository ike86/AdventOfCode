using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AoC19
{
    public class Day04
    {
        private const int Start = 172930;
        private const int End = 683082;

        /*
         * --- Day 4: Secure Container ---
         * You arrive at the Venus fuel depot only to discover it's protected by a password.
         * The Elves had written the password on a sticky note, but someone threw it out.
         * 
         * However, they do remember a few key facts about the password:
         * 
         * It is a six-digit number.
         * The value is within the range 172930-683082.
         * Two adjacent digits are the same (like 22 in 122345).
         * Going from left to right, the digits never decrease;
         * they only ever increase or stay the same (like 111123 or 135679).
         * Other than the range rule, the following are true:
         * 
         * 111111 meets these criteria (double 11, never decreases).
         * 223450 does not meet these criteria (decreasing pair of digits 50).
         * 123789 does not meet these criteria (no double).
         * How many different passwords within the range given in your puzzle input meet these criteria?
         */

        [Fact]
        void Is_a_six_digit_number()
        {
            IEnumerable<int> passwords = PossiblePasswords();

            passwords.Should().OnlyContain(p => p.ToString().Length == 6);
        }

        [Theory, AutoData]
        void Is_within_range(
            ////[Range(Start, End)]int expected,
            int deltaBelowRange,
            int deltaAboveRange)
        {
            IEnumerable<int> passwords = PossiblePasswords().ToArray();

            using var a = new AssertionScope();
            passwords.Should().NotContain(Start - deltaBelowRange);
            passwords.Should().NotContain(Start - 1);

            // TODO: how to fix these?
            ////passwords.Should().Contain(Start);
            ////passwords.Should().Contain(expected);
            ////passwords.Should().Contain(End);
            passwords.Should().NotContain(End + 1);
            passwords.Should().NotContain(End + deltaAboveRange);
        }

        [Fact]
        void Two_adjacent_digits_are_the_same()
        {
            IEnumerable<int> passwords = PossiblePasswords().ToArray();

            using var a = new AssertionScope();
            passwords.Should().Contain(234566);
            passwords.Should().NotContain(172934);
        }

        [Fact]
        void Digits_only_ever_increase_or_stay_the_same()
        {
            IEnumerable<int> passwords = PossiblePasswords().ToArray();

            using var a = new AssertionScope();
            passwords.Should().Contain(222222);
            passwords.Should().Contain(222345);
            passwords.Should().NotContain(222221);
        }

        private IEnumerable<int> PossiblePasswords()
        {
            return Enumerable
                .Range(Start, End - Start + 1)
                .Where(TwoAdjacentDigitsAreTheSame)
                .Where(DigitsOnlyEverIncreaseOrStayTheSame);
        }

        private static bool TwoAdjacentDigitsAreTheSame(int p)
        {
            return p.ToString()
                .Zip(
                    p.ToString().Substring(1) + " ",
                    (digit, nextDigit) => (digit, nextDigit))
                .Any(t => t.digit == t.nextDigit);
        }

        private static bool DigitsOnlyEverIncreaseOrStayTheSame(int p)
        {
            return p.ToString()
                .Zip(
                    p.ToString().Substring(1) + "9",
                    (digit, nextDigit) =>
                        (digit: int.Parse(digit.ToString()),
                        nextDigit: int.Parse(nextDigit.ToString())))
                .All(t => t.digit <= t.nextDigit);
        }
    }
}
