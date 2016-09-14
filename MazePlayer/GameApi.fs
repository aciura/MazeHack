module GameApi

open FSharp.Data

let serverUrl = "http://localhost:8083"

type ScanResult = JsonProvider<"""{
                                  "left": "#",
                                  "up": "#",
                                  "right": " ",
                                  "down": "#"
                                }""">

type MoveResult = JsonProvider<"""{
                                  "position": {
                                        "x": 3,
                                        "y": 1
                                      },
                                  "details": " ",
                                  "outcome": "success"
                                }""">

type StartCompetionResult = JsonProvider<"""{
                                                "startPoint": {
                                                "x": 2,
                                                "y": 1
                                                },
                                                "endPoint": {
                                                "x": 3,
                                                "y": 5
                                                }
                                            }""">

type GreatTeamResult = JsonProvider<"""{ "greeting": "Hi teamABC!" }""">


let sendPostRequest urlExt = 
    Http.RequestString ( 
            url = sprintf "%s%s" serverUrl urlExt, 
            headers = [HttpRequestHeaders.ContentType HttpContentTypes.Json],
            body = TextRequest """ {"teamId":'MazePlayer', 'mazeId':'mazeXXX'} """ )

let scanAround() = 
    sendPostRequest "/Scan" |> ScanResult.Parse

let move dir = 
    sendPostRequest (sprintf "%s%s" "/Move" dir) |> MoveResult.Parse

let startCompetition() = 
    sendPostRequest "/StartCompetition" |> StartCompetionResult.Parse

let greatTeam() = 
    sendPostRequest "/GreatTeam" |> GreatTeamResult.Parse
