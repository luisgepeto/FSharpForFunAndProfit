
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

//top level modules
//a top level module are defined slightly different than the modules
//module declaration is on top of the file and there is no = sign
//the module contents are not indented

//shadowing
//if a function has the same name it will mask any function declared previously
//to solve this we could use the RequireQualifiedAccess modifier
//this will not allow the module to be opened
[<RequireQualifiedAccess>]
module BustedStuff =
    let add x y = x + y

    [<RequireQualifiedAccess>]
    module BustedLib =
        let add x y :float = x + y

open BustedStuff //cannot be opened
open BustedStuff.BustedLib //cannot be opened
let result = BustedStuff.add  1 2
let result' = BustedStuff.BustedLib.add  1.0 2.0

//namespaces can be used just as in C#
namespace Utilities
module CrowdedStreets =
    let add x y = x + y
//however indentation rules apply
//in order to not use indentation a namespace can be also written implicitly as
module Utilities.CrowdedStreets
//we can even define multiple namespaces on the same file


//mixing types and functions in modules
//data structure and functions are combined in a module not a class
//a common pattern for mixing types and functions is
//1. having the type declared separately from the functions
//2. having the type declared in the same module as the functions

//FRIST APPROACH
namespace Example

type PersonType = {First:string; Last:string}

module Person =

    let create first last = {First = first; Last = last}
    let fullName {First=first; Last=last} = first + " " + last

//test
let person = Person.create "john" "doe"
Person.fullName person |> printfn "fullname is %s"


//SECOND APPROACH
module Customer =
    type T = {AccountId:int; Name:string}
    let create id name = {T.AccountId = id; T.Name = name}
    let isValid {T.AccountId=id;} = id > 0

//test
let customer = Customer.create 42 "bob"
Customer.isValid customer |> printfn "is valid %b"

//notice that we almost always have a factory method to create the type
//this means that we wont need to explicitly name the type in the client code
//FIRST APPROACH is more .NET like and for sharing libraries (exported class names are as expected)
//SECOND is more common for functional languages


//if you have a set of types that need to be declared withoout function s
//we can do so in a namespace only
namespace Example

type PersonType = {First:string; Last:string}