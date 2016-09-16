module GameApi

open FSharp.Data
open Maze

let serverUrl = "http://localhost:8083"

type ScanResultJson = JsonProvider<"""{
                                  "left": "#",
                                  "up": "#",
                                  "right": " ",
                                  "down": "#"
                                }""">

type ScanResult = {up:Cell; down:Cell; right:Cell; left:Cell}

type MoveResultJson = JsonProvider<"""{
                                  "position": {
                                        "x": 3,
                                        "y": 1
                                      },
                                  "details": " ",
                                  "outcome": "success"
                                }""">

type StartCompetionResultJson = JsonProvider<"""{
                                                "startPoint": {
                                                "x": 2,
                                                "y": 1
                                                },
                                                "endPoint": {
                                                "x": 3,
                                                "y": 5
                                                }
                                            }""">

type GreatTeamResultJson = JsonProvider<"""{ "greeting": "Hi teamABC!" }""">


let sendPostRequest urlExt = 
    printfn "%s[Sending: %s]" System.Environment.NewLine urlExt
    Http.RequestString ( 
            url = sprintf "%s%s" serverUrl urlExt, 
            headers = [HttpRequestHeaders.ContentType HttpContentTypes.Json],
            body = TextRequest """ {"teamId":'MazePlayer', 'mazeId':'mazeXXX'} """ )

let scanAround() : ScanResult = 
    sendPostRequest "/Scan" |> ScanResultJson.Parse 
    |> fun scan -> { up=Maze.Str2Cell scan.Up; 
                    down=Maze.Str2Cell scan.Down; 
                    left=Maze.Str2Cell scan.Left; 
                    right=Maze.Str2Cell scan.Right } 

let move dir = 
    sendPostRequest (sprintf "%s%s" "/Move" dir) |> MoveResultJson.Parse

let startCompetition() = 
    sendPostRequest "/StartCompetition" |> StartCompetionResultJson.Parse

let greatTeam() = 
    sendPostRequest "/GreatTeam" |> GreatTeamResultJson.Parse
