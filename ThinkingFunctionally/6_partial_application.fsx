open System.Net.Mime.MediaTypeNames
open System.Windows.Forms.VisualStyles.VisualStyleElement.Window
open System
open System.Globalization

//partial application essentially means fixing the first N parameters of a function
let add42 = (+) 42 //partially applied 42
add42 1
add42 2
[1;2;3] |> List.map add42

//we can creat a tester by partially applying a condition
let isBiggerThanTwo = (<) 2
isBiggerThanTwo 1
isBiggerThanTwo 3
[1;2;3] |> List.filter isBiggerThanTwo

let printer = printfn "printin param=%i"
[1;2;3] |> List.iter printer

//we can create more complex partially applied functions
let add1 = (+) 1
let add1ToEach = List.map add1
add1ToEach [1;2;3;4]

let filterEvens = List.filter (fun i -> i%2 = 0)
filterEvens [1;2;3;4]

//or even more complex (added logging)
let adderWithLogger logger x y =
    logger "x" x
    logger "y" y
    let result = x+y
    logger "x+y" result
    result

let consoleLogger  argName argValue = printfn "%s=%A" argName argValue
let popupLogger  argName argValue =
    let message = sprintf "%s=%A" argName argValue
    System.Windows.Forms.MessageBox.Show(text = message, caption="Logger") |> ignore

let addWithConsoleLogger = adderWithLogger consoleLogger
let addWithPopupLogger = adderWithLogger popupLogger
addWithConsoleLogger 1 2
addWithConsoleLogger 42 99
addWithPopupLogger 1 2
addWithPopupLogger 42 99
//these partially applied functions can create libraries which are flexible!

//in order to design function sfor partial applications the following advices hold true:
//1. put earlier parameters more likely to be static
//2. put last the data structure or collection (most varying argument)
//3. for well know operations such as subtract, put in expected order
//this results in easily composable and pipable functions!
let result = 
    [1..10]
    |> List.map (fun i-> i+1)
    |> List.filter (fun i -> i>5)
let composite = List.map (fun i-> i+1) >> List.filter (fun i -> i>5)
let result = composite [1..10]

// we can even wrap bcl function sfor partial application
//normally the most varied parameter goes last in F# while
//in other languages goes first. we can solve this as follows:
let replace oldStr newStr (s:string) =
    s.Replace(oldValue=oldStr, newValue=newStr)
let startsWith lookFor (s:string) =
    s.StartsWith(lookFor)

//we can partially apply now!
let result =
    "hello"
    |> replace "h" "j"
    |> startsWith "j"

["the";"quick";"brown";"fox"] |> List.filter (startsWith "f")
let compositeOperation = replace "h" "j" >> startsWith "j"
let result = compositeOperation "hello"

//we can now understand the pipe function! It allows us to put the argument in front of the function
let (|>) x f = f x
let doSomething x y z = x+y+z
let doSomethingPartial = doSomething 1 2 
//the following expressions are the same
doSomethingPartial 3
3 |> doSomethingPartial
//other applications 
"12" |> int //parsing
1 |> (+) 2 |> (*) 3 //chaining arithmetic


//the contrary operator <| is seldom used, though it might be useful
printf "%i" 1+2 //error
printf "%i" (1+2)
printf "%i" <| 1+2
let add x y = x+y
(1+2) add (3+4) // error
1+2 |> add <| 3+4