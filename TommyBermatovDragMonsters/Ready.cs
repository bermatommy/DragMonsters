using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TommyBermatovDragMonsters
{
    [Activity(Label = "Ready", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Ready : Activity
    {
        public static Button btnStart;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ready);
            InitViews();
            InitObjects();
            Login.LoginLogin.Enabled = true;
            Login.forgotPassword.Enabled = true;
        }

        private void InitObjects()
        {
            btnStart.Click += OnClick;
        }

        private void InitViews()
        {
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
        }
        private void OnClick(object sender, EventArgs e)
        {
            if (sender == btnStart && btnStart.Enabled == true)
            {
                btnStart.Enabled = false;
                Intent i = new Intent(this, typeof(Boss));
                StartActivity(i);
            }
        }
    }
}