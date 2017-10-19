open System.Net.WebRequestMethods

let rec movingAverages list = 
    match list with
    | [] -> []
    | x::y::rest ->
        let avg = (x+y)/2.0
        avg :: movingAverages (y :: rest)
    | [_] -> []

//movingAverages [1.0]
//movingAverages [1.0;2.0]
//movingAverages [1.0;2.0;3.0]

type Result<'a, 'b> = 
    | Success of 'a
    | Failure of 'b

type FileErrorReason = 
    | FileNotFound of string
    | UnauthorizedAccess of string * System.Exception

let performActionOnFile action filePath = 
    try
        use sr = new System.IO.StreamReader(filePath:string)
        let result = action sr
        sr.Close()
        Success (result)
    with
        | :? System.IO.FileNotFoundException as ex -> Failure (FileNotFound filePath)
        | :? System.Security.SecurityException as ex -> Failure(UnauthorizedAccess(filePath, ex))


let middleLayerDo action filePath =
    let fileResult = performActionOnFile action filePath
    fileResult

let topLayerDo action filePath = 
    let fileResult = middleLayerDo action filePath
    fileResult

//any client accessing the top layer will be forced to match against a failure pattern
let printFirstLineOfFile filePath = 
    let fileResult = topLayerDo (fun fs -> fs.ReadLine()) filePath
    match fileResult with
    | Success result -> printfn "first line is: '%s'" result
    | Failure reason -> 
        match reason with
        | FileNotFound file -> printfn "file not found : '%s'" file
        | UnauthorizedAccess (file,  _) -> printfn "file not accessible : '%s'" file

//we must explicitly ignore any failure
let printLengthOfFile filePath = 
    let fileResult = topLayerDo (fun fs -> fs.ReadToEnd().Length) filePath
    match fileResult with
    | Success result -> printfn "length is :%i" result
    | Failure _ -> printfn "Something happened"

let writeSomeText filePath someText = 
    use writer = new System.IO.StreamWriter(filePath:string)
    writer.WriteLine(someText:string)
    writer.Close()

let goodFileName = "good.txt"
let badFileName = "bad.txt"

writeSomeText goodFileName "hello"