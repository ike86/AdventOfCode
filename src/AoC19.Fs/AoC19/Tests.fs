namespace AoC19.Tests

open Xunit
open FsUnit.Xunit
open AoC19
open Intcode

module ``Given an Intcode program code`` =
    [<Fact>]
    let ``parse yields the Intcode.Program`` () =
        parse "1,0,0,3,99"
        |> should equal [| 1; 0; 0; 3; 99 |]

module ``Intcode tests`` =

    [<Fact>]
    let ``run halts on 99`` () =
        (fun () -> (run (parse "99")) |> ignore)
        |> should throw typeof<ProgramHaltedException>

    [<Fact>]
    let ``Opcode 1 adds together numbers`` () =
        let output: Program = run (parse "1,5,6,7,99,4,5,0")
        Array.last output |> should equal (4 + 5)

    [<Fact>]
    let ``Opcode 2 multiplies two numbers`` () =
        let output: Program = run (parse "2,5,6,7,99,4,5,0")
        Array.last output |> should equal (4 * 5)

    [<Fact>]
    let ``run iterates over all instructions, until hits halt`` () =
        let program = parse "1,9,10,3,2,3,11,0,99,30,40,50"
        (fun () -> (run program) |> ignore)
        |> should throw typeof<ProgramHaltedException>