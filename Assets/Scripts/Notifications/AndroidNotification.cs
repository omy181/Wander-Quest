using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class AndroidNotification : MonoBehaviour
{
   
    public void RequestAuthorization(){
        if(!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")){
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    public void RegisterNotificationChannel(){
        var channel = new AndroidNotificationChannel{
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Discovered the Location"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendNotification(string title, string text){
        var notification = new Unity.Notifications.Android.AndroidNotification{
            Text = text,
            Title = title,
            SmallIcon = "icon_0"
        };
        

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }
   
}
