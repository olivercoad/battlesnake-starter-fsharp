// Implementation of your battlesnake
module Battlesnake

open BattlesnakeAPI

let info () = {
    apiversion = "1"
    author = None
    color = Some "#b845fc"
    head = None
    tail = None
    version = None
}

let startGame gameState =
    printfn "Starting game %s: %A" gameState.game.id gameState

let endGame gameState =
    printfn "Ending game %s" gameState.game.id

type Move = Up | Down | Left | Right

let move gameState =
    
    // Step 0: Don't let your Battlesnake move back on it's own neck
    let myHead = gameState.you.head
    let myNeck = gameState.you.body.[1]

    let possibleMoves = [
        if myNeck.x >= myHead.x then
            Left
        if myNeck.x <= myHead.x then
            Right
        if myNeck.y >= myHead.y then
            Down
        if myNeck.y <= myHead.y then
            Up
    ]

    // TODO: Step 1 - Don't hit walls.
    // Use information in gameState to prevent your Battlesnake from moving beyond the boundaries of the board.
    // const boardWidth = gameState.board.width
    // const boardHeight = gameState.board.height

    // TODO: Step 2 - Don't hit yourself.
    // Use information in gameState to prevent your Battlesnake from colliding with itself.
    // const mybody = gameState.you.body

    // TODO: Step 3 - Don't collide with others.
    // Use information in gameState to prevent your Battlesnake from colliding with others.

    // TODO: Step 4 - Find food.
    // Use information in gameState to seek out and find food.

    // Finally, choose a move from the available safe moves.
    // TODO: Step 5 - Select a move to make based on strategy, rather than random.
    let safeMoves = possibleMoves

    printfn "Safe Moves: %A" safeMoves
    
    let randomIdx = System.Random().Next(List.length safeMoves)
    let chosenMove = safeMoves.[randomIdx]


    printfn $"{gameState.game.id} MOVE {gameState.turn}: {chosenMove}"

    {
        move = chosenMove.ToString().ToLowerInvariant()
        shout = Some <| sprintf "turn %i" gameState.turn
    }