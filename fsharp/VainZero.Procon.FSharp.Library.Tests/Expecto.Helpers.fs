namespace Expecto

open System

[<AutoOpen>]
module Helpers =
  let is expected actual =
    Expect.equal actual expected "These should equal"

  let defer (dispose: unit -> unit) =
    { new IDisposable with
        member __.Dispose() =
          dispose ()
    }
