//function application is left associative
//that means that the following two expressions are the same:
let F x y z = x y z
let F' x y z = (x y) z
let G w x y z = w x y z
let G' w x y z = ((w x) y) z

//if we want to break this pattern of association we can wither
//1. use explicit parenthesis (boring)
//2. use pipes!
let H x y z = x y z
let H' x y z = y z |> x //using right pipes
let H'' x y z = x <| y z //using left pipe!

//what is function composition?
//this is essentially passing the output of a function 
//to another function
let f (x:int) = float x * 3.0
let g (x:float) = x > 4.0
let h x = 
    let y = f x
    g y
let h' x = g <| f x
// we can mix these two functions using a third one
//this is the definition of the >> symbol
let compose f g x = g (f x)

let add1 x = x+1
let times2 x = x*2
let add1times2 = add1 >> times2 
let result = add1times2 3

//this operator has lower precedence than function application
//like all infix operators (+,-,*,>,<, etc..)
//this means that functions with multiple parameters can be composed
//without having to use paramethers
let add n x = x + n
let times n x = x*n
let add1Times2 = add 1 >> times 2
let add5Times3 = add 5 >> times 3

//composition can also be performed in the opposite way
//giving us a more english like syntax
let myList = []
myList |> List.isEmpty |> not // straight pipeline
myList |> (not << List.isEmpty)
List.isEmpty >> not <| myList

//so, after all, what is the difference between composition
//and pipeline operation
//All that pipe does is allow to change the order of parameters
//if the function has mulitple parameters, the last one will be the input
let doSomething x y z = x +y+z
doSomething 1 2 3
3 |> doSomething 1 2    
// however, composition ONLY acts upon functions
3 >> doSomething 1 2 //this is not allowed!
let add1Times2' = add1 |> times2 //this is not allwed, times2's last parameter isnt a function!

