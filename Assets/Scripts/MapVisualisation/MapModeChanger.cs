using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapModeChanger : MonoBehaviour
{
    [SerializeField] private MapVisualiser _mapVisualiser;
    [SerializeField] private SwitchButton _switchButton;

    [SerializeField] private Button _zoomInButton;
    [SerializeField] private Button _zoomOutButton;

    private const int _closeUpZoom = 16;
    private const int _globalZoom = 3;

    private int _currentZoomLevel;
    private int _zoomLevel { get=> _currentZoomLevel; set {
            _currentZoomLevel = Mathf.Clamp(value,2,20);

            _mapVisualiser.SetZoom(_currentZoomLevel);
            _mapVisualiser.SetPosition(MapUtilities.WorldToPixel(MapUtilities.LatLonToWorld(GPS.instance.GetLastGPSLocation()), _currentZoomLevel));
            // _mapVisualiser.SetPosition(_mapVisualiser.GetCameraPixelCords());
            _mapVisualiser.UpdateAllPlaneTextures();
        } }

    private void MapInitalization()
    {
        _currentZoomLevel = _closeUpZoom;
        _mapVisualiser.SetZoom(_currentZoomLevel);
        _mapVisualiser.SetPosition(MapUtilities.WorldToPixel(MapUtilities.LatLonToWorld(GPS.instance.GetLastGPSLocation()), _currentZoomLevel));
        _mapVisualiser.UpdateAllPlaneTextures();
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

        StartCoroutine(MapTilesAPI.instance.StartMapTiles(MapInitalization));
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
