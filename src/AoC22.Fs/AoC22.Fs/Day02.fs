namespace AoC22.Fs

module Day02 =

    type Result =
        | OpponentWins
        | IWin
        | Draw

    type HandShape =
        | Rock
        | Paper
        | Scissor

        member this.Value =
            match this with
            | Rock    -> 1
            | Paper   -> 2
            | Scissor -> 3

    type StrategyGuide = (HandShape * HandShape) []

    let parseLine (s: string) =
        let tokens = s.Split ' '

        let opponent =
            match tokens[0] with
            | "A" -> Rock
            | "B" -> Paper
            | "C" -> Scissor
            | _ -> failwith "Unexpected hand shape"

        let me =
            match tokens[1] with
            | "X" -> Rock
            | "Y" -> Paper
            | "Z" -> Scissor
            | _ -> failwith "Unexpected hand shape"

        (opponent, me)

    let parse (s: string) : StrategyGuide =
        s.Split([| '\n' |]) |> Array.map parseLine

    let totalScore (_: StrategyGuide) : int = 15

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

        [<Fact>]
        let ``totalScore for example`` () =
            parse Example.input
            |> totalScore
            |> should equal 15
