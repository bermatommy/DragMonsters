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
using Android.Graphics;
using TommyBermatovDragMonsters.Resources.ZClasses;
using Android.Graphics.Drawables;

namespace TommyBermatovDragMonsters.ZClasses
{
    public class Target:Sprite
    {
        public Target(Bitmap bitmapT, int dx, int dy, int frameWidth, int frameHeight) : base(frameWidth, frameHeight)
        {
            this.bitmap = bitmapT;
            this.x = dx;
            this.y = dy;
        }

        public void Move(int newX, int newY)
        {
            base.Move();
            this.x = newX - bitmap.Width / 2;
            this.y = newY - bitmap.Height / 2;
        }
    }
}