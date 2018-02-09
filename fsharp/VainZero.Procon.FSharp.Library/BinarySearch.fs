namespace global
  open System.Collections.Generic

  module BinarySearch =
    let lowerBound (x: 'x) (xs: #IReadOnlyList<'x>) =
      let comparer = Comparer.Default
      let mutable l = -1
      let mutable r = xs.Count
      while r - l >= 2 do
        let m = l + (r - l) / 2
        if comparer.Compare(xs.[m], x) < 0 then
          l <- m
        else
          r <- m
      r
