using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TommyBermatovDragMonsters.ZFirebase;

namespace TommyBermatovDragMonsters
{
    [Activity(Label = "Login", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity, IOnCompleteListener
    {
        private EditText email, password;
        public static Button LoginLogin, Cancel, forgotPassword;
        private Task tskLogin, tskReset;
        private FB_Data fbd;
        private FirebaseManager firebase;
        public static User user;
        private ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            InitViews();
            InitObjects();
            email.Text = Intent.GetStringExtra(General.KEY_MAIL);
            password.Text = Intent.GetStringExtra(General.KEY_PWD);
            MainActivity.LoginHP.Enabled = true;
            MainActivity.RegisterHP.Enabled = true;
        }

        // Init xml objects
        private void InitObjects()
        {
            LoginLogin.Click += OnClick;
            Cancel.Click += OnClick;
        }

        // What to do if each button is pressed
        private void OnClick(object sender, EventArgs e)
        {
            if (sender == LoginLogin && LoginLogin.Enabled == true)
            {
                LoginLogin.Enabled = false;
                loginUser();
            }
            else if (sender == Cancel)
            {
                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);
            }
            else if (sender == forgotPassword && forgotPassword.Enabled == true)
            {
                forgotPassword.Enabled = false;
                ResetPassword();
            }
        }

        // Connect between every xml object to activity variable
        private void InitViews()
        {
            email = FindViewById<EditText>(Resource.Id.etEmailLogin);
            password = FindViewById<EditText>(Resource.Id.etPasswordLogin);

            LoginLogin = FindViewById<Button>(Resource.Id.btnLoginLogin);
            Cancel = FindViewById<Button>(Resource.Id.btnCancelLogin);
            forgotPassword = FindViewById<Button>(Resource.Id.btnForgotPassword);
            firebase = new FirebaseManager();

            user = new User(this);
            fbd = new FB_Data();

            if (user.Exist)
                ShowUserData();
            else
                OpenRegisterActivity();
        }


        //Register a user
        private void OpenRegisterActivity()
        {
            Intent intent = new Intent(this, typeof(Register)); 
            StartActivityForResult(intent, General.REQUEST_REGISTER);
        }

        // Login the user
        private void loginUser()
        {
            if (password.Text != string.Empty && email.Text != string.Empty)
            {
                tskLogin = fbd.SignIn(email.Text, password.Text);
                tskLogin.AddOnCompleteListener(this);
                user.Pwd = password.Text;
                user.Mail = email.Text;
                user.Id = Convert.ToBase64String(Encoding.ASCII.GetBytes(email.Text));

                Toast.MakeText(this, "Please wait", ToastLength.Long).Show();
                progressDialog = new ProgressDialog(this);
                progressDialog.SetMessage("Loading...");
                progressDialog.Show();

                if (!user.Save())
                {
                    Toast.MakeText(this, "ERROR", ToastLength.Long).Show();
                }
            }
            else
            {
                if (password.Text == null || password.Text == string.Empty)
                Toast.MakeText(this, "Please enter password", ToastLength.Long).Show();
            else if (email.Text == null || email.Text == string.Empty)
                Toast.MakeText(this, "Please enter email", ToastLength.Long).Show();
            else if ((email.Text == null || email.Text == string.Empty) && (password.Text == null || password.Text == string.Empty))
                Toast.MakeText(this, "Please enter Values", ToastLength.Long).Show();
            }
            LoginLogin.Enabled = true;
        }

        // Writes the user data
        private void ShowUserData()
        {
            email.Text = user.Mail;
            password.Text = user.Pwd;
        }

        // Send a request to reset password
        private void ResetPassword()
        {
            try
            {
                tskReset = fbd.ResetPassword(user.Mail);
                tskReset.AddOnCompleteListener(this);
                Toast.MakeText(this, "Request sent", ToastLength.Long).Show();
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Please register before you login", ToastLength.Long).Show();
            }
            
        }

        // What to do when task is completed
        public void OnComplete(Task task)
        {
            string msg = string.Empty;
            if (task.IsSuccessful)
            {
                if (task == tskLogin)
                    StartGame();
                else if (task == tskReset)
                    msg = "Reseted!";
            }
            else
            {
                msg = task.Exception.Message;
                progressDialog.Dismiss();
                Toast.MakeText(this, "Cant login!", ToastLength.Short).Show();
            }
        }

        // Start the game
        private async void StartGame()
        {
            user = await firebase.GetUser(user.Id);
            progressDialog.Dismiss();
            Intent intent = new Intent(this, typeof(Ready));
            StartActivity(intent);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == General.REQUEST_REGISTER)
                if (resultCode == Result.Ok)
                {
                    user.Mail = data.GetStringExtra(General.KEY_MAIL);
                    user.Pwd = data.GetStringExtra(General.KEY_PWD);
                    ShowUserData();
                    if (!user.Save())
                        Toast.MakeText(this, "ERROR", ToastLength.Long).Show();

                }
        }
    }
}