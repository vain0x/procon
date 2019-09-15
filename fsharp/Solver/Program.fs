module Program

open System
open System.Collections
open System.Collections.Generic

let read f = Console.ReadLine().Split([|' '|]) |> Array.map f

[<EntryPoint>]
let main _ =
  0
