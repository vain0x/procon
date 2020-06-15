namespace Procon

open System
open Xunit

[<AutoOpen>]
module Helpers =
  let is<'T> (expected: 'T) (actual: 'T) =
    Assert.Equal(expected, actual)

  let (-->) x y = (x, y)

  let defer (dispose: unit -> unit) =
    { new IDisposable with
        member __.Dispose() =
          dispose ()
    }
