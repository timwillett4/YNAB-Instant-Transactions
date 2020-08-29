namespace YNABInstantTransactions.Android

// @TODO - 1)Use YNAB API To add transaction. (2) Add place to enter YNAB token (3) load categories from YNAB account

open System
open System.Diagnostics

open Android
open Android.App
open Android.Runtime
open Android.Support.V7.Widget
open Android.Support.V4.Content
open Android.Content
open Android.Support.V7.App
open YNABInstantTransactions.Presentation.ViewModels
open YNABInstantTransaction

[<Activity (Label = "YNABInstantTransactions.Android", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity () =
    inherit AppCompatActivity ()

    let REQUEST_SMS_PERMISIONS = 1

    let transactionReceiver = new TransactionReciever()

    member private this.TransactionView = this.FindViewById<RecyclerView>(ResourceProvider.Id.transactionsView)

    member private this.GetTransactionViewAdapter() =
        match this.TransactionView.GetAdapter() with
        | :? TransactionViewAdapter as adapter -> adapter
        | _ ->  Trace.Assert(false, "Unexpected cast failure", "Expected adapter to be of type 'TransactionViewAdapter'")
                this.TransactionView.GetAdapter() :?> TransactionViewAdapter // just to silence compier (@TODO is there a better way?)



    // @TODO remove code duplication between these two functions
    member private this.OnAddTransactionClicked (transactionID:Guid) = 
        let transaction = transactionID |> Application.Transaction.loadTransaction // @TODO these should be commands???
        // @TODO should add YNAB transaction and if succesful remoe 
        match transaction |> Application.Transaction.addTransaction with
        | Ok _ -> transactionID |> Application.Transaction.removeTransaction
        | Error -> transactionID |> this.GetTransactionViewAdapter().RemoveTransaction |> this.TransactionView.SetAdapter

    member private this.AddTransaction (transactionID:Guid) =
        transactionID 
        |> Application.Transaction.loadTransaction // @TODO these should be commands???
        |> this.GetTransactionViewAdapter().AddTransaction 
        |> this.TransactionView.SetAdapter

    [<Android.Runtime.Register("onCreate", "(Landroid/os/Bundle;)V", "GetOnCreate_Landroid_os_Bundle_Handler")>]
    override this.OnCreate (savedInstanceState) =

        let initializeActivity(savedInstanceState) =
            Xamarin.Essentials.Platform.Init(this, savedInstanceState)
            this.SetContentView(ResourceProvider.Layout.activity_main)

        let InitializeTransactionView() =

            let transactionViewAdapter = 
                let categories = [ "Category1"; "Category2";"Category3" ] // @TODO this should be loaded from budget settings
                let transactionVMs = Application.Transaction.loadAllTransaction() |> List.map createVM
                let vm = {Transactions = transactionVMs; Categories = categories }
                new TransactionViewAdapter(vm, this.OnAddTransactionClicked)

            new LinearLayoutManager(this) |> this.TransactionView.SetLayoutManager
            transactionViewAdapter |> this.TransactionView.SetAdapter

        initializeActivity(savedInstanceState)
        InitializeTransactionView()
        this.RequestPermissions([| Manifest.Permission.ReceiveSms; Manifest.Permission.ReadSms |], REQUEST_SMS_PERMISIONS)
        this.AddTransaction |> transactionReceiver.OnTransactionReceieved.Add
        base.OnCreate(savedInstanceState)

    [<Register("onResume", "()V", "GetOnResumeHandler")>]
    override this.OnResume() =
         LocalBroadcastManager.GetInstance(this).RegisterReceiver(transactionReceiver, new IntentFilter("TRANSACTION_RECIEVED"));
         base.OnResume()

    [<Register("onPause", "()V", "GetOnPauseHandler")>]
    override this.OnPause() =
        LocalBroadcastManager.GetInstance(this).UnregisterReceiver(transactionReceiver);
        base.OnPause()

    override this.OnCreateOptionsMenu(menu) =
        this.MenuInflater.Inflate(ResourceProvider.Menu.menu_main, menu)
        base.OnCreateOptionsMenu(menu)

    override this.OnOptionsItemSelected(item) =
        match item.ItemId with
        | id when id = ResourceProvider.Id.action_settings -> true
        | _ -> base.OnOptionsItemSelected(item);

    override this.OnRequestPermissionsResult(requestCode, permissions, grantResults) =
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults)

