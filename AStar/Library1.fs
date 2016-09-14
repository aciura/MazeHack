module AStar

open System.Collections.Generic

type Weight =
  | Blocked
  | Weight of weight:int

type Node =
  {
    x : int
    y : int
    weight : Weight
  }

let Directions =
  [
    { x =  1; y =  0; weight = Blocked }
    { x =  0; y = -1; weight = Blocked }
    { x = -1; y =  0; weight = Blocked }
    { x =  0; y =  1; weight = Blocked }
  ]

type Graph =
    {
        nodes : Dictionary<(int * int), Node>
        cameFrom : Dictionary<Node, Node>
        costSoFar : Dictionary<Node, int>
        path : Node list        
    }
    with 
    member this.neighbors_of (node:Node) = 
            Directions |> 
            List.map (fun dir -> this.nodes.TryGetValue(node.x + dir.x, node.y + dir.y))

//let reconstruct_path (cameFrom:Dictionary<Node,Node>) (current:Node) =
//    let mutable total_path = [current]
//    let mutable node = current
//    while cameFrom.ContainsKey node do
//        node <- cameFrom.[node]
//        total_path <- node::total_path
//    total_path
//
//let AStar (graph:Graph) (start:Node) (goal:Node) estimateCost = 
//    // The set of nodes already evaluated.
//    let closedSet : Node list = [] 
//
//    // The set of currently discovered nodes still to be evaluated.
//    // Initially, only the start node is known.
//    //let openSet : Node list = [start]
//    let mutable (openSet : (Node * int) list) = []
//    //fake a priority queue using lists
//    let addToOpenSet value = openSet <- value :: openSet
//    let removeFromOpenSet () : Node*int =
//      let sorted = openSet |> List.sortBy (fun (_, priority) -> priority)
//      openSet <- sorted.Tail
//      sorted.Head
//    
//    // For each node, which node it can most efficiently be reached from.
//    // If a node can be reached from many nodes, cameFrom will eventually contain the
//    // most efficient previous step.
//    let cameFrom = Dictionary<Node, Node>()
//
//    // For each node, the cost of getting from the start node to that node.
//    // Map with default value of Infinity
//    let gScore = Dictionary<Node,int>() 
//    // The cost of going from start to start is zero.
//    gScore.Add(start, 0)
//
//    // For each node, the total cost of getting from the start node to the goal
//    // by passing by that node. That value is partly known, partly heuristic.
//    let fScore = Dictionary<Node, int>()
//    // For the first node, that value is completely heuristic.
//    fScore.[start] <- estimateCost(start, goal)
//    
//    let mutable break' = false
//
//    while (not openSet.IsEmpty && break' <> true) do
//        //the node in openSet having the lowest fScore[] value
//        //TODO: openSet should be a heap or fScore?
//        let current,cost = removeFromOpenSet ()
//        
//        //printfn "current=%A cost=%d" current cost
//
//        if current = goal then
//            break' <- true
//            reconstruct_path cameFrom current |> ignore
//        else
//            for neighbor in (graph.neighbors_of current) do
//            // Ignore the neighbor which is already evaluated.                
//            if not closedSet.Contains neighbor then 
//                // The distance from start to a neighbor
//                let tentative_gScore = gScore.[current] + dist_between(current, neighbor)
//                if not openSet.Contains neighbor then // Discover a new node
//                    openSet.Add(neighbor)
//                else if tentative_gScore < gScore.[neighbor] then
//                    // This path is the best until now. Record it!
//                    cameFrom.[neighbor] <- current
//                    gScore.[neighbor] <- tentative_gScore
//                    fScore.[neighbor] <- gScore.[neighbor] + (estimateCost neighbor goal)                
//    
//    failwith "No path man"
