open System
open System.Timers
open System.Threading

//traditional asynchronous

let userTimerWithCallback = 
    let event = new AutoResetEvent(false)
    let timer = new System.Timers.Timer(2000.0)
    timer.Elapsed.Add (fun _ -> event.Set() |> ignore ) //remember that ignore returns a unit
    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()
    printfn "Doing something useful while waiting for event"
    event.WaitOne() |> ignore
    printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

//built in asynchronous workflows
let userTimerWithAsync =
    let timer = new System.Timers.Timer(2000.0)
    let timerEvent = Async.AwaitEvent (timer.Elapsed) |> Async.Ignore
    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()
    printfn "Doing something useful while waiting for event"
    Async.RunSynchronously timerEvent
    printfn "Timer ticked at %O" DateTime.Now.TimeOfDay


//THIS IS INCOMPLETE!