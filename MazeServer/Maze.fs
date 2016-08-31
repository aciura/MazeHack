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
        
    let Mazify (desc : string) =
        let lines = desc.Replace('\r',' ').Split('\n') 
        //printfn "%A" lines
        let lineToCellArr (line:string) = 
            line.ToCharArray() |> Array.map char2Cell
        lines |> Array.map lineToCellArr 

    //array2d : Cell [] []
    let array2d = Mazify desc
    
    //array2d' : Cell [,] - real array2d representation 
    //  it's not so useful as there are not many Array2d.* functions implemented
    //  there are just: Array2D.map, Array2D.iter
    let array2d' = Array2D.zeroCreate array2d.[0].Length array2d.Length
    let _ = array2d |> Array.iteri (fun j arr -> 
                              arr |> Array.iteri (fun i cell -> array2d'.[i,j] <- cell ) )          
    
    let findSpecificCell searchedFor = 
        let point = 
            array2d |> Array.fold (fun (i,j) arr -> 
                    match i with
                    | Some i -> (Some i,j)
                    | None -> (arr |> Array.tryFindIndex (fun cell -> cell = searchedFor)),j+1)
                (None,-1)
        match fst point with 
        | Some x -> {x=x; y=snd point} 
        | None -> failwith (sprintf "%A point not found" searchedFor)                

    member this.Start = findSpecificCell Start
    member this.End = findSpecificCell End
    member this.Size : Point = {x=array2d.[0].Length; y=array2d.Length}

    member this.GetCell (pos:Point) = array2d.[pos.y].[pos.x]
    
    member this.ToHTML =
        let changeCellRowToSymbols rows = 
            rows |> Array.map Cell2Symbol 
        let printCellRow i cellRow = 
            changeCellRowToSymbols cellRow 
            |> Seq.ofArray 
            |> String.concat ""
            |> sprintf "%i: %A " i 
        array2d 
        |> (Array.mapi printCellRow) 
        |> Seq.ofArray 
        |> String.concat "<br/>"

    member this.Print() = 
        let changeCellRowToSymbols rows = 
            rows |> Array.map Cell2Symbol
        let printCellRow i cellRow = 
            changeCellRowToSymbols cellRow |> printfn "%i: %A" i 
        array2d |> Array.iteri printCellRow 

