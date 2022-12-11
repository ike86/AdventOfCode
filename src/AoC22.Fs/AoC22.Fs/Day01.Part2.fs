namespace AoC22.Fs.Day01

open AoC22.Fs

module Part2 =
    let topThreeMostCalories (maybeCalories: int option []) : int =
        sumCaloriesPerElf maybeCalories
        |> Array.sortDescending
        |> Array.take 3
        |> Array.sum

    // In the example above, the top three Elves are the fourth Elf
    // (with 24000 Calories), then the third Elf (with 11000 Calories),
    // then the fifth Elf (with 10000 Calories).
    // The sum of the Calories carried by these three elves is 45000.

    open Xunit
    open FsUnit.Xunit

    module ``Given a calories inventory list`` =
        [<Fact>]
        let ``topThreeMostCalories carried by elves for Example`` () =
            parse Example.input
            |> topThreeMostCalories
            |> should equal 45000

        [<Fact>]
        let ``topThreeMostCalories carried by elves`` () =
            parse Day01.Puzzle.input
            |> topThreeMostCalories
            |> should equal 202346
