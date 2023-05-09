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

namespace TommyBermatovDragMonsters.ZFirebase
{
    public class SP_Data
    {
        private readonly ISharedPreferences sp;

        public SP_Data(Context ctx)
        {
            sp = ctx.GetSharedPreferences(General.SP_FILE_NAME, FileCreationMode.Private);
        }

        // get a value
        public string GetStringValue(string key)
        {
            return sp.GetString(key, string.Empty);
        }

        // save a value
        public bool PutStringValue(string key, string value)
        {
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutString(key, value);
            return editor.Commit();
        }
    }
}