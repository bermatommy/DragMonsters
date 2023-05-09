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
using Firebase.Database;
using Java.Util;

namespace TommyBermatovDragMonsters.ZFirebase
{

    public class FirebaseHelper
    {
        private UsersListeners usersListeners;
        private List<User> users;
        private ProgressBar progressBar;

        public FirebaseHelper(ProgressBar progressBar)
        {
            this.progressBar = progressBar;
            this.progressBar.Visibility = Android.Views.ViewStates.Invisible;
        }

        public FirebaseHelper()
        {

        }


        public FirebaseDatabase GetDataBase()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseDatabase database;
            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                .SetApplicationId("1:111732653428:android:3b83c2531d22078d5cc021")
                .SetApiKey("AIzaSyALGCS1kcQ-vOCZKIRB1nezOA5J5wZE_tc")
                .SetDatabaseUrl("https://dragmonsters-d92de-default-rtdb.europe-west1.firebasedatabase.app/")
                .SetStorageBucket("dragmonsters-d92de.appspot.com")
                .Build();
                app = FirebaseApp.InitializeApp(Application.Context, option);
            }
            database = FirebaseDatabase.GetInstance(app);
            return database;
        }

        public void RetriveData()
        {
            usersListeners = new UsersListeners();
            usersListeners.Create();
            usersListeners.UserRetrived += UsersListeners_UsersRetrived;
        }
        private void UsersListeners_UsersRetrived(object sender, UsersListeners.UserDataEventArgs e)
        {
            users = e.allUsers;
        }

        public void AddUser(DatabaseReference reference, string email, string password)
        {
            HashMap newUser = new HashMap();
            newUser.Put("email", email);
            newUser.Put("password", password);
            reference.SetValue(newUser);
        }


    }
}