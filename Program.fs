open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe

let webapp : HttpHandler =
    let s = Battlesnake.server
    choose [ route "/" >=> json (s.Info ()) // evaluated once on startup. use Giraffe warbler if not static
             route "/start" >=> bindJson (fun gameState -> s.StartGame gameState; text "Game started")
             route "/end" >=> bindJson (fun gameState -> s.EndGame gameState; text "Game ended")
             route "/move" >=> bindJson (s.Move >> json) // parse gameState from json and encode response as json
    ]

let configureApp (app: IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe webapp

let configureServices (services: IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe() |> ignore

    // Use Thoth for the json (de)serialization
    let thothSerializer = 
        Thoth.Json.Giraffe.ThothSerializer(Thoth.Json.Net.CaseStrategy.CamelCase)
    // would've been better if it was CaseStrategy.SnakeCase...
    services.AddSingleton<Json.ISerializer>(thothSerializer) |> ignore

[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .UseUrls("http://0.0.0.0:5009")
                .Configure(configureApp)
                .ConfigureServices(configureServices)
            |> ignore)
        .Build()
        .Run()

    0
