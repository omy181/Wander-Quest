using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _name;
    private MapVisualiser _mapVisualiser;
    public Place _place { get; private set; }
    public void Initialize(Place place,MapVisualiser mapVisualiser)
    {
        _name.text = place.displayName.text;
        transform.name = _name.text;
        _place = place;
        _mapVisualiser = mapVisualiser;
        _mapVisualiser.OnMapUpdated += _updatePosition;
    }

    private void _updatePosition()
    {        
        transform.position = _mapVisualiser.GPSCordinateToUnityCordinate(new GPSLocation(_place.location.latitude, _place.location.longitude)); ;
    }
}
