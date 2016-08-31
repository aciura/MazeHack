module Main 

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

open Game
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

type TimeResponse = {currentTime:System.DateTime}
type Team = {teamId:string}
type GreetingResponse = {greeting:string}
type TeamAndMaze = {teamId:string; mazeId:string}

let JSON v =
    let jsonSerializerSettings = JsonSerializerSettings()
    jsonSerializerSettings.ContractResolver <- Serialization.CamelCasePropertyNamesContractResolver()
    jsonSerializerSettings.Formatting <- Formatting.Indented
    OK (JsonConvert.SerializeObject(v, jsonSerializerSettings))
    >=> Writers.setMimeType "application/json; charset=utf-8"

let fromJson<'a> json =
    JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a

let getResourceFromReq<'a> (req : HttpRequest) =
    let getString rawForm =
        System.Text.Encoding.UTF8.GetString(rawForm)    
    let obj = req.rawForm |> getString |> fromJson<'a>
    printfn "%A" obj
    obj

//game has to be mutable as it requires path to maze file from startup args
let mutable game = Game "maze.txt"

let movePlayer direction teamId = 
    printfn "MovePlayer dir=%s teamId=%s" direction teamId
    game.Move direction

let Greeting (requestBody:Team) = {greeting = sprintf "Hi %s!" requestBody.teamId}
let ReturnStartEndPoints (teamAndMaze:TeamAndMaze) = game.StartAndEnd
let ScanAround (teamAndMaze) = game.ScanAround()

let app : WebPart = 
    choose [ 
        GET >=> choose [ 
            path "/HealthCheck" >=> warbler (fun _ -> {currentTime=System.DateTime.Now} |> JSON)
            path "/Maze" >=> warbler (fun _ -> OK (game.MazeToHtml))            
        ]
        POST >=> choose [ 
            path "/StartCompetition" >=> request (getResourceFromReq >> ReturnStartEndPoints >> JSON)

            pathScan "/Move%s" (fun dir -> 
                request (fun req -> 
                    (getResourceFromReq<TeamAndMaze> req).teamId 
                    |> movePlayer dir 
                    |> JSON))

            path "/GreatTeam" >=> request (getResourceFromReq<Team> >> Greeting >> JSON)
            path "/Scan" >=> request (getResourceFromReq<TeamAndMaze> >> ScanAround >> JSON)
        ]        
    ]    

[<EntryPoint>]
let main argv = 
    printfn "Starting up server. Args:%A" argv
    let mazeFilepath = if argv.Length > 0 then argv.[0] else "maze.txt"
    game <- Game mazeFilepath
    startWebServer defaultConfig app
    0 
