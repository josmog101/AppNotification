using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Messaging;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppNotification.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            FirebaseApp.InitializeApp(this);
            var body = message.Data["title"];
            ShowNotification(body);
            
            //SendNotification(body);
        }

        private void ShowNotification(string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = "Test Description",
                Title = "Notification!",
                ReturningData = "Dummy Data",
                NotificationId = 1337
                //NotifyTime = DateTime.Now.AddSeconds(5)
            };

            NotificationCenter.Current.Show(notification);
        }

        private void SendNotification(string title)
        {
            
            var intent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            Notification.BigTextStyle textStyle = new Notification.BigTextStyle();
            textStyle.BigText(title);
            textStyle.SetSummaryText("Ver mas...");

            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.notification_bg)
                .SetContentTitle("Dólar al Día")
                .SetStyle(textStyle)
                .SetContentText(title)
                .SetSound(defaultSoundUri)
                .SetVibrate(new long[] { 0, 1000, 0, 0, 0 })
                .SetAutoCancel(true)
                .SetLights(0x00FF00, 300, 100)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}