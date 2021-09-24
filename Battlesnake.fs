/// Implementation of your battlesnake
module Battlesnake

open BattlesnakeAPI

let info () = {
    Apiversion = "1"
    Author = None
    Color = Some "#b845fc"
    Head = None
    Tail = None
    Version = None
}

let startGame gameState =
    printfn "Starting game %s: %A" gameState.Game.Id gameState

let endGame gameState =
    printfn "Ending game %s" gameState.Game.Id

let move gameState =
    let occupiedPositions = set [
        // Step 0 - Avoid snakes
        yield! List.collect (fun snake -> snake.Body) gameState.Board.Snakes

        // TODO: Step 3 - Avoid walls
        // yield positions of walls
        // 0..gameState.Board.Width-1
    ]

    let myHead = gameState.You.Head
    let allMoves = [
        // TODO: Step 1 - List all directions along with where head would end up
        Down, { myHead with Y = myHead.Y - 1 }
        // Up, { myHead with ... }
        // Left, ...
        // Right, ...
    ]

    let moves =
        allMoves 
        //// TODO: Step 2 - filter out occupiedPositions
        //// use Set.contains
        // |> List.filter (fun (direction, point) -> ??)

        //// TODO: Step 4 - find food
        // |> List.sortBy (fun (direction, point) -> ??)

    // Choose a move from the available moves
    match moves with
    | [] -> { Move = Down; Shout = Some "No possible moves :(" }
    | (firstMove,_)::_ -> { Move = firstMove; Shout = None }

let server = {
    Info = info
    StartGame = startGame
    EndGame = endGame
    Move = move
}