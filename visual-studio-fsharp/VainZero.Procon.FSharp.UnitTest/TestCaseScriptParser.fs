namespace VainZero.Procon.FSharp

module ScriptParser =
  open System
  open System.Collections.Generic
  open System.IO
  open System.Text

  module String =
    let normalizeLinebreaks (str: string) =
      str.Replace("\r\n", "\n").Replace("\r", "\n")

    let splitByLinebreaks (str: string) =
      (str |> normalizeLinebreaks).Split([|'\n'|])

  module Seq =
    let splitBy (delimiter: 'x) (xs: seq<'x>) =
      seq {
        let comparer = EqualityComparer<'x>.Default
        let mutable chunk = ResizeArray()
        for x in xs do
          if comparer.Equals(x, delimiter) then
            yield chunk :> IReadOnlyList<_>
            chunk <- ResizeArray()
          else
            chunk.Add(x)
        yield chunk :> IReadOnlyList<_>
      }

    let chunkBySize chunkSize (xs: seq<_>) =
      seq {
        if chunkSize <= 0 then invalidArg "chunkSize" "chunkSize must be positive."
        let mutable chunkOrNone = None
        for x in xs do
          let chunk =
            match chunkOrNone with
            | None ->
              let chunk = ResizeArray(chunkSize)
              chunkOrNone <- Some chunk
              chunk
            | Some chunk ->
              chunk
          chunk.Add(x)
          if chunk.Count = chunkSize then
            yield chunk :> IReadOnlyList<_>
            chunkOrNone <- None
        match chunkOrNone with
        | None -> ()
        | Some chunk ->
          yield chunk :> IReadOnlyList<_>
      }

  let concat lines =
    let builder = StringBuilder()
    for line in lines do
      builder.AppendLine(line) |> ignore
    builder.ToString()

  let parse (script: string) =
    // Split by empty lines, regarding multiple empty lines as single empty line.
    let groupedLines =
      script
      |> String.splitByLinebreaks
      |> Seq.map (fun str -> str.Trim())
      |> Seq.splitBy ""
      |> Seq.filter (fun list -> list.Count <> 0)
    // Convert adjoint two chunks to an input/ouput pairs.
    let testCases =
      groupedLines
      |> Seq.chunkBySize 2
      |> Seq.map
        (fun chunk ->
          match chunk |> Seq.toArray with
          | [|input; output|] -> (input |> concat, output |> concat)
          | _ -> failwith "Missing output for the last input."
        )
    testCases

  module UnitTest =
    open Persimmon

    let test = Persimmon.Syntax.UseTestNameByReflection.test

    let ``test parse`` =
      test {
        let script =
          """i00 a
i01 b

o00 a
o01 b


i10


o10"""
        do!
          parse script |> Seq.toArray
          |> assertEquals
            [|
              ("i00 a\r\ni01 b\r\n", "o00 a\r\no01 b\r\n")
              ("i10\r\n", "o10\r\n")
            |]
      }
