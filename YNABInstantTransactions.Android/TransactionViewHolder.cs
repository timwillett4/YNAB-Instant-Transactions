using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace YNABInstantTransactions.Android
{
    public class TransactionViewHolder : RecyclerView.ViewHolder
    {
        public TextView Details { get; }
        public Spinner CategorySpinner { get; }
        public Button AddToYNABBudgetButton { get; }
        public TransactionViewHolder(View transactionView)
            : base(transactionView)
        {
            Details = ItemView.FindViewById<TextView>(Resource.Id.transaction_detail_textview);
            CategorySpinner = ItemView.FindViewById<Spinner>(Resource.Id.category_spinner);
            AddToYNABBudgetButton = ItemView.FindViewById<Button>(Resource.Id.add_to_YNAB_budget_button);
        }
    }
}