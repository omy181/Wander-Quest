using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using Unity.Notifications.Android;


public class NotificationManager : MonoBehaviour
{
    [SerializeField] AndroidNotification androidNotification;

    // Start is called before the first frame update
    void Start()
    {
        androidNotification.RequestAuthorization();
        androidNotification.RegisterNotificationChannel();
    }

    private void OnApplicationFocus(bool focusStatus) {
        if(focusStatus == true){
            AndroidNotificationCenter.CancelAllNotifications();
            androidNotification.SendNotification("New Discovery", "You discovered a new location!");
        }
    }
}
