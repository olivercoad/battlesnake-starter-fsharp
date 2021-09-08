open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Battlesnake

let webapp : HttpHandler =
    choose [ route "/" >=> json (info ())
             route "/start" >=> bindJson (fun gs -> startGame gs; text "Game started")
             route "/end" >=> bindJson (fun gs -> endGame gs; text "Game ended")
             route "/move" >=> bindJson (move >> json)
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
