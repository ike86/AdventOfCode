﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AoC19
{
    public class Day04
    {
        /*
        --- Day 4: Secure Container ---
        You arrive at the Venus fuel depot only to discover it's protected by a password.
        The Elves had written the password on a sticky note, but someone threw it out.

        However, they do remember a few key facts about the password:

        It is a six-digit number.
        The value is within the range 172930-683082.
        Two adjacent digits are the same (like 22 in 122345).
        Going from left to right, the digits never decrease;
        they only ever increase or stay the same (like 111123 or 135679).
        Other than the range rule, the following are true:

        111111 meets these criteria (double 11, never decreases).
        223450 does not meet these criteria (decreasing pair of digits 50).
        123789 does not meet these criteria (no double).
        How many different passwords within the range given in your puzzle input meet these criteria?
         */

        [Fact]
        void Is_a_six_digit_number()
        {
            IEnumerable<int> passwords = PossiblePasswords();

            passwords.Should().Contain(100000);
        }

        private IEnumerable<int> PossiblePasswords()
        {
            yield return 100000;
        }
    }
}
