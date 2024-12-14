using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapPinVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;
    [SerializeField] private MapVisualiser _mapVisualiser;
    [SerializeField] private GameObject _userPin;

    private List<PinObject> _pins = new();

    public void ShowQuestPins(Quest quest)
    {
        CreatePins(quest.GetPlaces());
    }

    public void CreatePins(List<QuestPlace> places)
    {
        foreach (var place in places)
        {
            if (_pins.Any(placeObj => placeObj._place.ID == place.ID)) continue;

            var pin = Instantiate(_pinObject, _mapVisualiser.GPSCordinateToUnityCordinate(place.Location), Quaternion.identity);
            var pinobject = pin.GetComponent<PinObject>();
            pinobject.Initialize(place, _mapVisualiser);

            pinobject.gameObject.SetActive(false);

            _pins.Add(pinobject);
        }
    }

    public void FocusPins(List<QuestPlace> places)
    {
        HideAllPins();
        foreach (var place in places)
        {
            var pin = _pins.Find(p => p._place.ID == place.ID);
            if (pin)
            {
                pin.gameObject.SetActive(true);
            }
        }
    }

    public void HideAllPins()
    {
        foreach (var pin in _pins)
        {
            pin.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _moveUserPin();
    }

    private void _moveUserPin()
    {
        _userPin.transform.position = _mapVisualiser.GPSCordinateToUnityCordinate(GPS.instance.GetLastGPSLocation());
    }
}
