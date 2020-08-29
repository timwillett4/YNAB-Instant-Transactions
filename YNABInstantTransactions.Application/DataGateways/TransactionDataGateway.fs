module YNABInstantTransactions.DataGateways.TransactionDataGateway

open YNABInstantTransactions

open System
open System.IO
open FSharp.Json

// seperate implemenation from interface
// should this be an interface than have file for AndroidTransactionGateway??

let private getFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)

let private getFilePath transactionID = 
    Path.Combine(getFolder, transactionID.ToString() + ".txt")

// @TODO exception handling
let saveTransaction (transaction:Transaction) =
    use writer = transaction.ID |> getFilePath |> File.CreateText
    transaction |> Json.serialize |> writer.WriteLine 

// @TODO - invoke YNAB API ADD Transaction Call
let addTransaction (transaction:Transaction) =
    Ok ()

let removeTransaction (ID:Guid) =
    ID |> getFilePath |> File.Delete 

let loadTransaction (ID:Guid) =
    ID |> getFilePath |> File.ReadAllText |> Json.deserialize<Transaction>

let toTransactionID (filename:string) =
    try
        Some (filename |> Guid.Parse)
    with _ -> None

let loadAllTransaction() =
    Directory.GetFiles(getFolder)
    |> Array.toList
    |> List.map (Path.GetFileNameWithoutExtension >> toTransactionID)
    |> List.choose id
    |> List.map loadTransaction
