
//sometimes it is convenient to switch to an object oriented style
//one behvaior is to dot into a class
//this is done via type extensions
//all types can have functions attached to them
module Person = 
    type T = {First: string; Last:string} with
        member this.FullName = this.First + " " + this.Last //we attach the function to the type

    let create first last = {First= first;Last = last}

    //members can be added later
    type T with
        member this.SortableName = this.Last + ", "+ this.First

     //members can be 

//test
let person = Person.create "john" "doe"
let fullname = person.FullName
let sortableName = person.SortableName

//the with keyword indicates start of list of memvers
//member keyword shows a member
//keyword this is a placeholder for the object being dotted into
//members can be declared after also!

//optional extensions
//you can add extra members from a completely different module
//(like extension methods)
module PersonExtensions = 
    type Person.T with
    member this.UpperCaseName = 
        this.FullName.ToUpper()

//test
open PersonExtensions
let upperCaseName = person.UpperCaseName

//we can also extend system types
//but we need to use the fully qualified system type
type System.Int32 with
    member this.IsEven = this % 2 = 0

//test
let i = 20
if i.IsEven then printfn "is even"

//we can make member functions static by adding the static keyword and dropping the this placeholder
type Person.T with
    static member Factory : Person.T = {First="default"; Last="person"}

let person' = Person.T.Factory
//we can do the same for other method types
type System.Double with 
    static member Pi = "3.4"
let pi = System.Double.Pi

//ATTACHING EXISTING FUNCTIONS
// we can attach preexisting functions to a type
//most of them can be accessed by a standalone function 
// but they are also appended to a type
let list = [1..10]
let len1 = List.length list
let len1' = list.Length

// or another example
module Person' =
    type T = {First: string; Last:string}
    let create first last = {First= first;Last = last}
    let fullName {First = first; Last = last} = first + " " + last
    type T with 
        member this.FullName = fullName this

let person' = Person'.create "john" "smith"
let fullName = person'.FullName //oo style
let fullName2 = Person'.fullName person' // functional style

//attaching existing function siwht multiple parameters
//if the this parameter is first we dont need to respecify all parameters
module Person'' =
    type T = {First: string; Last:string}
    let create first last = {First= first;Last = last}
    let hasSameFirstAndLastName (person:T) otherFirst otherLast =
        person.First = otherFirst && person.Last = otherLast
    type T with 
        member this.HasSameFirstAndLastName = hasSameFirstAndLastName this //due to partiall application we can do this!

let person'' = Person''.create "john" "smith"
person''.HasSameFirstAndLastName "john" "smith"
Person''.hasSameFirstAndLastName person'' "john" "smith"

//TUPLE FORM METHODS
//curried form methods arem ore functional vs tuples
type Product = {SKU:string; Price: float} with

    // curried style
    member this.CurriedTotal qty discount = 
        (this.Price * float qty) - discount

    // tuple style
    member this.TupleTotal(qty,discount) = 
        (this.Price * float qty) - discount

//testing
let product = {SKU="ABC"; Price=2.0}
let total1 = product.CurriedTotal 10 1.0
let total2 = product.TupleTotal (10,1.0)

//however we can partially applied the curreid total!
let totalFor10 = product.CurriedTotal 10
let discounts = [1.0..5.0]
let totalForDifferentDiscounts = discounts |> List.map totalFor10 // this cant be done with a tuple parameter

//however we can do named and optional parameters as well as overloading with tuples
//named parameters
let total3 = product.TupleTotal (qty=10, discount=1.0)
let total4 = product.TupleTotal (discount=1.0,qty=10) //named parametes must always be last
//we can specify optional parameters with typle style

type Product with
    member this.TutpleTotal2 (qty, ?discount) = //optional parameters come wrapped in an optional type
        let extPrice = this.Price * float qty
        match discount with
        | None -> extPrice
        | Some discount -> extPrice - discount

let total5 = product.TutpleTotal2(10)

//we can sumarize pattern matching with the defaultArg function
type Product with
    member this.TutpleTotal3 (qty, ?discount) = //optional parameters come wrapped in an optional 
        let extPrice = this.Price * float qty
        let discount = defaultArg discount 0.0
        extPrice - discount

//method overloading
//methods can only be overloaded for functions attahced to a type
//not make sense in functional style programming
//this due to the ability to pattern match arguments!
type Product with
    member this.TupleTotal4 (qty) =
        printfn "using non discountmethod"
        this.Price * float qty
    member this.TupleTotal4 (qty,discount) =
        printfn "using discount method"
        this.Price * float qty - discount


//HOWEVER
//methods dont play well with type inference
open Person'
let printFullName person =
    printfn "Name is %s" << fullName <| person


