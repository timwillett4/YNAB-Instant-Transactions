namespace YNABInstantTransactions.Android

open System

open YNABInstantTransaction.Application.Transaction
open YNABInstantTransactions.Presentation.ViewModels

open Android.Views
open Android.Support.V7.Widget
open Android.Widget

type ResourceProvider = Resource

// Cmd/Querry Seperation
// These should be in app have set of querries and commands
// commands take some kind of state and update it
// querries just return a state
type TransactionViewCmd =
    | CmdAddTransaction
    | CmdRemoveTransaction
    | CmdAddCategory
    | CmdRemoveCategory

type TransactionViewHolder(transactionView : View) =
    inherit RecyclerView.ViewHolder(transactionView)

    member this.Details with get () =
        this.ItemView.FindViewById<TextView>(ResourceProvider.Id.transaction_detail_textview)

    member this.CategorySpinner with get () =
        this.ItemView.FindViewById<Spinner>(ResourceProvider.Id.category_spinner)

    member this.AddToYNABBudgetButton with get () =
        this.ItemView.FindViewById<Button>(ResourceProvider.Id.add_to_YNAB_budget_button)


type TransactionViewAdapter(viewModel, onAddTransactionClicked:Guid->unit) =
    inherit RecyclerView.Adapter()

    override this.OnCreateViewHolder(parent: ViewGroup, position: int) =
        new TransactionViewHolder(LayoutInflater.From(parent.Context).Inflate(ResourceProvider.Layout.transaction_view, parent, false))
        :> RecyclerView.ViewHolder

    override this.OnBindViewHolder(holder : RecyclerView.ViewHolder, position : int) =
       match holder with
       | :? TransactionViewHolder as vh ->
            let transaction = (viewModel.Transactions |> List.item position)
            do vh.Details.Text <- transaction.Details
            do vh.CategorySpinner.Adapter <- new ArrayAdapter<string>(
                                                    vh.CategorySpinner.Context, 
                                                    ResourceProvider.Layout.support_simple_spinner_dropdown_item, 
                                                    viewModel.Categories |> Seq.toArray)
            do vh.AddToYNABBudgetButton.Click.Add(fun _ -> transaction.ID |> onAddTransactionClicked)
       | _ -> ()

    override this.ItemCount = viewModel.Transactions |> List.length

    // @TODO - this logic should be testable
    member this.AddTransaction(transaction) = 
       let transactions =  (transaction |> createVM) :: viewModel.Transactions
       new TransactionViewAdapter({ viewModel with Transactions = transactions }, onAddTransactionClicked) 
    
    member this.RemoveTransaction(ID: Guid) = 
       let transactions = viewModel.Transactions |> List.filter (fun vm -> vm.ID <> ID)
       new TransactionViewAdapter({ viewModel with Transactions = transactions }, onAddTransactionClicked) 
