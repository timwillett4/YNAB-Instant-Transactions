module YNABInstantTransactions.UseCases.ProcessSMS

open System
open FSharp.Core.Extensions.ActivePattern
open YNABInstantTransactions

let private parseSMS (smsText:string) =
    let MFCU = "Charge for \$([0-9]+\.[0-9]{2}) on ([0-9]{2}/[0-9]{2}) ([0-9]{2}:[0-9]{2}) ([A-Z]{3}) at (.*), (.*), (.*) on card ending in ([0-9]{4})"

    match smsText with
    | Regex MFCU [amount; date; time; timezone; payee; city; state; account] ->
        let isValidDate, date = date |> DateTime.TryParse
        let isValidAmount, amount = amount |> Decimal.TryParse
        match isValidDate, isValidAmount with 
        | true, true -> Some {ID=Guid.NewGuid(); Account=account; Date=date; Payee=payee; Amount=amount }
        | _ -> None
    | _ -> 
        None


let execute saveTransaction broadcastNewTransaction message =
    match message |> parseSMS with
    | Some transaction ->
        do transaction |> saveTransaction  
        do transaction.ID |> broadcastNewTransaction
    | None -> ()
