open System
open SwaggerProvider
open System.Net

let [<Literal>] schema = "http://swagger.local/swagger/docs/v1"
type SportsServiceProvider = SwaggerProvider<schema>

[<EntryPoint>]
let main argv =

    let sportsService = SportsServiceProvider()
    
    // Get all leagues, teams, and players
    printfn "All Leagues:\n%A\n" <| sportsService.LeaguesGet()
    printfn "All Teams:\n%A\n" <| sportsService.TeamsGet(None)
    printfn "All Players:\n%A\n" <| sportsService.PlayersGet(None, null, null, null)
    
    // Get a specific team and player
    printfn "Steelers:\n%A\n" <| sportsService.TeamsGet1(1)
    printfn "Hines Ward:\n%A\n" <| sportsService.PlayersGet(Some(1), null, "Ward", null)

    // Create a player
    let manning = SportsServiceProvider.Player()
    manning.TeamId <- Some(2)
    manning.FirstName <- "Peyton"
    manning.LastName <- "Manning"
    manning.Position <- "Quarterback"
    manning.IsCaptain <- Some(true)
    let p = sportsService.PlayersPut(manning)
    printfn "Peyton Manning:\n%A\n" p
    
    // Delete that player
    let manningId = p.Id
    let d = sportsService.PlayersDelete(manningId.Value)
    printfn "Player Delete:\n%A\n" d
    
    // Try to get a team that doesn't exist
    try
        sportsService.TeamsGet1(999) |> ignore
    with
        | :? WebException as ex -> printfn "Caught Exception:\n%A\n" ex

    Console.ReadKey() |> ignore
    0 // return an integer exit code
