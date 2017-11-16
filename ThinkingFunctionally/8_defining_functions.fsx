open System.Security.Cryptography.X509Certificates

//anonymous functions (lambdas)
//must have fun keyword
//uses single arrow -> not double =>
fun parameter1 parameter2 etc -> expression //definition
let add = fun x y -> x+y
List.map (fun i -> i+1) [1..10]
//used to clearly return a function
let adderGenerator x = (+) x
let adderGenerator' x = fun y -> x + y
let adderGenerator'' = fun x -> (fun y -> x+y)

//pattern matching on parameters on functions
//when defining a function we can pass explicit patterns 
type Name = {first:string; last:string}
let bob = {first="bob"; last="smith"}
let f1 name = 
    let {first=f; last=l} = name
    printfn "first=%s; last=%s" f l
let f2 {first=f; last=l} = printfn "first=%s; last=%s" f l
//this matching is only possible when all possibilities are covered
//therefore we cannot match on union types or lists
//we get an incomplete pattern matches compiler expression!
let f3 (x::xs) = 
    printfn "first element is %A" x


//tuples vs multiple parameters are easy to mistake
let addTwoParams x y = x+y
let addTuple aTuple =
    let (x,y) = aTuple
    x+y
let addTuple' (x,y) = x+y //this is a single argument!
addTwoParams 1 2
addTuple (1,2)
let y = 1,2
addTuple' y  //the comma is what you actually need not parenthesis

//we could turn multiparameter functions into tuple receving functions
//however that would limit the ability to partially apply and curry functions
//therefore we only use tuples when:
//1. tuples are meaningful in themselves (e.g. dimensional coordinates)
//2. bundled data (.NET library functions)
System.String.Compare ("a","b") // is receiving a tuple!
//however they are special cases
let tuple = ("a","b")
System.String.Compare tuple //wont work!
//however we could still pass a tuple by wrapping as seen before


//guidelines for separated vs grouped parameters
//due to partial application only some parameters might be grouped
//1. it is generally better to separate parameters rather than using a tuple
let add x y = x+y
let locateOnMap (xcoord, ycoord) = ignore //do something
type CustomerName = {first:string; last:string}
let setCustomerName aCustomerName  = ignore //good
let setCustomerName first last = ignore //not recommended
let setCustomerName myCredentials aName = ignore //credentials and name are two different things


//parameterless functions
//a function always needs a parameter
let sayHello = printfn "hi" //will not work!
//the fix is to add a unit parameter
let sayHello' () = printfn "hi"
sayHello'()
let sayHello'' = fun () -> printfn "hi" // with lambdas!
sayHello''()


//defining new operators
//this can be done by using parenthesis
//when using * a space must be used to not create a comment
let ( *+* ) x y = x + y + 1
//can be used normally
let result = ( *+* ) 2 3
//or infix if it has two parameters
let result' = 2 *+* 3


//if we leave the last parameter off it is called point free style!
// this makes function composition easier
let add x y = x+y // explicit style
let add x = (+) x // point free
let add1Times2 x = (x+1)*2 //explicit
let add1Times2 = (+) 1 >> (*) 2 // point free
let sum list = List.reduce (fun sum e -> sum+e) list // explicit
let sum = List.reduce (+) //point free
//ithelps clarify the underlying algorithm
//BUT it might lead for confusing code


//combinators
//a combinator is a function whose result depends only on parameters
//no dependency with the outside world
let (|>) x f = f x             // forward pipe
let (<|) f x = f x             // reverse pipe
let (>>) f g x = g (f x)       // forward composition
let (<<) g f x = g (f x)       // reverse composition
//combinators are the basis of a whole branch of logic
let I x = x                // identity function, or the Idiot bird
let K x y = x              // the Kestrel
let M x = x >> x           // the Mockingbird
let T x y = y x            // the Thrush (this looks familiar!)
let Q x y z = y (x z)      // the Queer bird (also familiar!)
let S x y z = x z (y z)    // The Starling
// and the infamous...
let rec Y f x = f (Y f) x  // Y-combinator, or Sage bird

//combinator libraries
//combinators allow us to declutter our code
//focusing only on functionality
// examples of these are the List.map and List.reduce
//which are essentially combinators


//recursive functions
let rec fib i = //we need to explicitly declare as recursive
    match i with
    | 1 -> 1
    | 2 -> 1
    | n -> fib (n-1) + fib (n-2)

let sumFibUntil x = List.map fib >> List.reduce (+) <| [1..x]
