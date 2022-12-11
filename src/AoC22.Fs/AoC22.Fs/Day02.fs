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
            | Rock -> 1
            | Paper -> 2
            | Scissor -> 3

    let round (opponent: HandShape, me: HandShape) : Result =
        let d = opponent.Value - me.Value
        match d with
        | 0 -> Draw
        | -1 -> IWin
        | -2 -> OpponentWins
        | 1 -> OpponentWins
        | 2 -> IWin
        | _ -> failwith "Should not happen"

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

    type ``round tests``() =
        static member TestRoundData =
            [| (Rock, Paper, IWin)
               (Paper, Rock, OpponentWins)
               (Paper, Scissor, IWin)
               (Scissor, Paper, OpponentWins)
               (Scissor, Rock, IWin)
               (Rock, Scissor, OpponentWins)
               (Rock, Rock, Draw)
               (Paper, Paper, Draw)
               (Scissor, Scissor, Draw) |]
            |> Array.toSeq
            |> Seq.map (fun t ->
                let opponent, me, result = t
                [| (opponent :> obj); (me :> obj); (result :> obj) |])

        [<Theory>]
        [<MemberData(nameof ``round tests``.TestRoundData)>]
        static member ``test round``(opponent, me, result) =
            round (opponent, me) |> should equal result
