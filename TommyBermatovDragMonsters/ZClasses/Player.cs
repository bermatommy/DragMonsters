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

namespace TommyBermatovDragMonsters.Resources.ZClasses
{
    public class Player
    {
        private int damage, moreGold, critChance, goldOwns;
        private int rCrit;
        private Random r = new Random();

        public Player(int damage, int moreGold, int critChance, int goldOwns)
        {
            this.damage = damage;
            this.moreGold = moreGold;
            this.critChance = critChance;
            this.goldOwns = goldOwns;
        }

        public Player()
        {
            this.damage = 10;
            this.moreGold = 0;
            this.critChance = 0;
            this.GoldOwns = 0;
        }

        public int Damage { get => damage; set => damage = value; }
        public int MoreGold { get => moreGold; set => moreGold = value; }
        public int CritChance { get => critChance; set => critChance = value; }
        public int GoldOwns { get => goldOwns; set => goldOwns = value; }
        
        //Calculates the total damage from the player
        public int TotalDMG()
        {
            if (IfCrit())
            {
                return this.damage * 2;
            }
            else
                return this.damage;
        }

        //Calculate if hit is is a crit
        public bool IfCrit()
        {
            rCrit = r.Next(1, 100);
            if (critChance >= rCrit)
                return true;
            else
                return false;
        }
    }
}