module VainZero.ImperativeBuilder.Tests

open Expecto

let imperative = ImperativeBuilder.Instance
let Break = ImperativeState.Break
let Continue = ImperativeState.Continue

[<Tests>]
let tests =
  testList "ImperativeBuilder" [
    testCase "Empty" <| fun () ->
      imperative { () } |> is ()

    testCase "Return" <| fun () ->
      imperative { return 1 } |> is 1

    testCase "Using" <| fun () ->
      let deferred = ref 0
      imperative {
        use _x = defer <| fun () ->
          deferred |> incr
        return ! deferred
      } |> is 0
      ! deferred |> is 1

    testCase "Combine" <| fun () ->
      let mutable c = 1
      imperative {
        c <- c + 2
        c <- c * 3
      } |> is ()
      c |> is 9

    testCase "While" <| fun () ->
      let mutable i = 0
      imperative {
        while i < 10 do
          i <- i + 1
        return i * 2
      } |> is 20
      i |> is 10

    testCase "For" <| fun () ->
      let mutable c = 0
      imperative {
        for i in 1..9 do
          c <- c + i
        return c * 2
      } |> is 90
      c |> is 45

    testCase "While Break" <| fun () ->
      let mutable i = 0
      imperative {
        while i < 1000 do
          i <- i + 1
          if i >= 10 then
            return! Break
      } |> is ()
      i |> is 10

    testCase "While Continue" <| fun () ->
      let mutable s = 1
      let mutable i = 1
      imperative {
        while i <= 8 do
          use _x = defer <| fun () ->
            i <- i + 1
          if i = 7 then
            return! Continue
          s <- s * i
      } |> is ()
      s |> is (40320 / 7)

    testCase "Break from nested loop" <| fun () ->
      let mutable k = 0
      imperative {
        for _ in 1..8 do
          for y in 1..8 do
            if y = 3 then
              return! Break
            k <- k + 1
        return -1
      } |> is -1
      k |> is 16

    testCase "Return from nested loop" <| fun () ->
      let mutable k = 0
      imperative {
        for x in 1..8 do
          for y in 1..8 do
            if (x, y) = (2, 5) then
              return k
            k <- k + 1
        return -1
      } |> is 12
      k |> is 12
  ]
