open Hopac
open Logary.Configuration
open Logary.Adapters.Facade
open Logary.Targets
open Expecto

[<EntryPoint>]
let main argv =
  let logary =
    Config.create "YNABInstantTransactions.Tests" "localhost"
    |> Config.targets [ LiterateConsole.create LiterateConsole.empty "console" ]
    |> Config.processing (Events.events |> Events.sink ["console";])
    |> Config.build
    |> run
  LogaryFacadeAdapter.initialise<Expecto.Logging.Logger> logary

  // Invoke Expecto:
  runTestsInAssemblyWithCLIArgs [] argv
