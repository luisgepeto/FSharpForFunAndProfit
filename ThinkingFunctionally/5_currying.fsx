
//if a mathematical function only has one paramter, how can an F# function have more than one?
//function with many parameters are rewritten as a series of new functions
let printTwoParameters x y = printfn "x=%i y=%i" x y

//internally is transformed into
let printTwoParameters' x =
    let subFunction y = printfn "x=%i y=%i" x y
    subFunction

//which is essentially a function that returns a function evaluated with one parameter that accepts another parameter
printTwoParameters 1
//evaluating the function with only one parameter returns a function!
//val it: (int -> unit)

let x = 6
let y = 99
let intermediateFn = printTwoParameters x
let result = intermediateFn 6

let addTwoParameters x y = x+y
let addTwoParameters' x =
    let subFunction y = x + y
    subFunction
    
let intermediateAdd = addTwoParameters x
let resultAdd = intermediateAdd y

//but then what happens with binary operations such as +, -, * etc
//they are actually also functions defined as:
let intermediatePlus = (+) x
let resultPlus = intermediatePlus y

//this works for all functions such as printfn
//if the function has more than two parameters, the same happens for each parameter
let multiParamFn (p1:int) (p2:bool) (p3:string) (p4:float) = ()
let intermediateFn1 = multiParamFn 42
let intermediateFn2 = intermediateFn1 false
let intermediateFn3 = intermediateFn2 "hello"
let finalResult = intermediateFn3 3.14
//val multiParamFn int -> bool -> string -> float
//val intermediateFn1 (bool -> string -> float -> unit)
//val intermediateFn2 : (string -> float -> unit)
//val intermediateFn3 : (float -> unit)
//val finalResult : unit = ()