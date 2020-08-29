namespace YNABInstantTransactions.Android

open System

open Android
open Android.App
open Android.Support.V4.Content
open Android.Content
open Android.Provider

open YNABInstantTransactions
open YNABInstantTransactions.DataGateways

[<BroadcastReceiver(Name = "com.willettEnterprise.YNABInstantTransaction.SmsReceiver", Enabled = true, Exported = true)>]
[<IntentFilter([|"android.provider.Telephony.SMS_RECEIVED"|])>]
type SmsReceiver () =
    inherit BroadcastReceiver()

    override this.OnReceive (context, intent) =

        let broadcastTransaction (transactionID:Guid) =
            use smsIntent = new Intent("TRANSACTION_RECIEVED")
            let smsIntent = smsIntent.PutExtra("TransactionID", transactionID.ToString())
            LocalBroadcastManager.GetInstance(context).SendBroadcast(smsIntent) |> ignore

        let processSMS = UseCases.ProcessSMS.execute TransactionDataGateway.saveTransaction broadcastTransaction

        Telephony.Sms.Intents.GetMessagesFromIntent(intent) 
        |> Array.toSeq
        |> Seq.map (fun sms -> sms.DisplayMessageBody)
        |> Seq.iter processSMS
