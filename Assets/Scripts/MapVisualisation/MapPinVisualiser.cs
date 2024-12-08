using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;
    [SerializeField] private MapVisualiser _mapVisualiser;
    [SerializeField] private GameObject _userPin;

    public void ShowPins(Places places)
    {
        foreach (Place p in places.places)
        {
            //var pin = Instantiate(_pinObject, MapUtilities.ConvertGPSToUnityCord(p.location.latitude, p.location.longitude, 0, 0, 1, 1, _mapVisualiser.CurrentZoomLevel), Quaternion.identity);
            //pin.GetComponent<PinObject>().Initialize(p);
        }
    }

    private void Update()
    {
        MoveUserPin();
    }

    public void MoveUserPin()
    {
        _userPin.transform.position = _mapVisualiser.GPSCordinateToUnityCordinate(GPS.instance.GetLastGPSLocation());

    }
}
