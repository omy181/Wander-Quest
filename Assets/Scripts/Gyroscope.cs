using UnityEngine;

public static class Gyroscope 
{
    
    public static float FindAngleToTarget(float targetLatitude, float targetLongitude){
        
        if(_canUseGyro()){
            var gyro = Input.gyro;
            gyro.enabled = true;

            float bearing = _calculateBearing((float)GPS.instance.GetLastGPSLocation().latitude, (float)GPS.instance.GetLastGPSLocation().longitude, targetLatitude,targetLongitude);
            float heading = _getGyroHeading();

            float angleDifference = Mathf.DeltaAngle(heading, bearing);

            return angleDifference;
        }

        return -1;
    }

    private static float _getGyroHeading()
    {
        Quaternion gyro = Input.gyro.attitude;
        Vector3 gyroEuler = gyro.eulerAngles;
        return gyroEuler.z;
    }


    private static float _calculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        float dLon = Mathf.Deg2Rad * (lon2 - lon1);
        float lat1Rad = Mathf.Deg2Rad * lat1;
        float lat2Rad = Mathf.Deg2Rad * lat2;

        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2Rad);
        float x = Mathf.Cos(lat1Rad) * Mathf.Sin(lat2Rad) - Mathf.Sin(lat1Rad) * Mathf.Cos(lat2Rad) * Mathf.Cos(dLon);
        float bearing = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        return (bearing + 360) % 360;
    }

    private static bool _canUseGyro(){
       return SystemInfo.supportsGyroscope;
    }

}
