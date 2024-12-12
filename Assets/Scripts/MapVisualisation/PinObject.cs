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
        _mapVisualiser.OnMapUpdated += _updatePositionScale;
    }

    private void _updatePositionScale()
    {        
        transform.position = _mapVisualiser.GPSCordinateToUnityCordinate(_place.Location);
        transform.localScale = Vector3.Lerp(Vector3.one*0.005f,Vector3.one,_mapVisualiser.CurrentZoomLevel/22f);
    }
}
