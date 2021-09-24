/// The object definitions for the Battlesnake webserver
/// These types are to be encoded/decoded as camelCase json.
/// https://docs.battlesnake.com/references/api#object-definitions
module BattlesnakeAPI

/// Give info about your snake as response to GET requests to /
type BattlesnakeInfo = {
    /// version of the battlesnake API implemented
    Apiversion: string
    /// username of the author of this battlesnake
    Author: string option
    /// hex color code to display this battlesnake
    Color: string option
    /// displayed head of this battlesnake
    Head: string option
    /// displayed tail of this battlesnake
    Tail: string option
    /// a version number of tag for your snake
    Version: string option
}

/// Identifies and describes the game
type Game = {
    /// a unique identifier for this game
    Id: string
    /// information about the ruleset being used to run this game
    Ruleset: Ruleset
    /// (milliseconds) how much time your snake has to respond to requests
    Timeout: int
}

/// Information about the ruleset being used
and Ruleset = {
    /// name of the ruleset this game
    Name: string
    /// the release version of the rules module
    Version: string
    /// a collection of specific settings used by the current game
    Settings: RulesetSettings option
}

/// Specific settings being used
and RulesetSettings = {
    /// percent change of spawning a new food every round
    FoodSpawnChance: int
    /// minimum food to keep on the board every turn
    MinimumFood: int
    /// extra health damage taken when ending turn in hazard
    HazardDamagePerTurn: int
    /// only apply in royale mode
    Royale: Royale option
    /// only apply in squad mode
    Squad: Squad option
}

/// Rules that relate to royale mode
and Royale = {
    /// number of turns between generating new hazards
    ShrinkEveryNTurns: int
}

/// Rules that relate to squad mode
and Squad = {
    /// allow members of same squad to move over each other
    AllowBodyCollisions: bool
    /// all squad members eliminated when one is eliminated
    SharedElimination: bool
    /// all squad members share health
    SharedHealth: bool
    /// all squad members share length
    SharedLength: bool
}

/// Represents cell position on the board. (0, 0) is bottom left.
type Point = {
    /// Distance from left
    X: int
    /// Distance from bottom
    Y: int
}

/// Information about a snake and its current state
type Battlesnake = {
    /// unique identifier for this battlesnake in the context of the current game
    Id: string
    /// name given by its author
    Name: string
    /// health balue between 0-100 inclusive
    Health: int
    /// list of coordinates representing location on the snake on the game board from head to tail
    Body: Point list
    /// the previous response time in milliseconds ("0" means timed out or failed)
    /// needs to be parsed manually.
    Latency: string // Battlesnake encodes latency as a string not an int for some reason
    /// coordinates of the head of the snake = List.head Body
    Head: Point
    /// length from head to tail = Body.Length
    Length: int
    /// message shouted by this battlesnake in previous turn
    Shout: string
    /// the squad that the battlesnake belongs to
    Squad: string
}

/// Represents the state of the board.
/// (0,0) is at bottom left
type Board = {
    /// number of rows in y-axis
    Height: int
    /// number of columns in x-axis
    Width: int
    /// coordinates representing for locations on the game board
    Food: Point list
    /// coordinates representing hazardous locations
    Hazards: Point list
    /// all remaining battlesnakes (including self)
    Snakes: Battlesnake list
}

/// Body of requests made to /start /end or /move
type GameState = {
    /// Game object describing the game being played
    Game: Game
    /// Turn number for this move
    Turn: int
    /// Board object describing the game board on this turn
    Board: Board
    /// Battlesnake describing your Battlesnake
    You: Battlesnake
}

/// Direction to move in
[<Fable.Core.StringEnum>]
type Direction = Up | Down | Left | Right

/// Response to requests to /move
type MoveResponse = {
    /// your move for this turn
    Move: Direction
    /// optionally send a message to all other battlesnakes 256 char or less
    Shout: string option
}

/// The API for a Battlesnake server
type BattlesnakeServer = {
    /// GET /
    Info: unit -> BattlesnakeInfo
    /// POST /start
    StartGame: GameState -> unit
    /// POST /end
    EndGame: GameState -> unit
    /// POST /move
    Move: GameState -> MoveResponse
}