namespace AoC19.Tests

    open Xunit
    open FsUnit.Xunit
    open AoC19
    open Intcode

    module ``Intcode tests`` =
        module ``Given an Intcode program code`` =
            [<Fact>]
            let ``parse yields the Intcode.Program`` ()=
                parse "1,0,0,3,99" |> should equal [|1;0;0;3;99|]

        [<Fact>]
        let ``run halts on 99`` ()=
            (fun () -> (run (parse "99")) |> ignore)
            |> should throw typeof<ProgramHaltedException>

        [<Fact>]
        let ``Opcode 1 adds together numbers`` ()=
            let output: Program = run (parse "1,5,6,7,99,4,5,0")
            Array.last output |> should equal (4 + 5)