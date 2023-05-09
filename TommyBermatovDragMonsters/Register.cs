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
using TommyBermatovDragMonsters.ZFirebase;
using Android.Content.PM;
using Android.Gms.Tasks;
using TommyBermatovDragMonsters.Resources.ZClasses;

namespace TommyBermatovDragMonsters
{
    [Activity(Label = "Register", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Register : Activity, IOnCompleteListener
    {
        private EditText password, email;
        public static Button btnRegister, btnCancel;
        private User user, userdisplay;
        private FB_Data fbd;
        private Player player;
        private FirebaseManager firebase;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register);
            InitViews();
            InitObjects();
            MainActivity.LoginHP.Enabled = true;
            MainActivity.RegisterHP.Enabled = true;
        }
        // Init xml objects
        private void InitObjects()
        {
            btnRegister.Click += OnClick;
            btnCancel.Click += OnClick;
        }

        // What to do if each button is pressed
        private void OnClick(object sender, EventArgs e)
        {
            if (sender == btnRegister)
            {
                btnRegister.Enabled = false;
                RegisterUser();
            }
            else if (sender == btnCancel)
            {
                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);
            }
        }

        // Register the user and check that all values are valid
        private void RegisterUser()
        {
            user = new User(Convert.ToBase64String(Encoding.ASCII.GetBytes(email.Text)), player);
            userdisplay = new User(email.Text, password.Text);
            btnRegister.Enabled = true;
            if (email.Text != String.Empty && password.Text != String.Empty && user.Mail != string.Empty && user.Pwd != string.Empty)
            {
                Toast.MakeText(this, "Please wait", ToastLength.Short).Show();
                fbd.CreateUser(email.Text, password.Text).AddOnCompleteListener(this);
            }
            else
            {
                Toast.MakeText(this, "Enter all values ", ToastLength.Short).Show();
            }
        }

        // Check that user registered successfully 
        public async void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                await firebase.AddUser(user);
                user.Exist = true;
                Toast.MakeText(this, "Registered Successfull", ToastLength.Short).Show();
                Intent i = new Intent(this, typeof(Login));
                i.PutExtra(General.KEY_MAIL, userdisplay.Mail);
                i.PutExtra(General.KEY_PWD, userdisplay.Pwd);
                StartActivity(i);
            }
            else
            {
                Toast.MakeText(this, task.Exception.Message, ToastLength.Short).Show();
            }
        }

        // Connect between every xml object to activity variable
        private void InitViews()
        {
            password = FindViewById<EditText>(Resource.Id.etPasswordRegister);
            email = FindViewById<EditText>(Resource.Id.etEmailRegister);

            btnRegister = FindViewById<Button>(Resource.Id.btnRegisterRegister);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancelRegister);

            firebase = new FirebaseManager();
            player = new Player();
            fbd = new FB_Data();
        }
    }
}