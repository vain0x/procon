module VainZero.BinarySearch.Tests

open Expecto

let (-->) x y = (x, y)

[<Tests>]
let tests =
  testList "BinarySearch" [
    let array = [|1; 1; 2; 3; 3; 3; 5; 8|]
    let cases =
      [
        0 --> 0
        1 --> 0
        2 --> 2
        3 --> 3
        4 --> 6
        5 --> 6
        8 --> 7
        9 --> 8
      ]

    yield testList "lowerBound" [
      for (x, index) in cases ->
        testCase (string (x, index)) <| fun () ->
          let actual = array |> BinarySearch.lowerBound x
          Expect.equal actual index "should equal"
    ]
  ]
