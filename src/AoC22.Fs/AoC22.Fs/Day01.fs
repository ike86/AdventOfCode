namespace AoC22.Fs

open System

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Day01 =

    let maybeParse (s: string) : int option =
        match Int32.TryParse s with
        | true, number -> Some number
        | _ -> None

    let parse (s: string) : int option [] =
        s.Split([| '\n' |])
        |> Array.map maybeParse
        |> Seq.toArray

    let filterSome<'T> (a: 'T option []) =
        a
        |> Array.map Option.toArray
        |> Array.map Array.toSeq
        |> Array.toSeq
        |> Seq.reduce Seq.append
        |> Seq.toArray

    let mostCalories (maybeCalories: int option []) : int =
        // None
        // 1, 2, 3, N, 4, N, 5, 6
        let noneIndices =
            Array.indexed maybeCalories
            |> Array.choose (fun t ->
                match t with
                | _, Some _ -> None
                | i, None -> Some i)
        //       3,       5
        let extended =
            Array.concat [ [| 0 |]
                           noneIndices
                           [| maybeCalories.Length - 1 |] ]
        // 0,    3,       5, 7

        let caloriesPerElf =
            extended
            |> Array.windowed 2
            // 0,3   3,5      5,7
            |> Array.map (fun indices -> maybeCalories[indices[0] .. indices[1]])
            |> Array.map filterSome

        let sumCaloriesPerElf =
            caloriesPerElf |> Array.map Array.sum

        Array.max sumCaloriesPerElf

    // One important consideration is food - in particular,
    // the number of Calories each Elf is carrying (your puzzle input).
    //
    // The Elves take turns writing down the number of Calories contained
    // by the various meals, snacks, rations, etc. that they've brought with them,
    // one item per line. Each Elf separates their own inventory from
    // the previous Elf's inventory (if any) by a blank line.
    //
    // For example, suppose the Elves finish writing their items' Calories and
    // end up with the following list:

    module Example =
        let input =
            "1000
            2000
            3000

            4000

            5000
            6000

            7000
            8000
            9000

            10000"
    //
    // This list represents the Calories of the food carried by five Elves:
    //
    //     The first Elf is carrying food with 1000, 2000, and 3000 Calories, a total of 6000 Calories.
    //     The second Elf is carrying one food item with 4000 Calories.
    //     The third Elf is carrying food with 5000 and 6000 Calories, a total of 11000 Calories.
    //     The fourth Elf is carrying food with 7000, 8000, and 9000 Calories, a total of 24000 Calories.
    //     The fifth Elf is carrying one food item with 10000 Calories.
    //
    // In case the Elves get hungry and need extra snacks,
    // they need to know which Elf to ask: they'd like to know how many Calories
    // are being carried by the Elf carrying the most Calories.
    // In the example above, this is 24000 (carried by the fourth Elf).
    //
    // Find the Elf carrying the most Calories. How many total Calories is that Elf carrying?

    open Xunit
    open FsUnit.Xunit

    module ``Given a calories inventory list`` =
        [<Fact>]
        let ``parse yields the list of calories`` () =
            parse Example.input
            |> should
                equal
                [| Some 1000
                   Some 2000
                   Some 3000
                   None
                   Some 4000
                   None
                   Some 5000
                   Some 6000
                   None
                   Some 7000
                   Some 8000
                   Some 9000
                   None
                   Some 10000 |]

        [<Fact>]
        let ``mostCalories carried by an elf for Example`` () =
            parse Example.input
            |> mostCalories
            |> should equal 24000

        [<Fact>]
        let ``mostCalories carried by an elf`` () =
            parse Day01.Puzzle.input
            |> mostCalories
            |> should equal 69501
