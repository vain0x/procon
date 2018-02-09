module VainZero.Sample.Tests

open Expecto

[<Tests>]
let tests =
  testList "samples" [
    testCase "arithmetic" <| fun () ->
      let actual = 1 + 2 * 3
      let expected = 7
      Expect.equal actual expected "These should equal"

    testCase "catch" <| fun () ->
      Expect.throwsC
        (fun () -> raise (exn ("Expected")))
        (fun ex ->
          Expect.stringContains ex.Message "Expected" "as expected"
        )
  ]
