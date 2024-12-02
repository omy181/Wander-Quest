using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;

    public void ShowPins(Places places)
    {
        foreach (Place p in places.places)
        {
            var pin = Instantiate(_pinObject, MapUtilities.ConvertGPSToUnityCord(p.location.latitude, p.location.longitude, 0, 0, 1, 1, MapVisualiser.zoomMap), Quaternion.identity);
            pin.GetComponent<PinObject>().Initialize(p);
        }
    }
}
