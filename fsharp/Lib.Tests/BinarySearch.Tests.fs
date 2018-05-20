module Procon.BinarySearchTests

open Xunit

[<Fact>]
let testLowerBound () =
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

  for (value, index) in cases do
    array |> BinarySearch.lowerBound value |> is index
