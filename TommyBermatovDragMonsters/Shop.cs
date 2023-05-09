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
using TommyBermatovDragMonsters.Resources.ZClasses;
using TommyBermatovDragMonsters.ZFirebase;

namespace TommyBermatovDragMonsters
{
    [Activity(Label = "Shop", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Shop : Activity
    {
        private TextView Gold, DamageDealShop, MoreGoldPercentageShop, CritPercentageShop;
        public static Button btnUpgradeDamageShop, btnUpgradeGoldShop, btnUpgradeCritShop, btnExitShop;
        public static int upgradeDamageCost = 20, upgradeGoldCost = 40, upgradeCritCost = 40;
        private int addDamageNum = 10, addGoldNum = 10, addCritNum = 10;
        private int monsterMaxHP;
        FirebaseManager firebaseManager = new FirebaseManager();
        Android.App.AlertDialog d;
        User user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.shop);
            InitViews();
            ShowPlayerValues();
            InitObjects();
            Boss.ShopBoss.Enabled = true;
        }


        // Display players gold
        public void PlayerGold()
        {
            Gold.Text = Boss.player.GoldOwns.ToString();
        }

        // Init xml objects
        private void InitObjects()
        {
            btnUpgradeDamageShop.Click += OnClick;
            btnUpgradeGoldShop.Click += OnClick;
            btnUpgradeCritShop.Click += OnClick;
            btnExitShop.Click += OnClick;
        }

        // Upgrade the players damage
        public void UpgradeDamage()
        {
            if (Boss.player.GoldOwns >= upgradeDamageCost && btnUpgradeDamageShop.Enabled == true)
            {
                btnUpgradeDamageShop.Enabled = false;
                Boss.player.GoldOwns -= upgradeDamageCost;
                Boss.player.Damage += addDamageNum;
                DamageDealShop.Text = Boss.player.Damage.ToString() + "(+" + addDamageNum + ")";
                upgradeDamageCost += monsterMaxHP / 100;
                btnUpgradeDamageShop.Text = "Cost: " + upgradeDamageCost.ToString();

                //await fb.UpdatePlayer(Boss.player, Boss.userID);
                Toast.MakeText(this, "+" + addDamageNum + " Damage", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Not Enought Gold", ToastLength.Short).Show();
            }
            Login.user.Player.Damage = Boss.player.Damage;
            btnUpgradeDamageShop.Enabled = true;
        }

        // Upgrade the players chance to get more gold
        public void UpgradeGold()
        {
            if (Boss.player.MoreGold == 100)
            {
                Toast.MakeText(this, "Maxed More Gold", ToastLength.Short).Show();
            }
            else if (Boss.player.GoldOwns >= upgradeGoldCost && Boss.player.MoreGold < 100 && btnUpgradeGoldShop.Enabled == true)
            {
                btnUpgradeGoldShop.Enabled = false;
                Boss.player.GoldOwns -= upgradeGoldCost;
                Boss.player.MoreGold += addGoldNum;
                MoreGoldPercentageShop.Text = Boss.player.MoreGold.ToString() + "%" + "(+" + addGoldNum + ")";
                upgradeGoldCost += monsterMaxHP / 100;
                btnUpgradeGoldShop.Text = "Cost: " + upgradeGoldCost.ToString();
                Toast.MakeText(this, "+" + addGoldNum + "% Gold Gained", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Not Enought Gold", ToastLength.Short).Show();
            }
            Login.user.Player.MoreGold = Boss.player.MoreGold;
            btnUpgradeGoldShop.Enabled = true;
        }

        // Upgrade the players crit chance
        public void UpgradeCrit()
        {
            if (Boss.player.CritChance == 100)
            {
                Toast.MakeText(this, "Maxed Crit Chance", ToastLength.Short).Show();
            }
            else if (Boss.player.GoldOwns >= upgradeCritCost && Boss.player.CritChance < 100 && btnUpgradeCritShop.Enabled == true)
            {
                btnUpgradeCritShop.Enabled = false;
                Boss.player.GoldOwns -= upgradeCritCost;
                Boss.player.CritChance += addCritNum;
                CritPercentageShop.Text = Boss.player.CritChance.ToString() + "%" + "(+" + addCritNum + ")";
                upgradeCritCost += monsterMaxHP / 100;
                btnUpgradeCritShop.Text = "Cost: " + upgradeCritCost.ToString();
                Toast.MakeText(this, "+" + addCritNum + "% Crit Chance", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Not Enought Gold", ToastLength.Short).Show();
            }
            Login.user.Player.CritChance = Boss.player.CritChance;
            btnUpgradeCritShop.Enabled = true;
        }

        // What to do if each button is pressed
        private async void OnClick(object sender, EventArgs e)
        {
            if (sender == btnUpgradeDamageShop)
            {
                UpgradeDamage();
                PlayerGold();
            }
            else if (sender == btnUpgradeGoldShop)
            {
                UpgradeGold();
                PlayerGold();
            }
            else if (sender == btnUpgradeCritShop)
            {
                UpgradeCrit();
                PlayerGold();
            }
            else if (sender == btnExitShop && btnExitShop.Enabled == true)
            {
                btnExitShop.Enabled = false;
                Login.user.Player.GoldOwns = Boss.player.GoldOwns;
                user = await firebaseManager.GetUser(Login.user.Id);
                HandleDialog();
            }
        }

        // if clicked yes
        public async void Yes(object sender, DialogClickEventArgs e)
        {
            await firebaseManager.UpdatePlayer(Boss.player, user.Id);
            Toast.MakeText(this, "Saving...", ToastLength.Short);
            btnExitShop.Text = "saving";
            monsterMaxHP = Intent.GetIntExtra("monsterMaxHP", 1500);
            Intent i = new Intent(this, typeof(Boss));
            btnExitShop.Enabled = true;
            i.PutExtra("1", 1);
            StartActivity(i);
        }

        // if clicked no
        public void No(object sender, DialogClickEventArgs e)
        {
            btnExitShop.Enabled = true;
        }

        // show dialong
        private void HandleDialog()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetMessage("Sure you want to exit?");
            builder.SetPositiveButton("Yes", Yes);
            builder.SetNegativeButton("No", No);
            builder.SetCancelable(false);
            d = builder.Create();
            d.Show();
        }

        // Connect between every xml object to activity variable
        private void InitViews()
        {
            Gold = FindViewById<TextView>(Resource.Id.tvGoldShop);
            DamageDealShop = FindViewById<TextView>(Resource.Id.tvDamageDealShop);
            MoreGoldPercentageShop = FindViewById<TextView>(Resource.Id.tvMoreGoldPercentageShop);
            CritPercentageShop = FindViewById<TextView>(Resource.Id.tvCritPercentageShop);

            btnUpgradeDamageShop = FindViewById<Button>(Resource.Id.btnUpgradeDamageShop);
            btnUpgradeGoldShop = FindViewById<Button>(Resource.Id.btnUpgradeGoldShop);
            btnUpgradeCritShop = FindViewById<Button>(Resource.Id.btnUpgradeCritShop);
            btnExitShop = FindViewById<Button>(Resource.Id.btnExitshopShop);
        }

        // shows the player stats in the XML
        private void ShowPlayerValues()
        {
            Gold.Text = Boss.player.GoldOwns.ToString();
            DamageDealShop.Text = Boss.player.Damage.ToString() + "(+5)";
            MoreGoldPercentageShop.Text = Boss.player.MoreGold.ToString() + "(+5%)";
            CritPercentageShop.Text = Boss.player.CritChance.ToString() + "(5%)";

            btnUpgradeDamageShop.Text = "Cost: " + upgradeDamageCost;
            btnUpgradeGoldShop.Text = "Cost: " + upgradeGoldCost;
            btnUpgradeCritShop.Text = "Cost: " + upgradeCritCost;
        }
    }
}