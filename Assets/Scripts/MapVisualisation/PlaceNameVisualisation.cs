using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceNameVisualisation : MonoBehaviour
{
    private MapVisualiser _mapVisualiser;
    [SerializeField] private PlacePinObject _placeNamePrefab;
    void Start()
    {
        _mapVisualiser = FindObjectOfType<MapVisualiser>();
        _createNamePins();       
    }

    private void _createNamePins()
    {
        var countryToLoc = CountryDatabase.instance.CountryToLocation;

        foreach (var country in countryToLoc)
        {
            var textObj = Instantiate(_placeNamePrefab,transform);
            textObj.Initialize(country.Key,country.Value, _mapVisualiser);
        }
    }
}
