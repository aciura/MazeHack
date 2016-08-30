module Player

type Result = 
    | Success 
    | Failure

type Player(name:string, position:Maze.Point, result:Result, targetCell) =  
    member this.Name = name
    member this.Position = position
    member this.Result = result
    member this.Target : Maze.Cell = targetCell
    override this.ToString() = 
        sprintf "%s (%d,%d) Res:%A target:%A" this.Name this.Position.x this.Position.y this.Result this.Target
