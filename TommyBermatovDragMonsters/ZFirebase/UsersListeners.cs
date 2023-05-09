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

namespace TommyBermatovDragMonsters.ZFirebase
{
    public class UsersListeners : Java.Lang.Object, IValueEventListener
    {
        private List<User> users = new List<User>();
        private FirebaseHelper firebaseHelper = new FirebaseHelper();
        public class UserDataEventArgs : EventArgs
        {
            public List<User> allUsers { get; set; }
        }

        public event EventHandler<UserDataEventArgs> UserRetrived;

        public void Create()
        {
            DatabaseReference databaseReference = firebaseHelper.GetDataBase().GetReference("Users");
            databaseReference.AddValueEventListener(this);
        }

        public void OnCancelled(DatabaseError error)
        {

        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                foreach (DataSnapshot userData  in child)
                {
                    User user = new User();
                    user.Mail = userData.Child("Email").Value.ToString();
                    user.Pwd = userData.Child("Password").Value.ToString();
                    users.Add(user);
                }
                UserRetrived.Invoke(this, new UserDataEventArgs { allUsers = users });
            }
        }
    }
}