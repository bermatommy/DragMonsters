using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using TommyBermatovDragMonsters.Resources.ZClasses;
using TommyBermatovDragMonsters.ZClasses;

namespace TommyBermatovDragMonsters
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        public static bool isPlaying = false;
        public static Button LoginHP, RegisterHP, btnTutorial;
        private BatteryReciver broadcastBattery;
        private TextView tvBattery;
        private int level;
        private Android.App.AlertDialog tutorial;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitViews();
            InitObjects();
        }

        // Change battery precentage display
        public void OnBatteryChanged(int level)
        {
            this.level = level;
            this.tvBattery.Text = "Battery: " + level.ToString() + "%";
        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.first_menu, menu);
            return true;
        }


        // Handles the music 
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            Intent i = new Intent(this, typeof(MusicService));
            if (item.ItemId == Resource.Id.action_musicStart)
            {
                if (!isPlaying)
                {
                    StartService(i);
                    isPlaying = !isPlaying;
                    Toast.MakeText(this, "Started playing", ToastLength.Short).Show();

                }
                return true;
            }
            else if (item.ItemId == Resource.Id.action_musicStop)
            {
                if (isPlaying)
                {
                    StopService(i);
                    isPlaying = !isPlaying;
                    Toast.MakeText(this, "Stopped playing", ToastLength.Short).Show();
                }
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        // Init xml objects
        private void InitObjects()
        {
            LoginHP.Click += OnClick;
            RegisterHP.Click += OnClick;
            btnTutorial.Click += OnClick;

            var reciver = new BatteryReciver();
            RegisterReceiver(reciver, new IntentFilter(Intent.ActionBatteryChanged));
        }

        // What to do if each button is pressed
        private void OnClick(object sender, EventArgs e)
        {
            if (sender == LoginHP && LoginHP.Enabled == true)
            {
                LoginHP.Enabled = false;
                Intent i = new Intent(this, typeof(Login));

                StartActivity(i);
            }
            else if (sender == RegisterHP && RegisterHP.Enabled == true)
            {
                Intent i = new Intent(this, typeof(Register));
                StartActivity(i);
            }
            else if (sender == btnTutorial && btnTutorial.Enabled == true)
            {
                btnTutorial.Enabled = false;
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("How To Play");
                builder.SetMessage("1) Drag the target to the monster \n2) When you have enough gold, click the shop button. \n3) When you enter the shop you can buy upgardes:\n1. Damage - deal more damage to the monster \n2. More gold - get more gold from killing the monster \n3. Crit - double damage \nGood Luck!");
                builder.SetPositiveButton("Understood", OkAction);
                tutorial = builder.Create();
                btnTutorial.Enabled = true;
                tutorial.Show();
            }
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            btnTutorial.Enabled = true;
            return;
        }

        // Connect between every xml object to activity variable
        private void InitViews()
        {
            LoginHP = FindViewById<Button>(Resource.Id.btnLoginHP);
            RegisterHP = FindViewById<Button>(Resource.Id.btnRegisterHP);
            tvBattery = FindViewById<TextView>(Resource.Id.tvPrecentage);
            btnTutorial = FindViewById<Button>(Resource.Id.btnTutorial);
        }
    }
}