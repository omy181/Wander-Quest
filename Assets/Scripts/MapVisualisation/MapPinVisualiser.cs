using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class MapPinVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;
    [SerializeField] private MapVisualiser _mapVisualiser;
    [SerializeField] private GameObject _userPin;

    private List<PinObject> _pins = new();

    public void ShowPins(Places places)
    {
        foreach (Place p in places.places)
        {
            if (_pins.Any(po => po._place.id == p.id)) continue;

            var pin = Instantiate(_pinObject, _mapVisualiser.GPSCordinateToUnityCordinate(new GPSLocation(p.location.latitude, p.location.longitude)), Quaternion.identity);
            var pinobject = pin.GetComponent<PinObject>();
            pinobject.Initialize(p, _mapVisualiser);

            _pins.Add(pinobject);
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
