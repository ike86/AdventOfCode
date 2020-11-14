namespace AoC19.Tests

    open Xunit
    open FsUnit.Xunit
    open AoC19
    open Intcode

    module ``Intcode tests`` =
        module ``Given an Intcode program`` =
            [<Fact>]
            let ``parse yields the list of integers`` ()=
                parse "1,0,0,3,99" |> should matchList [1;0;0;3;99]