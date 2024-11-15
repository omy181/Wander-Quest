using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class Gyroscope 
{
    
    public static float FindAngleToTarget(float targetLatitude, float targetLongitude){
        
        if(_canUseGyro()){
            var gyro = Input.gyro;
            gyro.enabled = true;

            float bearing = _calculateBearing(GPS.instance.LastLocation.latitude, GPS.instance.LastLocation.longitude, targetLatitude,targetLongitude);
            float heading = _getGyroHeading();

            float angleDifference = Mathf.DeltaAngle(heading, bearing);

            return angleDifference;
        }

        return 0;
    }

    private static float _getGyroHeading()
    {
        // Gyroscope rotation matrix converted to a yaw (heading)
        Quaternion gyro = Input.gyro.attitude;
        Vector3 gyroEuler = gyro.eulerAngles;
        return gyroEuler.y; // Yaw represents the heading in Unity
    }


    private static float _calculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        float dLon = Mathf.Deg2Rad * (lon2 - lon1);
        float lat1Rad = Mathf.Deg2Rad * lat1;
        float lat2Rad = Mathf.Deg2Rad * lat2;

        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2Rad);
        float x = Mathf.Cos(lat1Rad) * Mathf.Sin(lat2Rad) - Mathf.Sin(lat1Rad) * Mathf.Cos(lat2Rad) * Mathf.Cos(dLon);
        float bearing = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        // Ensure bearing is within 0 to 360 degrees
        return (bearing + 360) % 360;
    }

    private static bool _canUseGyro(){
        if(SystemInfo.supportsGyroscope){
            return true;
        }
        else{
            Debug.LogError("System does not support gyroscope");
            return false;
        }
       
    }

/*
    UnityEngine.Gyroscope gyro;
    bool gyroEnabled;
    Quaternion rotation;
    public TextMeshProUGUI textMessage;
    public GameObject arrow;
    public float latitude;
    public float longitude;
    public float rotationSpeed = 2f;
    
    
    public TextMeshProUGUI textMessage2;


    bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            //arrow.transform.rotation = Quaternion.Euler (90f, 90f, 0f);
            //rotation = new Quaternion (0, 0, 1, 0);
            rotation = Quaternion.identity;
            return true;
        }
        return false;
    }

    void RotateArrow(float targetLatitude, float targetLongitude)
    {
        // Calculate the bearing angle from the current location to the target location
        float bearing = CalculateBearing(latitude, longitude, targetLatitude, targetLongitude);
        

        // Adjust the arrow's rotation based on the gyroscope and calculated bearing
        //Quaternion gyroAdjustedRotation = gyro.attitude * rotation;
        Quaternion gyroAdjustedRotation = gyro.attitude * Quaternion.Euler(90f, 0f, -90f);  // Adjusted for alignment
        Quaternion targetRotation = Quaternion.Euler(0, 0, -bearing); // Negative bearing for correct alignment

        arrow.transform.localRotation = Quaternion.Slerp(arrow.transform.localRotation, targetRotation * gyroAdjustedRotation, Time.deltaTime * rotationSpeed);
    }

    // Helper method to calculate the bearing between two geographic locations
    float CalculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        float dLon = Mathf.Deg2Rad * (lon2 - lon1);
        float lat1Rad = Mathf.Deg2Rad * lat1;
        float lat2Rad = Mathf.Deg2Rad * lat2;

        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2Rad);
        float x = Mathf.Cos(lat1Rad) * Mathf.Sin(lat2Rad) - Mathf.Sin(lat1Rad) * Mathf.Cos(lat2Rad) * Mathf.Cos(dLon);
        float bearing = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        // Ensure bearing is within 0 to 360 degrees
        return (bearing + 360) % 360;
    }

    private void Start()
    {
        Debug.Log("Started");
        gyroEnabled = EnableGyro();
        rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (gyroEnabled){
            //transform.localRotation = gyro.attitude * rotation;
            //Quaternion localRotation = transform.localRotation;
            Quaternion localRotation = gyro.attitude * Quaternion.Euler(90f, 0f, -90f);
            textMessage.text = $"x: {localRotation.x:F5}, y: {localRotation.y:F5}, z: {localRotation.z:F5}, w: {localRotation.w:F5}";
            RotateArrow(latitude, longitude);
        }
        else
        {
            textMessage.text = "The gyroscope is not supported";
        }
    }

    public void tryME(){
        textMessage2.text = "YOU DID IT!";
    }*/
}
