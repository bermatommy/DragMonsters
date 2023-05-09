using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Systems;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TommyBermatovDragMonsters.ZClasses;

namespace TommyBermatovDragMonsters.Resources.ZClasses
{
    public class GameBoard: SurfaceView
    {
        private Canvas canvas;
        private Player player;
        private Context context;
        private Paint paint;
        private MyHandler goldHandler;
        private Bitmap bitmapMonster, bitmapMap, bitmaptarget;
        public Monster monster;
        private Target target;
        public bool run, jumped = false;
        private readonly ThreadStart ts;
        private readonly System.Threading.Thread t;
        private int frmWidth, frmHeight, monsterSize , rwidth, rheight;
        private ZFirebase.FirebaseManager firebaseManager = new ZFirebase.FirebaseManager();
        private string userID;
        private int bank = 0, counter = 0;
        private Message msg;
        private Random rWidth = new Random();
        private Random rHeight = new Random();

        public GameBoard(Context context, int frmWidth, int frmHeight, string userID, int damage, int goldOwns, int moreGold,
                         int critChance, MyHandler handler, int health, int monsterHealth) : base(context)
        {
            this.context = context;
            this.frmHeight = frmHeight;
            this.frmWidth = frmWidth;
            this.goldHandler = handler;

            monsterSize = frmHeight / 10;

            InitBoard();
            bitmapMonster = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.boss_cropped);
            bitmapMonster = Bitmap.CreateScaledBitmap(bitmapMonster, monsterSize, monsterSize, false); // size

            bitmapMap = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.levels_pic);
            bitmapMap = Bitmap.CreateScaledBitmap(bitmapMap, frmWidth, frmHeight, false);

            bitmaptarget = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.target_cropped);
            bitmaptarget = Bitmap.CreateScaledBitmap(bitmaptarget, 150, 150, false);

            monster = new Monster(bitmapMonster, frmWidth / 2, frmHeight / 2, frmWidth, frmHeight, health ,monsterHealth);

            target = new Target(bitmaptarget, frmWidth / 2 - 300, frmHeight / 2 - 300, frmWidth, frmHeight);

            player = new Player(damage, moreGold, critChance, goldOwns);

            msg = goldHandler.ObtainMessage();
            msg.Arg1 = 111;
            goldHandler.SendMessage(msg);

            this.run = true;
            this.ts = new ThreadStart(Run);
            this.t = new System.Threading.Thread(this.ts);
            this.t.Start();
            this.userID = userID;
        }

        //Reset the gameboard every time
        public void InitBoard()
        {
            this.paint = new Paint();
            this.paint.Color = Color.Black;
        }

        ~GameBoard()
        {
            this.run = false;
        }

        //Start or end the game
        public void SetRun(bool command)
        {
            this.run = command;
        }

        // The game itself
        public void Run()
        {
            while (run == true)
            {
                monster.Move(); // draws the monster
                monster.MoveMonster(); // moves the monster
                JumpMonster();
                target.Move(); // draws the target
                DrawSurface();
                try
                {
                    System.Threading.Thread.Sleep(40);
                    counter++;
                    {
                        DamageMonster(player);
                    }
                }
                catch (InterruptedException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

        public void JumpMonster()
        {
            if (counter % 25 == 0 && jumped == false)
            {
                jumped = true;
            }
            if (jumped)
            {
                rwidth = rWidth.Next(0, this.frmWidth - this.monsterSize);
                rheight = rHeight.Next(0, this.frmHeight - this.monsterSize);
                monster.StartPos(rwidth, rheight);
                jumped = false;
            }
        }
        // update the player values
        public async void Update()
        {
            await firebaseManager.UpdatePlayer(player, userID);
        }


        //What to do after monster died
        public void InitMonster()
        {
            rwidth = rWidth.Next(0, this.frmWidth-this.monsterSize);
            rheight = rHeight.Next(0, this.frmHeight-this.monsterSize);

            this.monster.StartPos(rwidth, rheight);
            monster.health = monster.maxHealth;
            if (monster.maxHealth < 1000)
            {
                monster.health = 1000;
                monster.maxHealth = 1000;
            }
            if (monster.health % 100 != 0)
            {
                monster.health = monster.maxHealth - monster.maxHealth % 100;
            }
        }

        //Handles all the damage to monster
        public int DamageMonster(Player player) // the current hp of the monster
        {
            if (CheckTouch(monster, target))
            {

                if (monster.health > player.TotalDMG())
                {
                    this.monster.health -= player.TotalDMG();
                }
                else
                {
                    MonsterDead();
                    msg = goldHandler.ObtainMessage();
                    msg.Arg1 = 222;
                    msg.Arg2 = player.GoldOwns + bank;
                    bank = 0;
                    goldHandler.SendMessage(msg);
                }
                msg = goldHandler.ObtainMessage();
                msg.Arg1 = 111;
                msg.Arg2 = monster.health - player.TotalDMG();
                goldHandler.SendMessage(msg);

                
            }
            return monster.health;
        }

        // Handle the death of the monster
        public int MonsterDead()
        {
            bank += monster.maxHealth / 100 + (monster.maxHealth/100 * (player.MoreGold / 100));
            monster.maxHealth += 100;
            monster.health = monster.maxHealth;
            InitMonster();
            monster.health += player.TotalDMG();
            if (monsterSize > (frmHeight * frmWidth) * 10/100)
            {
                monsterSize -= (frmHeight * frmWidth) * 5 / 100;
            }
            return bank;
        }

        // Check if monster and player target intersects
        public bool CheckTouch(Monster monster, Target target)
        {
            Rect m = monster.GetRect();
            Rect t = target.GetRect();
            return Rect.Intersects(m, t);
        }

        // Display the gameboard
        private void DrawSurface()
        {
            if (Holder.Surface.IsValid)
            {
                canvas = Holder.LockCanvas();
                canvas.DrawPaint(paint);
                canvas.DrawBitmap(bitmapMap, 0, 0, paint);
                monster.Draw(canvas, paint);
                target.Draw(canvas, paint);
                Holder.UnlockCanvasAndPost(canvas);
            }
        }

        // Move the target to where the finger is
        public override bool OnTouchEvent(MotionEvent e)
        {
            int x = (int)e.GetX();
            int y = (int)e.GetY();
            target.Move(x, y);
            return true;
        }
    }
}