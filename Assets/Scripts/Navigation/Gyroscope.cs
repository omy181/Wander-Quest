using UnityEngine;
using System.Collections;

public static class Gyroscope
{    
    private static bool _sensorsInitialized = false;
    private static float _headingAccuracy = 0f;
    private static float _smoothedHeading = 0f;
    private static readonly float HEADING_SMOOTHING = 0.15f;
    private static readonly float MIN_ACCURACY_THRESHOLD = 15f; 
    private static readonly float COMPASS_SETTLE_TIME = 2f;
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
    }

    public static float FindAngleToTarget(float targetLatitude, float targetLongitude)
    {
        if (!_sensorsInitialized || !_canUseSensors()) return -1;

        var gpsLocation = GPS.instance.GetLastGPSLocation();
        if (gpsLocation.latitude == 0 && gpsLocation.longitude == 0) return -1;

        if (!_isCompassReady()) return -1;

        float rawHeading = Input.compass.trueHeading;
        _headingAccuracy = Input.compass.headingAccuracy;

        if (_headingAccuracy > MIN_ACCURACY_THRESHOLD && _headingAccuracy > 0)
        {
            Debug.LogWarning($"Low compass accuracy: {_headingAccuracy:F1}°");
        }

        if (_smoothedHeading == 0f)
        {
           _smoothedHeading = rawHeading; 
        } 
        _smoothedHeading = Mathf.LerpAngle(_smoothedHeading, rawHeading, HEADING_SMOOTHING);

        float bearing = _calculateBearing(
            (float)gpsLocation.latitude,
            (float)gpsLocation.longitude,
            targetLatitude,
            targetLongitude
        );

        float angleDifference = Mathf.DeltaAngle(_smoothedHeading, bearing);
        return _adjustForScreenOrientation(angleDifference);
    }

    private static bool _isCompassReady()
    {
        if (Time.time - _compassInitTime < COMPASS_SETTLE_TIME)
            return false;
            
        if (Input.compass.timestamp == 0)
            return false;
            
        return true;
    }

    private static float _adjustForScreenOrientation(float angle)
    {
        switch (Screen.orientation)
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
                switch (Input.deviceOrientation)
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