
//functions can be organized
//1. nested functions
//2. modules in an application
//3. use object oriented approach and attach functions to types as methods

//NESTED FUNCTIONS
//helps us encapsulate helper functions
let addThreeNumbers x y z =
    let add n = 
        fun x -> x + n
    x |> add y |> add z

addThreeNumbers 2 3 4

 //a nested function can access parent parameters
let validateSize max n =
    let printError () = printfn "Oops: '%i' is bigger than max: '%i'" n max
    if n > max then printError ()
validateSize 10 9
validateSize 10 11

//a common pattern is that the main function defines a nested recursive helper

let sumbNumbersUpTo max = 
    let rec recursiveSum n sumSoFar = 
        match n with 
        | 0 -> sumSoFar
        | _ -> recursiveSum (n-1) (n+sumSoFar)
    recursiveSum max 0
sumbNumbersUpTo 10

//however we must avoid ddeply nested functions
//especially if nested functions access the parent scope
//what does this do?!
let f x =
    let f2 y = 
        let f3 z =
            x * z
        let f4 z = 
            let f5 z = 
                y * z
            let f6 () =
                y * x
            f6 ()
        f4 y
    x * f2 x

//MODULES
//a module is just a group of functions
//starts with module definition and then a list of the contents
//modules work like static classes
module MathStuff =
    let add' x y = x + y
    let subtract x y = x - y

//to access across module boundaries we do:
module OtherStuff = 
    MathStuff.add' 4 5 
MathStuff.add' 4 5 

//and we can also do the open directory
open MathStuff
add' 3 5

//modules can also contain child modules
module MathStuff' = 
    module FloatLib =
        let add x y :float = x + y
        let subtract x y :float = x - y

module OtherStuff' =    
    open MathStuff'
    let add1 x = add' x 1
    let add1Float x = MathStuff'.FloatLib.add x 1.0
    let sub1Float x = FloatLib.subtract x 1.0