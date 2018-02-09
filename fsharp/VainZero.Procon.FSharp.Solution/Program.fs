namespace global
  open System
  open System.Collections
  open System.Collections.Generic

  [<AutoOpen>]
  module Operators =
    let read f = Console.ReadLine().Split([|' '|]) |> Array.map f

  module Program =
    [<EntryPoint>]
    let main _ =
      0
