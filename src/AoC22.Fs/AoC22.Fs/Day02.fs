namespace AoC22.Fs

module Day02 =

    type HandShape =
        | Rock
        | Paper
        | Scissor

    let parse (s: string) =
        let tokens = s.Split ' '

        let opponent =
            match tokens[0] with
            | "A" -> Rock
            | "B" -> Paper
            | "C" -> Scissor

        let me =
            match tokens[1] with
            | "X" -> Rock
            | "Y" -> Paper
            | "Z" -> Scissor

        (opponent, me)

    module Example =
        let input =
            "A Y\n\
             B X\n\
             C Z"

    open Xunit
    open FsUnit.Xunit

    module ``Given a strategy guide`` =
        [<Fact(Skip="WIP")>]
        let ``parse yields the list of hand shape pairs`` () =
            parse Example.input
            |> should
                equal
                [| (Rock, Paper)
                   (Paper, Rock)
                   (Scissor, Scissor) |]
        let WIPf ((input, opponent, me): string * 'a * 'b): unit =
            parse input |> should equal (opponent, me)

        [<Fact>]
        let WIP () =
            WIPf ("A Y", Rock, Paper)

        [<Fact>]
        let WIP2 () =
            WIPf ("B X", Paper, Rock)

        [<Fact>]
        let WIP3 () =
            WIPf ("C Z", Scissor, Scissor)