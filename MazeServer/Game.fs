module Game

open System.IO

open Maze
open GameLogic
open Player

type MoveResult = { position:Point; details:string; outcome:string; }
type StartEndPoint = { startPoint:Point; endPoint:Point }

type Game (mapFilepath : string) = 
    
    let maze = File.ReadAllText(mapFilepath) |> Maze 
    
    let gameLogic = maze |> GameLogic 
    
    let start = maze.Start
    
    let mutable player = Player("X", start, Success, maze.GetCell start)

    member this.DebugPrintMaze = 
        printfn "%A" (maze.Print())
        printfn "Start = (%d,%d)" maze.Start.x maze.Start.y
        printfn "End = (%d,%d)" maze.End.x maze.End.y
            
    member this.Move direction = 
        player <- gameLogic.Move direction player
        { position=player.Position; 
          details=  player.Target |> Maze.Cell2Symbol; 
          outcome= player.Result |> function 
                | Success -> "success" 
                | _ -> "failure" }

    member this.ScanAround() = gameLogic.ScanAround player

    member this.StartAndEnd = { startPoint = maze.Start; endPoint = maze.End }

    member this.MazeToHtml = maze.ToHTML