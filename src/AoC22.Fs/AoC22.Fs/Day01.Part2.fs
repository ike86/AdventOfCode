namespace AoC22.Fs.Day01

module Part2 =
    let topThreeMostCalories (maybeCalories: int option []) : int =
        42

    // In the example above, the top three Elves are the fourth Elf
    // (with 24000 Calories), then the third Elf (with 11000 Calories),
    // then the fifth Elf (with 10000 Calories).
    // The sum of the Calories carried by these three elves is 45000.

    open Xunit
    open FsUnit.Xunit

    module ``Given a calories inventory list`` =
        [<Fact>]
        let ``topThreeMostCalories carried by an elf for Example`` () =
            parse Example.input
            |> topThreeMostCalories
            |> should equal 45000