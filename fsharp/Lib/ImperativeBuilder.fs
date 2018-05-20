namespace global
  open System

  [<RequireQualifiedAccess>]
  type ImperativeState<'T> =
    | Step
    | Break
    | Continue
    | Return of 'T

  type ImperativeBuilder private () =
    member __.Delay(f: unit -> ImperativeState<'r>): unit -> ImperativeState<'r> =
      f

    member __.Run(f: unit -> ImperativeState<unit>): unit =
      f () |> ignore

    member __.Run(f: unit -> ImperativeState<'r>): 'r =
      match f () with
      | ImperativeState.Return x ->
        x
      | ImperativeState.Step
      | ImperativeState.Break
      | ImperativeState.Continue ->
        InvalidProgramException() |> raise

    member __.Zero(): ImperativeState<'r> =
      ImperativeState.Step

    member __.Return(r: 'r): ImperativeState<'r> =
      ImperativeState.Return r

    member __.ReturnFrom(m: ImperativeState<'r>): ImperativeState<'r> =
      m

    member __.Bind(m: ImperativeState<_>, f: unit -> ImperativeState<_>): ImperativeState<_> =
      match m with
      | ImperativeState.Step ->
        f ()
      | ImperativeState.Break
      | ImperativeState.Continue
      | ImperativeState.Return _ ->
        m

    member __.Combine(r: ImperativeState<'r>, f: unit -> ImperativeState<'r>): ImperativeState<'r> =
      match r with
      | ImperativeState.Step ->
        f ()
      | ImperativeState.Break
      | ImperativeState.Continue
      | ImperativeState.Return _ ->
        r

    member __.Using(x: 'r, f: 'r -> ImperativeState<'y>): ImperativeState<'y> =
      use _x = x
      f x

    member __.TryWith(f: unit -> 'r, h: exn -> 'r) =
      try
        f ()
      with
      | e -> h e

    member __.TryFinally(f: unit -> 'r, g: unit -> unit) =
      try
        f ()
      finally
        g ()

    member __.While(p: unit -> bool, f: unit -> ImperativeState<'r>): ImperativeState<'r> =
      let rec loop () =
        if p () then
          match f () with
          | ImperativeState.Step
          | ImperativeState.Continue ->
            loop ()
          | ImperativeState.Break ->
            ImperativeState.Step
          | ImperativeState.Return _ as r ->
            r
        else
          ImperativeState.Step
      loop ()

    member this.For(xs: #seq<'x>, body: 'x -> ImperativeState<'r>): ImperativeState<'r> =
      use e = xs.GetEnumerator()
      this.While(e.MoveNext, fun () -> body e.Current)

    static member val Instance = ImperativeBuilder()
