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

        let me =
            match tokens[1] with
            | "Y" -> Paper

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

        [<Fact>]
        let WIP () =
            parse "A Y" |> should equal (Rock, Paper)
