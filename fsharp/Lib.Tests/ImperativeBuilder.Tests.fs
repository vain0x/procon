module ImperativeBuilderTests

open ImperativeBuilders
open Procon
open Xunit

let imperative = ImperativeBuilder.Instance
let Break = ImperativeState.Break
let Continue = ImperativeState.Continue

[<Fact>]
let testEmpty () =
  imperative { () } |> is ()

[<Fact>]
let testReturn () =
  imperative { return 1 } |> is 1

[<Fact>]
let testUsing () =
  let deferred = ref 0
  imperative {
    use _x = defer <| fun () ->
      deferred |> incr
    return ! deferred
  } |> is 0
  ! deferred |> is 1

[<Fact>]
let testCombine () =
  let mutable c = 1
  imperative {
    c <- c + 2
    c <- c * 3
  } |> is ()
  c |> is 9

[<Fact>]
let testWhile () =
  let mutable i = 0
  imperative {
    while i < 10 do
      i <- i + 1
    return i * 2
  } |> is 20
  i |> is 10

[<Fact>]
let testFor () =
  let mutable c = 0
  imperative {
    for i in 1..9 do
      c <- c + i
    return c * 2
  } |> is 90
  c |> is 45

[<Fact>]
let testWhileBreak () =
  let mutable i = 0
  imperative {
    while i < 1000 do
      i <- i + 1
      if i >= 10 then
        return! Break
  } |> is ()
  i |> is 10

[<Fact>]
let testWhileContinue () =
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

[<Fact>]
let testBreakFromNestedLoop () =
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

[<Fact>]
let testReturnFromNestedLoop () =
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
