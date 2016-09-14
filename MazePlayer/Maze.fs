module Maze

open System

type Cell = 
    | Wall
    | Empty
    | Start
    | End

type Point = { x:int; y:int }

let Cell2Symbol cell = 
        match cell with 
        | Wall -> "#"
        | Empty -> " "
        | Start -> "A"
        | End -> "B"

type Maze(desc:string) = 
    let char2Cell (ch:char) : Cell =
        match ch with 
        | '#' -> Wall 
        | ' ' -> Empty
        | 'A' -> Start 
        | 'B' -> End
        | _ -> failwith (sprintf "Unknown character %c" ch)
        
        
//    member this.Print() = 
//        let changeCellRowToSymbols rows = 
//            rows |> Array.map Cell2Symbol
//        let printCellRow i cellRow = 
//            changeCellRowToSymbols cellRow |> printfn "%i: %A" i 
//        array2d |> Array.iteri printCellRow 

