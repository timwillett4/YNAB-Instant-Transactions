using System;
using System.Linq;
using Android.Widget;
using Android.Views;
using Android.Support.V7.Widget;
using static YNABInstantTransactions.Presentation.ViewModels;

namespace YNABInstantTransactions.Android
{
    internal class TransactionViewAdapter : RecyclerView.Adapter
    {
        private readonly TransactionListVM ViewModel;

        public TransactionViewAdapter(TransactionListVM viewModel)
        {
            ViewModel = viewModel;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.transaction_view, parent, false);

            return new TransactionViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as TransactionViewHolder;

            // @TODO handle exeption
            vh.Details.Text = ViewModel.Transactions.ElementAt(position).Details;
            vh.CategorySpinner.Adapter = new ArrayAdapter<string>(vh.CategorySpinner.Context, Resource.Layout.support_simple_spinner_dropdown_item, ViewModel.Categories.ToArray());
        }

        public override int ItemCount => ViewModel.Transactions.Count();
    }
}