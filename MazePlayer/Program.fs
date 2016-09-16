module Main 

open System
open Maze
open GameApi
open FSharp.Data

type Player (position:Point) = 
    member this.Position = position

let Directions =
  [
    { x =  1; y =  0; }
    { x =  0; y = -1; }
    { x = -1; y =  0; }
    { x =  0; y =  1; }
  ]


let rnd = System.Random()
let isEmptyCell (maze:Maze) (position:Point) : bool = 
    match maze.GetCell position with 
    | Some Empty | Some Start | Some Target -> true
    | _ -> false 

let positionToDirection position (newPos:Point) =
        match {x = newPos.x - position.x; y= newPos.y - position.y} with 
        | {x=1;  y=0} -> "Right"
        | {x= -1; y=0} -> "Left"
        | {x=0;  y=1} -> "Down"
        | {x=0;  y= -1} ->"Up"
        | _ -> failwith (sprintf "Impossible! Current position:%A New Position:%A " position newPos)

let RndMove (position:Point) (maze:Maze) =
    let moves = 
        Directions
        |> List.map (fun dir -> {x= position.x + dir.x; y= position.y + dir.y})
        |> List.filter (isEmptyCell maze)     
    
    printfn "Available moves: "
    moves |> List.iter (fun e -> printf "%A" e)
    
    moves.[rnd.Next(moves.Length)] |> positionToDirection position

let updateMaze (maze:Maze) (position:Point) (scanResult:ScanResult) = 
    maze.SetCell {x=position.x; y=position.y-1} scanResult.up
    maze.SetCell {x=position.x; y=position.y+1} scanResult.down
    maze.SetCell {x=position.x+1; y=position.y} scanResult.right
    maze.SetCell {x=position.x-1; y=position.y} scanResult.left

let rec GameLoop i (maze:Maze) (player:Player)  = 
    if player.Position <> maze.Target then 
        printfn "--- iter:%d ---" i
        printfn "Player pos=(%d,%d)" player.Position.x player.Position.y
        
        let scanResult = GameApi.scanAround()
        printfn "%s Scan = %A" Environment.NewLine scanResult

        updateMaze maze player.Position scanResult
        
        let nextMove = RndMove player.Position maze
        let result = GameApi.move nextMove
        printfn "%s Move Result = %A" Environment.NewLine result
        let player' = Player { x=result.Position.X; y=result.Position.Y }

        GameLoop (i+1) maze player'
    else 
        printfn "Player %A reached target %A" player maze.Target


[<EntryPoint>]
let main argv = 
    
    GameApi.greatTeam() |> ignore

    let startInfo = GameApi.startCompetition()
    printfn "start:%A \n\r target:%A" startInfo.StartPoint startInfo.EndPoint
    let start = {x=startInfo.StartPoint.X; y=startInfo.StartPoint.Y}
    let target = {x=startInfo.EndPoint.X; y=startInfo.EndPoint.Y}
    let maze = new Maze (start, target)
    let player = Player (start)

    GameLoop 0 maze player

    0

