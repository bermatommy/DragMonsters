using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using Android.Graphics;
using System.Linq;
using System.Text;

namespace TommyBermatovDragMonsters.Resources.ZClasses
{
    public class Monster : Sprite
    {
        private Random r = new Random();
        private int moveSizeX, moveSizeY;
        public int health, maxHealth;
        private int runNum = 0, minMS = 10, maxMS = 20;
        private double movement = 1;
        private bool isRight, isDown;
        public int location;

        public Monster(Bitmap bitmap2, int x, int y, int frameWidth, int frameHeight,int health, int monsterMaxHP) : base(frameWidth, frameHeight)
        {
            this.bitmap = bitmap2;
            this.x = x;
            this.y = y;
            this.health = health;
            this.maxHealth = monsterMaxHP;
        }

        public Monster(int health, int maxHealth) : base(health, maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;
        }

        //starting position of the monster
        public void StartPos(int width, int height)
        {
            this.x = width;
            this.y = height;
        }

        //Monster movement left and right
        public void MoveLR(int moveSize) // x
        {
            if (runNum <= 1)
            {
                if (x + bitmap.Width <= frameWidth)
                {
                    isRight = true;
                }
                else
                {
                    isRight = false;
                }
                runNum++;
            }
            else
            {
                if (x + bitmap.Width >= frameWidth && isRight) // end of right wall and switch direction
                {
                    isRight = false;
                    x -= moveSize;
                }
                else if(x + bitmap.Width <= frameWidth && isRight) // go right
                {
                    x += moveSize;
                }
                else if (x <= 0 && !isRight) // end of left wall and switch direction
                {
                    x += moveSize;
                    isRight = true;
                }
                else if (!isRight && x > 0) // go left
                {
                    x -= moveSize;
                }
            }
        }

        // Monster movement up and down
        public void MoveUD(int moveSize) // y
        {
            if (runNum <= 1)
            {
                if (y + bitmap.Height <= frameHeight)
                {
                    isDown = true;
                }
                else
                {
                    isDown = false;
                }
                runNum++;
            }
            else
            {
                if (y + bitmap.Height >= frameHeight && isDown) // end of down wall and switch direction
                {
                    isDown = false;
                    y -= moveSize;
                }
                else if(y + bitmap.Height <= frameHeight && isDown) // go down
                {
                    y += moveSize;
                }
                else if (y <= 0 && !isDown) // end of left wall and switch direction
                {
                    y += moveSize;
                    isDown = true;
                }
                else if (!isDown && y >= 0) // go left
                {
                    y -= moveSize;
                }
                
            }
        }


        /// Move the monster
        public bool MoveMonster()
        {
            base.Move();
            if (this.health == 0 && this.maxHealth <= 10000)
            {
                movement += 0.05;
            }
            moveSizeX = r.Next(minMS, maxMS);
            moveSizeY = r.Next(minMS, maxMS);
            MoveLR(moveSizeX);
            MoveUD(moveSizeY);


            //MoveLR((int)((double)moveSizeX * movement));
            //MoveUD((int)((double)moveSizeY * movement));
            return true;
        }
    }
}