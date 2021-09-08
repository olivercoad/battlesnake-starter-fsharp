/// The object definitions for the Battlesnake webserver
/// https://docs.battlesnake.com/references/api#object-definitions
module BattlesnakeAPI

// fsharplint:disable RECORDFIELDNAMES

/// Response to requests to /
type BattlesnakeInfo = {
    apiversion: string // version of the battlesnake API implemented
    author: string option // username of the author of this battlesnake
    color: string option // hex color code to display this battlesnake
    head: string option // displayed head of this battlesnake
    tail: string option // displayed tail of this battlesnake
    version: string option // a version number of tag for your snake
}

type Game = {
    id: string // a unique identifier for this game
    ruleset: Ruleset // information about the ruleset being used to run this game
    timeout: int // (milliseconds) how much time your snake has to respond to requests
}
and Ruleset = {
    name: string // name of the ruleset this game
    version: string // the release version of the rules module
    settings: RulesetSettings
}
and RulesetSettings = {
    foodSpawnChance: int // percent change of spawning a new food every round
    minimumFood: int // minimum food to keep on the board every turn
    hazardDamagePerTurn: int // extra health damage taken when ending turn in hazard
    royale: Royale option
    squad: Squad option
}
and Royale = {
    shrinkEveryNTurns: int // number of turns between generating new hazards
}
and Squad = {
    allowBodyCollisions: bool // allow members of same squad to move over each other
    sharedElimination: bool // all squad members eliminated when one is eliminated
    sharedHealth: bool // all squad members share health
    sharedLength: bool // all squad members share length
}

type Point = {
    x: int
    y: int
}

type Battlesnake = {
    id: string // unique identifier for this battlesnake in the context of the current game
    name: string // name given by its author
    health: int // health balue between 0-100 inclusive
    body: Point array // array of coordinates representing location on the game board from head to tail
    latency: string // the previous response time in milliseconds (0 means timed out or failed)
    head: Point // coordinates of the head
    length: int // length from head to tail = body.Length
    shout: string // message shouted by this battlesnake in previous turn
    squad: string // the squad that the battlesnake belongs to
}

type Board = {
    // (0,0) is at bottom left
    height: int // number of rows in y-axis
    width: int // number of columns in x-axis
    food: Point array // coordinates representing for locations on the game board
    hazards: Point array // coordinates representing hazardous locations
    snakes: Battlesnake array // all remaining battlesnakes (including self)
}

/// Body of requests made to /start /end or /move
type GameState = {
    game: Game
    turn: int
    board: Board
    you: Battlesnake
}

/// Response to requests to /move
type MoveResponse = {
    move: string // your move for this turn
    shout: string option // message to all other battlesnakes 256 char or less
}