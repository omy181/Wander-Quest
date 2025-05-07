using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapModeChanger : Singleton<MapModeChanger>
{
    [SerializeField] private MapVisualiser _mapVisualiser;
    [SerializeField] private SwitchButton _switchButton;

    [SerializeField] private Button _zoomInButton;
    [SerializeField] private Button _zoomOutButton;
    [SerializeField] private Button _reCenterButton;

    private const int _closeUpZoom = 16;
    private const int _globalZoom = 3;

    private int _currentZoomLevel;
    private int _zoomLevel { get=> _currentZoomLevel; set {
            _currentZoomLevel = Mathf.Clamp(value,2,20);

            if(_currentZoomLevel != value) return;
            _reCenter();

            /*
            var worldcords = _mapVisualiser.GetCameraWorldCords();
            _mapVisualiser.SetZoom(_currentZoomLevel);
            //_mapVisualiser.SetPosition(MapUtilities.WorldToPixel(MapUtilities.LatLonToWorld(GPS.instance.GetLastGPSLocation()), _currentZoomLevel));
            _mapVisualiser.SetPosition(worldcords);
            _mapVisualiser.SetCameraPosition(_mapVisualiser.WorldCordToUnityCordinate(worldcords));
            _mapVisualiser.UpdateAllPlaneTextures();
            */
        }
    }

    private void MapInitalization()
    {
        _currentZoomLevel = _closeUpZoom;
        _reCenter();
    }

    private void Start()
    {
        _switchButton.Initialize(new Action[]
        {
            () => _zoomLevel = _closeUpZoom,
            () => _zoomLevel = _globalZoom,
        });

        _zoomInButton.onClick.AddListener(_zoomIn);
        _zoomOutButton.onClick.AddListener(_zoomOut);
        _reCenterButton.onClick.AddListener(_reCenter);

        StartCoroutine(MapTilesAPI.instance.StartMapTiles(MapInitalization));
    }

    private void Update()
    {
        _checkIsOnCenter();
    }

    private void _checkIsOnCenter()
    {
        var despos = _mapVisualiser.GPSCordinateToUnityCordinate(GPS.instance.GetLastGPSLocation());
        var pos = new Vector3(despos.x, _mapVisualiser.GetCameraPosition().y, despos.z);
        var dis = Vector3.Distance(_mapVisualiser.GetCameraPosition(), pos);
        _reCenterButton.gameObject.SetActive(dis >= 4);
    }

    public void FocusOnPlace(QuestPlace place)
    {
        _mapVisualiser.SetPosition(MapUtilities.LatLonToWorld(place.Location));
        _mapVisualiser.SetCameraPosition(_mapVisualiser.GPSCordinateToUnityCordinate(place.Location));
        _mapVisualiser.UpdateAllPlaneTextures();
    }

    private void _reCenter()
    {
        _mapVisualiser.SetZoom(_currentZoomLevel);
        _mapVisualiser.SetPosition(MapUtilities.LatLonToWorld(GPS.instance.GetLastGPSLocation()));
        _mapVisualiser.SetCameraPosition(_mapVisualiser.GPSCordinateToUnityCordinate(GPS.instance.GetLastGPSLocation()));
        _mapVisualiser.UpdateAllPlaneTextures();
    }

    private void _zoomIn()
    {
        _zoomLevel += 2;
    }

    private void _zoomOut()
    {
        _zoomLevel -= 2;
    }
}
