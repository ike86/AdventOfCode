namespace AoC22.Fs

module Day02 =

    type HandShape =
        | Rock
        | Paper
        | Scissor

    let parseLine (s: string) =
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

    let parse (s: string) : (HandShape * HandShape) [] =
        s.Split([| '\n' |]) |> Array.map parseLine

    module Example =
        let input =
            "A Y\n\
             B X\n\
             C Z"

    open Xunit
    open FsUnit.Xunit

    module ``Given a strategy guide`` =
        [<Fact>]
        let ``parse yields the list of hand shape pairs`` () =
            parse Example.input
            |> should
                equal
                [| (Rock, Paper)
                   (Paper, Rock)
                   (Scissor, Scissor) |]
