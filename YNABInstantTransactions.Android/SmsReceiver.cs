using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
//using static YNABInstantTransaction.Domain.DomainTypes;
//using static YNABInstantTransaction.Domain.API;

namespace YNABInstantTransactions.Android
{
    [BroadcastReceiver(Name = "com.willettEnterprise.YNABInstantTransaction.SmsReceiver", Enabled = true, Exported = true)]
    [IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class SmsReceiver : BroadcastReceiver
    {
    //    private readonly API API = createAPI();
        public override void OnReceive(Context context, Intent intent)
        {
            System.Console.WriteLine("Got a text message!");

            foreach (var sms in Telephony.Sms.Intents.GetMessagesFromIntent(intent))
            {
                System.Console.WriteLine("Got message: " + sms.MessageBody + "; From: " + sms.OriginatingAddress);
                //          var result = API.ProcessSMS(sms.OriginatingAddress, sms.DisplayMessageBody);
//
 //               if (Microsoft.FSharp.Core.FSharpOption<Transaction>.get_IsSome(result))
  //              {
   //                 // @TODO - save to a file and send broadcast to main activity
    //                Toast.MakeText(context, "Got a transaction!", ToastLength.Short);
               // }
            }
        }
    }
}