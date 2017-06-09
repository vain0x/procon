namespace VainZero.Procon.FSharp

open System
open System.IO
open System.Text
open Persimmon

module ``test Program`` =
  let testScript =
    """

"""

  let ``test main`` =
    [|
      let cases = ScriptParser.parse testScript
      for (i, (input, expected)) in cases |> Seq.mapi (fun i x -> (i, x)) ->
        test (sprintf "Case #%d" (i + 1)) {
          use inputReader = new StringReader(input)
          use outputWriter = new StringWriter()
          Console.SetIn(inputReader)
          Console.SetOut(outputWriter)
          Program.main [||] |> ignore
          do! outputWriter.ToString() |> assertEquals expected
          return ()
        }
    |]
