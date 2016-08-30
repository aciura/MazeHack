module Game

open System.IO

open Maze
open GameLogic
open Player

type MoveResult = { position:Point; details:string; outcome:string; }
type StartEndPoint = { startPoint:Point; endPoint:Point }

let maze = File.ReadAllText(@"maze.txt") |> Maze 

printfn "%A" (maze.Print())
printfn "Start = %A" (maze.Start)
printfn "End = %A" (maze.End)

let gameLogic = maze |> GameLogic 

let start = maze.Start
let mutable player = Player("X", start, Success, maze.GetCell start)

let Init argv = None //TODO    

let Move direction = 
    player <- gameLogic.Move direction player
    { position=player.Position; 
      details=  player.Target |> Maze.Cell2Symbol; 
      outcome= player.Result |> function 
            | Success -> "success" 
            | _ -> "failure" }

let StartAndEndPoint = { startPoint = maze.Start; endPoint = maze.End }