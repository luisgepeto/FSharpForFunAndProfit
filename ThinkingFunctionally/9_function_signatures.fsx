//f sharp has two syntaxes, one for expressions and for type definitions
[1;2;3] //expression
int list //type definition

Some 1 // expression
int option //type definition

(1,"a") //expression
int * string //type expression

// expression syntax // type syntax
let add1 x = x + 1            // int -> int 
let add x y = x + y           // int -> int -> int
let print x = printf "%A" x   // 'a -> unit
System.Console.ReadLine       // unit -> string
List.sum                      // 'a list -> 'a
List.filter                   // ('a -> bool) -> 'a list -> 'a list
List.map                      // ('a -> 'b) -> 'a list -> 'b list

//knowing function signatures by type can help us identify how a function works
//we can define our own types
type Adder = int -> int
type AdderGenerator = int -> Adder
//we can use these type declarations as function definitions
let a:AdderGenerator =  fun x -> (fun y -> x + y)
let b:AdderGenerator = fun (x:float) -> (fun y -> x + y) // this fails
let c = fun (x:float) -> (fun y -> x + y) // this is no problem (function type is not constrained)

type testA = int -> int
type testB = int -> int -> int
type testC = int -> (int -> int)      
type testD = (int -> int) -> int
type testE = int -> int -> int -> int
type testF = (int -> int) -> (int -> int)
type testG = int -> (int -> int) -> int
type testH = (int -> int -> int) -> int
let testAFunction:testA = fun x -> x*2
let testBFunction:testB = fun x y -> x + y
let testCFunction:testC = fun x -> (fun y -> x + y)
let testDFunction:testD = fun f -> f 1
let testEFunction:testE = fun x y z -> x*y+z
let testFFunction:testF = fun f -> f
let testGFunction:testG = fun x f -> f x
let testHFunction:testH = fun f -> f 1 2