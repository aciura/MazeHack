module Main 

[<EntryPoint>]
let main argv = 
    
    GameApi.greatTeam() |> ignore
    
    let startInfo = GameApi.startCompetition()
    printfn "start:%A \n\r target:%A" startInfo.StartPoint startInfo.EndPoint
        
    let result = GameApi.scanAround()
    printfn "%A" result

    let result = GameApi.move "Right"
    printfn "%A" result

    let result = GameApi.move "Up"
    printfn "%A" result

    0

