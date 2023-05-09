using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TommyBermatovDragMonsters.Resources.ZClasses;
using TommyBermatovDragMonsters.ZClasses;
using TommyBermatovDragMonsters.ZFirebase;

namespace TommyBermatovDragMonsters
{
    [Activity(Label = "Boss", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Boss : Activity
    {
        public FrameLayout frm;
        public bool isPlaying;
        public static GameBoard gameBoard;
        public static Player player = new Player();
        public static Button ShopBoss, btnMusic;
        private TextView MonsterHP, CurrentGold;
        private MyHandler goldHandler;
        public static string userID = Login.user.Id;
        private int goldOwns, damage, critChance, moreGold;
        public static int health;
        public int firstMaxHealth = 1500;
        private static int monsterMaxHP = 0;
        private FirebaseManager fb = new FirebaseManager();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ConfigUser();
            SetContentView(Resource.Layout.boss);
            InitViews();
            InitObjects();
            Ready.btnStart.Enabled = true;
        }

        // update the user 
        private void ConfigUser()
        {
            player.GoldOwns = Login.user.Player.GoldOwns;
            player.Damage = Login.user.Player.Damage;
            player.MoreGold = Login.user.Player.MoreGold;
            player.CritChance = Login.user.Player.CritChance;
        }

        // Init xml objects
        private void InitObjects()
        {
            ShopBoss.Click += OnCLick;
            btnMusic.Click += OnCLick;
        }

        // What to do if each button is pressed
        private void OnCLick(object sender, EventArgs e)
        {
            if (sender == ShopBoss)
            {
                ShopBoss.Enabled = false;
                gameBoard.SetRun(false);
                frm.RemoveView(gameBoard);
                gameBoard = null;
                Intent i = new Intent(this, typeof(Shop));
                i.PutExtra("userID", userID);
                i.PutExtra("playerGold", player.GoldOwns);
                i.PutExtra("monsterMaxHP", monsterMaxHP);
                
                StartActivity(i);
            }
            else if (sender == btnMusic)
            {
                Intent i = new Intent(this, typeof(MusicService));
                if (isPlaying == true)
                {
                    StartService(i);
                    Toast.MakeText(this, "Started playing", ToastLength.Short).Show();
                }
                else
                {
                    StopService(i);
                    Toast.MakeText(this, "Stopped playing", ToastLength.Short).Show();
                }
                isPlaying = !isPlaying;
            }
        }

        // Change the Monster health value
        public void ChangeMonsterHP(string value)
        {
            if (int.Parse(value) > 0)
            {
                this.MonsterHP.Text = value;
            }
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                if (gameBoard == null)
                {
                    damage = Login.user.Player.Damage;
                    goldOwns = Login.user.Player.GoldOwns;
                    moreGold = Login.user.Player.MoreGold;
                    critChance = Login.user.Player.CritChance;
                    userID = Intent.GetStringExtra("userID");
                    if (monsterMaxHP == 0)
                    {
                        monsterMaxHP = firstMaxHealth;
                        health = monsterMaxHP;
                    }
                    gameBoard = new GameBoard(this, frm.Width, frm.Height, userID, this.damage, this.goldOwns, this.moreGold, this.critChance, goldHandler, health, monsterMaxHP);
                    frm.AddView(gameBoard);
                    gameBoard.SetRun(true);
                }
            }
        }


        // Add gold from boss
        public void AddGoldFromBoss(int bank)
        {
            player.GoldOwns += bank;
            CurrentGold.Text = player.GoldOwns.ToString();
        }
        
        // Connect between every xml object to activity variable
        private void InitViews()
        {
            frm = FindViewById<FrameLayout>(Resource.Id.frmGame);

            MonsterHP = FindViewById<TextView>(Resource.Id.tvMonsterHP);
            CurrentGold = FindViewById<TextView>(Resource.Id.tvGoldOwns);

            ShopBoss = FindViewById<Button>(Resource.Id.btnShopBoss);
            btnMusic = FindViewById<Button>(Resource.Id.btnMusic);

            isPlaying = MainActivity.isPlaying;

            this.goldHandler = new MyHandler(this);
        }
    }
}