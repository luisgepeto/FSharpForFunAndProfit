open System.Data.SqlClient

//f# supports .net classes, interfaces and structures
//F# can call into existing .net code
//expose any .net api to other languages (C#, VB, COM)


//try parse and try get value can be used in F# using tuples 
//this feature has been introduced in C# 7
let (i1success, i1) = System.Int32.TryParse("123")
if i1success then printfn "parsed as %i" i1 else printfn "parse failed"

let (i2success, i2) = System.Int32.TryParse("hello")
if i2success then printfn "parsed as %i" i2 else printfn "parse failed"

let dict = new System.Collections.Generic.Dictionary<string, string>();
dict.Add("a", "hello")
let (e1success, e1) = dict.TryGetValue("a");
let (e2success, e2) = dict.TryGetValue("b");

//f# can used named arguments when calling methods in .net libarys
let createReader fileName = new System.IO.StreamReader(path=fileName)

//we can use active patterns for system.char operations
//first we define the active pattern for the .net library
let (|Digit|Letter|Whitespace|Other|) ch = 
    if System.Char.IsDigit(ch) then Digit
    else if System.Char.IsLetter(ch) then Letter
    else if System.Char.IsWhiteSpace(ch) then Whitespace
    else Other

//then we use it with code
let printChar ch =
    match ch with
    | Digit -> printfn "%c is a digit" ch
    | Letter -> printfn "%c is a letter" ch
    | Whitespace -> printfn "%c is a whitespace" ch
    | _ -> printfn "%c is something else" ch

['a';'b';'1';' ';'-';'c'] |> List.iter printChar

//we can use active patterns in exceptions
open System.Data.SqlClient
let (|ConstraintException|ForeignKeyException|Other|) (ex:SqlException) = 
   if ex.Number = 2601 then ConstraintException 
   else if ex.Number = 2627 then ConstraintException 
   else if ex.Number = 547 then ForeignKeyException 
   else Other 

let executeNonQuery (sqlCommand:SqlCommand) =
    try
        let result = sqlCommand.ExecuteNonQuery()
        result
    with 
    | :? SqlException as sqlException -> 
        match sqlException with
        | ConstraintException -> 1
        | ForeignKeyException -> 2
        | _ -> reraise() 

//we can also create objects from an interface without having a concrete class!
let makeResource name =
    { new System.IDisposable with member this.Dispose() = printfn "%s disposed" name}

let useAndDisposeResources = 
    use r1 = makeResource "first resource"
    printfn "using first resource"
    for i in [1..3] do
        let resourceName = sprintf "\tinner resource %d" i
        use temp = makeResource resourceName
        printfn "\tdosomething with %s" resourceName
    use r2 = makeResource "second resource"
    printfn "using second resource"
    printfn "done"

//we can create instances of aan interface on the fly
type IAnimal = abstract member MakeNoise : unit -> string

let showThenoiseAnimalMakes (animal:IAnimal) =
    animal.MakeNoise() |> printfn "Making noise %s"

type Cat = Felix | Misifus
type Dog = Fido | Lassie

//although we cannot use our animal classes we can create Ianimal extend the F# classes
type Cat  with
    member this.AsAnimal = {new IAnimal with member a.MakeNoise() = "Meow" }
type Dog  with
    member this.AsAnimal = {new IAnimal with member a.MakeNoise() = "Woof" }

let dog = Fido
showThenoiseAnimalMakes (dog.AsAnimal)
let cat = Misifus
showThenoiseAnimalMakes (cat.AsAnimal)


// we can even use reflection!
open System.Reflection
open Microsoft.FSharp.Reflection

type Account = {Id:int; Name:string}
let fields = FSharpType.GetRecordFields(typeof<Account>) |> Array.map (fun  propInfo -> propInfo.Name, propInfo.PropertyType.Name)
