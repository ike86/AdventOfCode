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
    let ``Opcode 1 adds together numbers`` () =
        let output: Program = run (parse "1,5,6,7,99,4,5,0")
        Array.last output |> should equal (4 + 5)

    [<Fact>]
    let ``Opcode 2 multiplies two numbers`` () =
        let output: Program = run (parse "2,5,6,7,99,4,5,0")
        Array.last output |> should equal (4 * 5)

    [<Fact>]
    let ``run iterates over all instructions, until hits halt`` () =
        let output: Program = run (parse "1,9,10,3,2,3,11,0,99,30,40,50")
        output |> should equal (parse "3500,9,10,70,2,3,11,0,99,30,40,50")

    [<Fact>]
    let ``run gravity assist program`` () =
        let ``gravity assist program`` = "1,12,2,3,1,1,2,3,1,3,4,3,1,5,0,3,2,13,1,19,1,5,19,23,2,10,23,27,1,27,5,31,2,9,31,35,1,35,5,39,2,6,39,43,1,43,5,47,2,47,10,51,2,51,6,55,1,5,55,59,2,10,59,63,1,63,6,67,2,67,6,71,1,71,5,75,1,13,75,79,1,6,79,83,2,83,13,87,1,87,6,91,1,10,91,95,1,95,9,99,2,99,13,103,1,103,6,107,2,107,6,111,1,111,2,115,1,115,13,0,99,2,0,14,0"
        let output: Program = run (parse ``gravity assist program``)
        Array.head output |> should equal 4714701