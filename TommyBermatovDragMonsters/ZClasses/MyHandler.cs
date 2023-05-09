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
    public class MyHandler : Handler
    {
        private Context context;

        public MyHandler(Context context)
        {
            this.context = context;
        }
        /// <summary>
        /// Handle the messages
        /// </summary>
        public override void HandleMessage(Message msg)
        {
            if (msg.Arg1 == 111)
            {
                ((Boss)context).ChangeMonsterHP(msg.Arg2.ToString());
            }
            else if (msg.Arg1 == 222)
            {
                ((Boss)context).AddGoldFromBoss(msg.Arg2);
            }
            base.HandleMessage(msg);
        }
    }
}