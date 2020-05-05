module YNABInstantTransaction.Application.Reciever

open FSharp.Core.Extensions.ActivePattern

open System

type Transaction = {
    Account : string
    Date : DateTime // @TODO - use YODA time package to deal with timezones
    Payee : string
    Amount : decimal
}

let parseSMS (sms:string) =
    let MFCU = "Charge for \$([0-9]+\.[0-9]{2}) on ([0-9]{2}/[0-9]{2}) ([0-9]{2}:[0-9]{2}) ([A-Z]{3}) at (.*), (.*), (.*) on card ending in ([0-9]{4})"
    match sms with
    | Regex MFCU [amount; date; time; timezone; payee; city; state; account] ->
        let isValidDate, date = date |> DateTime.TryParse
        let isValidAmount, amount = amount |> Decimal.TryParse
        match isValidDate, isValidAmount with 
        | true, true -> Some {Account=account; Date=date; Payee=payee; Amount=amount }
        | _ -> None
    | _ -> 
        None
