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
using Firebase;
using Firebase.Auth;

namespace TommyBermatovDragMonsters.ZFirebase
{
    public class FB_Data
    {
        private readonly FirebaseApp app;
        private readonly FirebaseAuth auth;

        public FB_Data()
        {
            app = FirebaseApp.InitializeApp(Application.Context);
            if (app is null)
            {
                FirebaseOptions options = GetMyOptions();
                app = FirebaseApp.InitializeApp(Application.Context, options);
            }
            auth = FirebaseAuth.Instance;
        }

        private FirebaseOptions GetMyOptions()
        {
            return new FirebaseOptions.Builder()
                .SetProjectId("dragmonsters-d92de")
                .SetApplicationId("1:111732653428:android:3b83c2531d22078d5cc021")
                .SetApiKey("AIzaSyALGCS1kcQ-vOCZKIRB1nezOA5J5wZE_tc")
                .SetStorageBucket("dragmonsters-d92de.appspot.com")
                .Build();
        }

        // Send a request to create a user
        public Android.Gms.Tasks.Task CreateUser(string email, string password)
        {
            return auth.CreateUserWithEmailAndPassword(email, password);
        }

        // Send a request to sign in
        public Android.Gms.Tasks.Task SignIn(string email, string password)
        {
            return auth.SignInWithEmailAndPassword(email, password);
        }

        // Send a request to reset password
        public Android.Gms.Tasks.Task ResetPassword(string email)
        {
            return auth.SendPasswordResetEmail(email);
        }
    }
}