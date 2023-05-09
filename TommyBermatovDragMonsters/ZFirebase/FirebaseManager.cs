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
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using TommyBermatovDragMonsters.Resources.ZClasses;

namespace TommyBermatovDragMonsters.ZFirebase
{
    public class FirebaseManager
    {
        FirebaseClient firebase = new FirebaseClient("https://dragmonsters-d92de-default-rtdb.europe-west1.firebasedatabase.app/");

        //add data to firebase
        //will add the new object if not exist
        //if object exists, will override it
        //async - אסינכרוני

        // update player values

        public async Task UpdatePlayer(Player player, string userId)
        {
            await firebase.Child("users").Child(userId).Child("Player").PutAsync<Player>(player);
        }

        // add user to fb
        public async Task AddUser(User user)
        {

            await firebase.Child("users").Child(user.Id).PutAsync<User>(user);
        }

        //get a single object
        public async Task<User> GetUser(string id)
        {
            return await firebase.Child("users").Child(id).OnceSingleAsync<User>();
        }

        //get a list of all the objects in the firebase
        public async Task<List<User>> GetAllUser()
        {
            return (await firebase.Child("users").OnceAsync<User>()).Select(item => new User(
                item.Object.Id,
                item.Object.Player)
            ).ToList();
        }

        //delete a user by its title
        public async Task DeleteUser(string id)
        {
            await firebase.Child("users").Child(id).DeleteAsync();
        }
    }
}