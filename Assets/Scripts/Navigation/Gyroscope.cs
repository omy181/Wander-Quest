using UnityEngine;
using System.Collections;

public static class Gyroscope
{
    /*
    public static void CalibrateGyro()
    {

    }
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
        Vector3 gyroEuler = _correctAngle(gyro.eulerAngles);
        return gyroEuler.z;
    }

    private static Vector3 _correctAngle(Vector3 angle)
    {
        var orientation = Input.deviceOrientation;
        switch (orientation)
        {
            case DeviceOrientation.FaceUp:
            angle.z += 15;
            break;
            case DeviceOrientation.FaceDown:
                angle.z -= 15;
                break;
            case DeviceOrientation.Portrait:
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                break;
            default:
                Debug.LogWarning("Unknown device orientation");
                break;
        }
        return angle;
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
    */
    
    private static bool _sensorsInitialized = false;
    private static float _lastValidHeading = 0f;
    private static float _headingAccuracy = 0f;
    
    // Smoothing for compass readings
    private static float _smoothedHeading = 0f;
    private static readonly float HEADING_SMOOTHING = 0.15f;
    
    // Constants for accuracy thresholds
    private static readonly float MIN_ACCURACY_THRESHOLD = 15f; // degrees
    private static readonly float COMPASS_SETTLE_TIME = 2f; // seconds
    private static float _compassInitTime = 0f;
    
    public static IEnumerator InitializeSensors()
    {
        if (_sensorsInitialized) yield break;
        
        if (!_canUseSensors())
        {
            Debug.LogWarning("Required sensors not available");
            yield break;
        }
        
        Input.gyro.enabled = true;
        Input.compass.enabled = true;
        
        yield return new WaitForSeconds(0.5f);
        
        _compassInitTime = Time.time;
        _sensorsInitialized = true;
        
        Debug.Log("Sensors initialized successfully");
    }
    
    public static float FindAngleToTarget(float targetLatitude, float targetLongitude)
    {
        if (!_sensorsInitialized)
        {
            return -1;
        }
        
        if (!_canUseSensors())
        {
            return -1;
        }
        
        var gpsLocation = GPS.instance.GetLastGPSLocation();
        if (gpsLocation.latitude == 0 && gpsLocation.longitude == 0)
        {
            return -1;
        }
        
        if (!_isCompassReady())
        {
            return -1;
        }
        
        // Get compass reading with accuracy
        float rawHeading = Input.compass.trueHeading;
        float magneticHeading = Input.compass.magneticHeading;
        _headingAccuracy = Input.compass.headingAccuracy;
        
        // Validate compass accuracy - still proceed but with caution
        if (_headingAccuracy > MIN_ACCURACY_THRESHOLD && _headingAccuracy > 0)
        {
            // Low accuracy but continue with calculation
            Debug.LogWarning($"Low compass accuracy: {_headingAccuracy:F1}°");
        }
        
        if (_smoothedHeading == 0f) _smoothedHeading = rawHeading;
        _smoothedHeading = _lerpAngle(_smoothedHeading, rawHeading, HEADING_SMOOTHING);
        
        float bearing = _calculateBearing(
            (float)gpsLocation.latitude, 
            (float)gpsLocation.longitude, 
            targetLatitude, 
            targetLongitude
        );
        
        float angleDifference = Mathf.DeltaAngle(_smoothedHeading, bearing);
        
        float orientationAdjustedAngle = _adjustForScreenOrientation(angleDifference);
        
        _lastValidHeading = _smoothedHeading;
        
        return orientationAdjustedAngle;
    }
    
    private static bool _isCompassReady()
    {
        // Check if compass has had time to settle
        if (Time.time - _compassInitTime < COMPASS_SETTLE_TIME)
            return false;
            
        // Check if compass timestamp is valid
        if (Input.compass.timestamp == 0)
            return false;
            
        return true;
    }
    
    private static float _lerpAngle(float a, float b, float t)
    {
        float delta = Mathf.DeltaAngle(a, b);
        return a + delta * t;
    }
    
    private static float _adjustForScreenOrientation(float angle)
    {
        ScreenOrientation orientation = Screen.orientation;
        
        switch (orientation)
        {
            case ScreenOrientation.Portrait:
                return angle;
                
            case ScreenOrientation.PortraitUpsideDown:
                return angle + 180;
                
            case ScreenOrientation.LandscapeLeft:
                return angle + 90;
                
            case ScreenOrientation.LandscapeRight:
                return angle - 90;
                
            case ScreenOrientation.AutoRotation:
                // Handle auto-rotation by checking device orientation
                DeviceOrientation deviceOrientation = Input.deviceOrientation;
                switch (deviceOrientation)
                {
                    case DeviceOrientation.Portrait:
                        return angle;
                    case DeviceOrientation.PortraitUpsideDown:
                        return angle + 180;
                    case DeviceOrientation.LandscapeLeft:
                        return angle - 90;
                    case DeviceOrientation.LandscapeRight:
                        return angle + 90;
                    default:
                        return angle;
                }
                
            default:
                return angle;
        }
    }
    
    private static float _calculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        float lat1Rad = lat1 * Mathf.Deg2Rad;
        float lat2Rad = lat2 * Mathf.Deg2Rad;
        float dLonRad = (lon2 - lon1) * Mathf.Deg2Rad;
        
        float y = Mathf.Sin(dLonRad) * Mathf.Cos(lat2Rad);
        float x = Mathf.Cos(lat1Rad) * Mathf.Sin(lat2Rad) - 
                  Mathf.Sin(lat1Rad) * Mathf.Cos(lat2Rad) * Mathf.Cos(dLonRad);
        
        float bearing = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        
        return (bearing + 360) % 360;
    }
    
    private static bool _canUseSensors()
    {
        return SystemInfo.supportsGyroscope && SystemInfo.supportsLocationService;
    }

}
