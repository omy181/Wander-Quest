using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _name;
    private MapVisualiser _mapVisualiser;
    public QuestPlace _place { get; private set; }
    public void Initialize(QuestPlace place,MapVisualiser mapVisualiser)
    {
        _name.text = place.Name;
        transform.name = _name.text;
        _place = place;
        _mapVisualiser = mapVisualiser;
        _mapVisualiser.OnMapUpdated += _updatePosition;
    }

    private void _updatePosition()
    {        
        transform.position = _mapVisualiser.GPSCordinateToUnityCordinate(_place.Location); ;
    }
}
