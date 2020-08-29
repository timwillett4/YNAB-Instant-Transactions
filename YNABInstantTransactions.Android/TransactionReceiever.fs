namespace YNABInstantTransactions.Android

open System

open Android.App
open Android.Content

[<BroadcastReceiver(Name = "com.willettEnterprise.YNABInstantTransaction.TransactionReciever", Enabled = true, Exported = true)>]
[<IntentFilter([|"TRANSACTION_RECIEVED"|])>]
type TransactionReciever () =
    inherit BroadcastReceiver()

    let onTransactionRecieved = Event<_>()

    override this.OnReceive (context, intent) =
        intent.GetStringExtra("TransactionID") |> Guid.Parse |> onTransactionRecieved.Trigger

    [<CLIEvent>]
    member this.OnTransactionReceieved = onTransactionRecieved.Publish
