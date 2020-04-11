module YNABInstantTransactions.Presentation.ViewModels

type TransactionVM = {
    Details : string
}

type TransactionListVM = {
   Transactions : TransactionVM seq
   Categories : string seq
}
