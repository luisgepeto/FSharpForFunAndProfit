
type EmailAddress = EmailAddress of string

let sendEmail (EmailAddress email) = 
    printfn "sent an email to %s" email

let aliceEmail = EmailAddress "alice@example.com"
sendEmail aliceEmail

//this wont compile
sendEmail "bob@example.com"

//printing values is checked on compile time
let printingExample = 
   printf "an int %i" 2                        // ok
   printf "an int %i" 2.0                      // wrong type
   printf "an int %i" "hello"                  // wrong type
   printf "an int %i"                          // missing param

   printf "a string %s" "hello"                // ok
   printf "a string %s" 2                      // wrong type
   printf "a string %s"                        // missing param
   printf "a string %s" "he" "lo"              // too many params

   printf "an int %i and string %s" 2 "hello"  // ok
   printf "an int %i and string %s" "hello" 2  // wrong type
   printf "an int %i and string %s" 2          // missing param


//types can be inferred from printing strings
let printAsString x = printf "%s"
let printAnInt x = printf "%i"

//we can use units of measure
[<Measure>]
type cm

[<Measure>]
type inches

[<Measure>]
type feet = 
    static member toInches(feet : float<feet>) : float<inches> =
        feet*12.0<inches/feet>

let meter = 100.0<cm>
let yard = 3.0<feet>
//we can convert units of measurement
let yardInInches = feet.toInches(yard)

//incorrect units dont match!
yard+meter

[<Measure>]
type GBP
[<Measure>]
type USD

let gbp10 = 10.0<GBP>
let usd10 = 10.0<USD>
gbp10+gbp10 //is allowed
gbp10+usd10 //not allowed
gbp10 + 1.0 //not allowed no currency!
gbp10 + 1.0<_> //ANY CURRENCY DANGER!

//On F# we cannot compare two different objects
open System
let obj = new Object()
let ex = new Exception()
let b = (obj = ex) //CANNOT DO!

//we can also deny comparison on a type
[<NoEquality;NoComparison>]
type CustomerAccount = {CustomerAccountId:int}
let x={CustomerAccountId =1}
x = x //This is a compile type error!
x.CustomerAccountId = x.CustomerAccountId // this can easily be performed :)
