module Maze

open System
open System.Collections.Generic

let MAZE_INIT_SIZE = 512

type Cell = 
    | Wall
    | Empty
    | Start
    | Target

type Point = { x:int; y:int }

let Cell2Symbol cell = 
        match cell with 
        | Wall -> "#"
        | Empty -> " "
        | Start -> "A"
        | Target -> "B"

let Str2Cell (ch) : Cell =
        match ch with 
        | "#" -> Wall 
        | " " -> Empty
        | "A" -> Start 
        | "B" -> Target
        | _ -> failwith (sprintf "Unknown character %s" ch)


type Maze (start:Point, target:Point) = 
    let cells = Dictionary<Point,Cell>(MAZE_INIT_SIZE)
        
    member this.Start = start
    member this.Target = target 

    member this.SetCell (position:Point) (cell:Cell) : unit = 
        cells.[position] <- cell

    member this.GetCell (position:Point) : Cell option= 
        match cells.TryGetValue(position) with 
        | false,_ -> None
        | true,cell -> Some cell
        
//    member this.Print() = 
//        let changeCellRowToSymbols rows = 
//            rows |> Array.map Cell2Symbol
//        let printCellRow i cellRow = 
//            changeCellRowToSymbols cellRow |> printfn "%i: %A" i 
//        array2d |> Array.iteri printCellRow 

