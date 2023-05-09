using Android.App;
using Android.Content;
using Android.Graphics;
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
    public class Sprite /// Handles all the movement in the game
    {
        protected Bitmap bitmap;//the picture of the sprite
        protected int frameHeight;//the height of the frame
        protected int frameWidth;//the width of the frame
        protected int x;//the x position of the sprite
        protected int y;//the y position of the sprite
        protected Rect rect;//a rect around the sprite

        public Sprite(int frameWidth, int frameHeight)
        {
            this.frameHeight = frameHeight;
            this.frameWidth = frameWidth;
            this.rect = new Rect();
        }

        // Get X value of object
        public int GetX()
        {
            return this.x;
        }

        // Get Y value of object
        public int GetY()
        {
            return this.y;
        }

        public Rect GetRect()
        {
            return this.rect;
        }

        //Draw the object in its place
        public void Draw(Canvas canvas, Paint paint)
        {
            canvas.DrawBitmap(bitmap, x, y, paint);
        }//draws the sprite

        // Moves an object
        public void Move()
        {
            rect.Set(x, y, x + bitmap.Width, y + bitmap.Height);
        }
    }
}