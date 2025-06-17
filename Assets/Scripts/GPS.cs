using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

[Serializable]
public struct GPSLocation
{
    public double latitude;
    public double longitude;

    public GPSLocation(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public override string ToString()
    {
        return latitude + "," + longitude;
    }
}

public class GPS : Singleton<GPS>
{
    [SerializeField] private double test_latitude = 56.6420988598888;
    [SerializeField] private double test_longitude = 14.91592350839002;

    private LocationInfo _lastLocation;

    public bool isUpdating;

    public GPSLocation GetLastGPSLocation()
    {
        if (ApplicationSettings.IsUnityEditor)
        {
            return new GPSLocation(test_latitude, test_longitude);
        }
        else
        {
            return new GPSLocation(_lastLocation.latitude,_lastLocation.longitude);
        }
    }

    private void Update()
    {

        if (!isUpdating && !ApplicationSettings.IsUnityEditor)
        {
            StartCoroutine(GetLocation());
            isUpdating = !isUpdating;
        }

#if UNITY_EDITOR
        double precision = 0.00001d;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            precision *= 100;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            test_longitude -= precision;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            test_longitude += precision;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            test_latitude += precision;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            test_latitude -= precision;
        }
#endif
    }


    IEnumerator GetLocation()
    {

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield return new WaitForSeconds(10);

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            _lastLocation = Input.location.lastData;
            //gpsOut.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + 100f + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
            // Access granted and location value could be retrieved
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        isUpdating = !isUpdating;
        Input.location.Stop();
    }
}
