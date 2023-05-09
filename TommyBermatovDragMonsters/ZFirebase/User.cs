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
using Newtonsoft.Json;
using TommyBermatovDragMonsters.Resources.ZClasses;

namespace TommyBermatovDragMonsters.ZFirebase
{
    public class User
    {
        private string mail, pwd;
        private bool exist;
        private readonly SP_Data spd;
        private Player player;
        private string id;


        public string Mail { get => mail; set => mail = value; }
        public string Pwd { get => pwd; set => pwd = value; }
        public bool Exist { get => exist; set => exist = value; }
        public Player Player { get => player; set => player = value; }
        public string Id { get => id; set => id = value; }

        public User(Context ctx)
        {
            spd = new SP_Data(ctx);
            player = new Player();
            this.exist = this.Mail != String.Empty;
            if (this.exist)
            {
                this.Mail = spd.GetStringValue(General.KEY_MAIL);
                this.Pwd = spd.GetStringValue(General.KEY_PWD);
            }
        }
        [JsonConstructor]
        public User(string id, Player player)
        {
            this.id = id;
            this.player = player;
        }

        public User(string mail, string pwd)
        {
            this.mail = mail;
            this.pwd = pwd;
        }

        // save the user data
        public bool Save()
        {
            bool success = spd.PutStringValue(General.KEY_PWD, this.Pwd);
            return success && spd.PutStringValue(General.KEY_MAIL, this.Mail);
        }
    }
}