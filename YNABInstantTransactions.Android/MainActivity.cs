using System.Collections.Generic;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Content.PM;
using static YNABInstantTransactions.Presentation.ViewModels;

namespace YNABInstantTransactions.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const int REQUEST_SMS_PERMISIONS = 1;

        private RecyclerView.LayoutManager LayoutManager;
        private RecyclerView TransactionView;
        private TransactionViewAdapter TransactionViewAdapter;

        private List<TransactionVM> Transactions;
        private List<string> Categories;
        private TransactionListVM TransactionListVM;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Transactions = new List<TransactionVM>();
            Categories = new List<string>();
            TransactionListVM = new TransactionListVM(Transactions, Categories);

            InitializeActivity(savedInstanceState);
            InitializeToolbar();
            InitializeTransactionView();
            RequestPermissions(new[] { Manifest.Permission.ReceiveSms, Manifest.Permission.ReadSms }, REQUEST_SMS_PERMISIONS);

            void InitializeActivity(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.activity_main);
                LayoutManager = new LinearLayoutManager(this);
            }

            void InitializeToolbar()
            {
                var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
            }

            void InitializeTransactionView()
            {
                Transactions.Add(new TransactionVM("First Transaction"));
                Categories.AddRange(new[] { "Category1", "Category2", "Category3" });

                TransactionView = FindViewById<RecyclerView>(Resource.Id.transactionsView);
                TransactionView.SetLayoutManager(LayoutManager);

                TransactionViewAdapter = new TransactionViewAdapter(TransactionListVM);
                TransactionView.SetAdapter(TransactionViewAdapter);
            }
        }

        [Register("onResume", "()V", "GetOnResumeHandler")]
        protected override void OnResume()
        {

        }

        [Register("onPause", "()V", "GetOnPauseHandler")]
        protected override void OnPause()
        {

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}

