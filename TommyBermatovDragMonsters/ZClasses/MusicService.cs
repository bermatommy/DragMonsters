using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;

namespace TommyBermatovDragMonsters.Resources.ZClasses
{
    [Service]
    class MusicService : Service
    {
        private string chId = "ChannelID1";
        private MediaPlayer mediaPlayer;
        private ISharedPreferences sp;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnCreate()
        {
            base.OnCreate();
            mediaPlayer = MediaPlayer.Create(this, Resource.Raw.theme_song);
            mediaPlayer.Looping = true;
            mediaPlayer.SetVolume(100, 100);
            mediaPlayer.Completion += SongEnded;
            sp = this.GetSharedPreferences("media_player", FileCreationMode.Private);
        }
        private void SongEnded(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Song stopped", ToastLength.Short).Show();
            this.OnDestroy();
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            if (sp != null)
            {
                int newPos = sp.GetInt("currentPosition", 0);
                mediaPlayer.SeekTo(newPos);
            }
            mediaPlayer.Start();
            Intent notificationIntent = new Intent(this, typeof(MainActivity));
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutInt("currentPosition", mediaPlayer.CurrentPosition);
            editor.Commit();
            mediaPlayer.Stop();
            base.OnDestroy();
            StopForeground(true);
            StopSelf();
        }
    }
}