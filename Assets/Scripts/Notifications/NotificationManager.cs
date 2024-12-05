using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using Unity.Notifications.Android;

public class NotificationManager : Singleton<NotificationManager>
{
    [SerializeField] AndroidNotification androidNotification;

    private const string discoveryIcon = "icon_0"; 
    private const string friendIcon = "icon_1";

    void Start()
    {
        androidNotification.RequestAuthorization();
        androidNotification.RegisterNotificationChannel();
        SendDiscoveryNotification("Migros");
        SendMissedYouNotification();
        SendFriendRequestNotification("Seden");
    }

    public static void SendDiscoveryNotification(string place){
        instance.androidNotification.SendNotification("New Discovery", $"You discovered a new {place}!", discoveryIcon);
    }

    public static void SendMissedYouNotification(){
        instance.androidNotification.SendNotification("Come Back", "We missed you!", discoveryIcon);
    }

    public static void SendFriendRequestNotification(string friendName){
        instance.androidNotification.SendNotification("New Friend", $"{friendName} wants to be friend with you!", friendIcon);
    }  

}

