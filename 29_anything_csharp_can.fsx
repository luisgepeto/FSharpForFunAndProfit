
//we can have examples of interface, abstract class and concrete class inheriting
//interface
type IEnumerator<'a> =
    abstract member Current : 'a
    abstract MoveNext : unit -> bool

//abstract base class with virtual methods
[<AbstractClass>]
type Shape() =
    abstract member Width : int with get
    abstract member Height : int with get
    member this.BoundingArea = this.Height * this.Width
    abstract member Print : unit -> unit
    default this.Print () = printfn "Im a shape"

//Concreete class implementing and overriding
type Rectangle(x:int, y:int) = 
    inherit Shape()
    override this.Width = x
    override this.Height = y
    override this.Print () =  printfn "Im a rectangle"

//test
let r = Rectangle(2,3)
printfn "The width is %i" r.Width
printfn "The area is %i" r.BoundingArea
r.Print()

//classes can have multiple constructors and mutable propertie s
type Circle(rad:int) =
    inherit Shape()
    let mutable radius = rad
    override this.Width = rad*2
    override this.Height = rad*2
    //we can add alternate constuctor with default radius
    new () = Circle(10)
    member this.Radius
        with get() = radius
        and set(value) = radius <- value


// test constructors
let c1 = Circle()   // parameterless ctor
printfn "The width is %i" c1.Width
let c2 = Circle(2)  // main ctor
printfn "The width is %i" c2.Width

// test mutable property
c2.Radius <- 3
printfn "The width is %i" c2.Width


//we can also have generics
type KeyValuePair<'a, 'b>(key:'a, value:'b) = 
    member this.Key = key
    member this.Value =value

type Container<'a,'b 
    when 'a:equality
    and 'b:> System.Collections.ICollection > 
    (name:'a, values:'b) =
        member this.Name = name
        member this.Values = values

//we can have structs
type Point2D = 
    struct
        val X:float
        val Y:float
        new(x:float, y:float) = {X=x; Y=y}
    end
let p = Point2D()
let p2 = Point2D(2.0, 3.0)


//exceptions
exception MyError of string
try 
    let e = MyError("oops")
    raise e
with
    |MyError msg -> printfn "The exception error was %s" msg
    |_ -> printfn "something else happened"


//extension methods
type System.String with 
    member this.StartsWithA = this.StartsWith "A"

type System.Int32 with
    member this.IsEven = this % 2 = 0

let s = "Alice"
printfn "'%s' starts with an 'A' = %A" s s.StartsWithA
let i = 20
if i.IsEven then printfn "'%i' is even" i


//parameter arrays
open System
type MyConsole() =
    member this.WriteLine([<ParamArray>] args: Object[]) =
        for arg in args do
            printfn "%A" arg

let cons = new MyConsole()
cons.WriteLine("abc", 42, 3.14, true)

//delegates
type MyDelegate  = delegate of int -> int
let f = MyDelegate  (fun x -> x * x)
let result f.Invoke(5)

//enums
type Color = | Red = 1 | Green = 2 | Blue = 3
let color1 = Color.Red
let color2:Color = enum 2
let color3 = System.Enum.Parse(typeof<Color>, "Green") :?> Color //downcasting to color type
