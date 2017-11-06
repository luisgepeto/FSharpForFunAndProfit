
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