module GameLogic

open Maze
open Player

type Direction = 
    | Up
    | Down 
    | Left
    | Right

type ScanResult = { left:string; up:string; right:string; down:string }

type GameLogic (maze:Maze) = 
    let IsEmptyCell cell = 
        match cell with 
        | Empty -> true
        | Start -> true 
        | End -> true
        | _ -> false
        
    let mapStrToDir str = 
        match str with 
        | "Up" -> Up
        | "Down" -> Down
        | "Left" -> Left
        | "Right"-> Right
        | _ -> failwith (sprintf "Unknown direction: %s" str)

    member this.Maze = maze
        
    member this.ValidateMove (newPos:Point) = 
        if  newPos.y < 0 || newPos.y >= this.Maze.Size.y 
            || newPos.x < 0 || newPos.y >= this.Maze.Size.x 
        then failwith (sprintf "Outside of maze. (x=%d,y=%d)" newPos.x newPos.y) 

    member private this.MoveTo (newPos:Point) (player:Player) = 
        printfn "MoveTo: (%d,%d)  player:%A" newPos.x newPos.y (player.ToString())
        this.ValidateMove newPos
        let targetCell = maze.GetCell newPos 
        let playerAfterMove = 
            if IsEmptyCell targetCell 
            then Player(player.Name, newPos, Success, targetCell)
            else Player(player.Name, player.Position, Failure, targetCell)
        printfn "Result: %A" (playerAfterMove.ToString())
        playerAfterMove
    
    member this.Move (dir:string) (player:Player) = 
        printfn "Move %s %A" dir (player.ToString())
        let newPos = mapStrToDir dir |> function
            | Up    -> {x=player.Position.x; y=player.Position.y - 1}
            | Down  -> {x=player.Position.x; y=player.Position.y + 1}
            | Left  -> {x=player.Position.x - 1; y=player.Position.y}
            | Right -> {x=player.Position.x + 1; y=player.Position.y}
        player |> this.MoveTo newPos

    member this.ScanAround (player:Player) = 
        printfn "ScanAround %A" (player.ToString())
        let x,y = player.Position.x, player.Position.y
        let symbolForCell point = point |> maze.GetCell |> Maze.Cell2Symbol
        { left  = symbolForCell {x=x-1;y=y}; 
          up    = symbolForCell {x=x;y=y-1}; 
          right = symbolForCell {x=x+1;y=y}; 
          down  = symbolForCell {x=x;y=y+1}}
        