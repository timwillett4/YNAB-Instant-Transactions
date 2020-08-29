module YNABInstantTransactions.Presentation.ViewModels

open System

open YNABInstantTransactions

// @TODO need to have a category entity type
type CategoryVM = {
    ID : Guid
    Name : string
}

type TransactionVM = {
    ID : Guid    
    Details : string
}

type TransactionListVM = {
   Transactions : TransactionVM list
   Categories : string list
}

type Commands = 
    | AddTransaction of Transaction
    | AddTransactions of Transaction list
    | RemoveTransaction of ID:Guid
    | ClearTransactions 
    | AddCategory of CategoryVM
    | RemoeCategory of Guid

    // @TODO Categories

let private createVM (transaction:Transaction) = 
    { ID = transaction.ID
      Details = sprintf "Charge for $%.2f at %s on %s from account %s" 
                    transaction.Amount 
                    transaction.Payee 
                    (transaction.Date.ToShortDateString()) 
                    transaction.Account }

let update command viewModel =
    match command with
    | AddTransaction transaction ->
        {viewModel with Transactions = (transaction |> createVM) :: viewModel.Transactions}
    | RemoveTransaction ID ->
        {viewModel with Transactions = viewModel.Transactions |> List.filter (fun transaction -> transaction.ID <> ID)}

