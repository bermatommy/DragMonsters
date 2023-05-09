using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TommyBermatovDragMonsters.ZClasses
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBatteryChanged})]
    public class BatteryReciver : BroadcastReceiver
    {
        private int batteryLevel;

        //Display the battery precentage on homepage
        public override void OnReceive(Context context, Intent intent)
        {
            batteryLevel = intent.GetIntExtra(BatteryManager.ExtraLevel, -1);

            if (batteryLevel <= 10)
            {
                Toast.MakeText(context, "Battery below 10, pls charge", ToastLength.Short).Show();
            }
            MainActivity mainActivity = (MainActivity)context;
            mainActivity.OnBatteryChanged(batteryLevel);

        }
    }
}