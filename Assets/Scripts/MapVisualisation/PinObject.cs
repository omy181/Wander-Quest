using Holylib.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _name;
    [SerializeField] private Image _back;
    [SerializeField] private MeshRenderer _model;
    private MapVisualiser _mapVisualiser;
    public QuestPlace _place { get; private set; }
    public void Initialize(QuestPlace place,MapVisualiser mapVisualiser)
    {
        _name.text = place.Name;
        transform.name = _name.text;
        _place = place;
        _mapVisualiser = mapVisualiser;
        _mapVisualiser.OnMapUpdated += RefreshVisual;

        RefreshVisual();

        _model.transform.Rotate(0,Random.Range(-90f,90f),0);
    }

    private void OnEnable()
    {
        if(_mapVisualiser)
            RefreshVisual();
    }

    private void _updatePositionScale()
    {        
        transform.position = _mapVisualiser.GPSCordinateToUnityCordinate(_place.Location);
        transform.localScale = Vector3.Lerp(Vector3.one*0.005f,Vector3.one,_mapVisualiser.CurrentZoomLevel/22f);
    }

    public void RefreshVisual()
    {
        if (_place.IsTraveled)
        {
            HolyUtilities.ChangeMaterialColor(_model, Color.green);
            _back.color = Color.green;
        }
        else
        {
            HolyUtilities.ChangeMaterialColor(_model, Color.red);
            _back.color = Color.red;
        }

        _updatePositionScale();
    }
}
