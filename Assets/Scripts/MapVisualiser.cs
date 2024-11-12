using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;

    public float zoomLevel = 1000;
    public void ShowPins(Places places)
    {
        foreach (Place p in places.places)
        {
            var pin = Instantiate(_pinObject, ConvertGPSToUnityCord(p.location.latitude,p.location.longitude, 0, 0, 1, 1),Quaternion.identity);
            pin.GetComponent<PinObject>().Initialize(p);
        }
        
    }

    public Vector3 ConvertGPSToUnityCord(double latitude, double longitude, double mapLatitude, double mapLongitude, double mapWidth, double mapHeight)
    {
        // Calculate the distance between the user and the map center
        double dx = (longitude - mapLongitude) * Mathf.Deg2Rad * 6371000 * Mathf.Cos((float)latitude * Mathf.Deg2Rad);
        double dy = (latitude - mapLatitude) * Mathf.Deg2Rad * 6371000;

        // Calculate the x and y position of the user on the map
        float x = (float)(((dx / mapWidth) + 0.5) * mapWidth);
        float y = (float)(((dy / mapHeight) + 0.5) * mapHeight);

        x /= zoomLevel;
        y /= zoomLevel;

        return new Vector3(x,0, y);
    }
}
