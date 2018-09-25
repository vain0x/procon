use std::io::BufRead;

#[allow(unused_macros)]
macro_rules! scan {
    (!! ($ws:expr);
        $([ $yv:ident : $yt:ty ]),*
        $(( $($xv:ident : $xt:ty),* )),*
    ) => {
        $(let $yv : Vec<$yt> =
            $ws.map(|w| w.parse().unwrap()).collect();)*
        $($(let $xv;)* {
            let mut ws = $ws;
            $($xv = ws.next().unwrap().parse::<$xt>().unwrap();)*
        })*
    };
    (!! ($ws:expr) { $zv:ident : $zn:expr };
        $([ $yv:ident : $yt:ty ]),*
        $(( $($xv:ident : $xt:ty),* )),*
    ) => {
        let mut $zv = vec![];
        for _ in 0..$zn {
            $(scan!(!! ($ws); [ $yv : $yt ]); $zv.push($yv);)*
            $(scan!(!! ($ws); ($($xv : $xt),*)); $zv.push(($($xv),*));)*
        }
    };
    (
        @input $r:expr;
        $(
            $([ $yv:ident : $yt:ty ]),*
            $(( $($xv:ident : $xt:ty),* )),*
            $({ $zv:ident : $zn:expr }),*
        ;)+
    ) => {
        let mut stdin = std::io::BufReader::new($r);
        let mut line = String::new();
        $(scan!{
            !! ({
                line.clear();
                stdin.read_line(&mut line).unwrap();
                line.split_whitespace()
            }) $({ $zv : $zn })*;
            $([ $yv : $yt ]),*
            $(( $($xv : $xt),* )),*
        })*
    };
}

fn main() {
    let source = r#"1
2 3
test
3 1 4 1 5 9
1 2
1 3
3 1 4
1 5 9
"#;

    // stdin.lock は1回だけ。
    // read_line(&mut line) のバッファ line は1個を使い回す。
    scan!{
        @input std::io::Cursor::new(source);
        (a: usize);                   // 1
        (b: usize, c: i64);           // 2 3
        (s: String);                  // test
        [x: i32];                     // 3 1 4 1 5 9
        (u: usize, v: usize) {es: b}; // 1 2
                                      // 1 3
        [x: i32] {board: b};          // 3 1 4
                                      // 1 5 9
    }

    println!("{} {}", a + b + c as usize, s);
    println!(
        "a={a} b={b} c={c} s={s} x={x:?} es={es:?} board={board:?}",
        a = a,
        b = b,
        c = c,
        s = s,
        x = x,
        es = es,
        board = board
    );
}
