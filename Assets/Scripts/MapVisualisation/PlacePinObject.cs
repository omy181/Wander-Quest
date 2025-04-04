using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePinObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _name;
    private GPSLocation _location;
    private MapVisualiser _mapVisualiser;
    public void Initialize(string name,GPSLocation location, MapVisualiser mapVisualiser)
    {
        _mapVisualiser = mapVisualiser;
        _location = location;
        _name.text = name;
        transform.name = _name.text;
        _mapVisualiser.OnMapUpdated += RefreshVisual;

        RefreshVisual();
    }

    private void OnEnable()
    {
        if (_mapVisualiser)
            RefreshVisual();
    }

    private void _updatePositionScale()
    {
        transform.position = _mapVisualiser.GPSCordinateToUnityCordinate(_location);
        transform.localScale = Vector3.Lerp(Vector3.one * 0.005f, Vector3.one, _mapVisualiser.CurrentZoomLevel / 22f);
    }

    public void RefreshVisual()
    {
        /// TODO: make the color of the text green if that country is visited before
        if(_name.text == "Turkey"|| _name.text == "Sweden" || _name.text == "Germany" || _name.text == "Greece" || _name.text == "Norway" || _name.text == "Denmark" || _name.text == "Estonia" || _name.text == "Belgium" || _name.text == "Netherlands" || _name.text == "France")
        _name.color = Color.green;

        _updatePositionScale();
    }
}
