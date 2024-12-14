using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _planePrefab;
    [SerializeField] private MapCamera _camera;

    private Vector2Int _currentTileCords;
    private Vector2Int _lastTileCords;
    private Vector3 _lastTileUnityCord;

    private int _zoomLevel = 19;
    public int CurrentZoomLevel => _zoomLevel;

    public Vector3 WorldCordToUnityCordinate(Vector2 worldCords)
    {
        var pixelCords = MapUtilities.WorldToPixel(worldCords,CurrentZoomLevel);
        var midPixel = (Vector2)MapUtilities.TileToPixel(_lastTileCords);

        var pixelMapSize = MapUtilities.GoogleMapsTileSize;
        var unityMapSize = MapUtilities.UnityTileSize;

        var offset = ((Vector2)pixelCords - midPixel) * (float)unityMapSize / (float)pixelMapSize;

        return _lastTileUnityCord + new Vector3(-offset.x+5, 0, offset.y-4.5f);
    }
    public Vector3 GPSCordinateToUnityCordinate(GPSLocation location)
    {
        var worldCords = MapUtilities.LatLonToWorld(location);
        return WorldCordToUnityCordinate(worldCords);
    }

    public Vector2 UnityToWorldCords(Vector3 unityCords)
    {
        var pixelMapSize = MapUtilities.GoogleMapsTileSize;
        var unityMapSize = MapUtilities.UnityTileSize;

        var offset = -_lastTileUnityCord + new Vector3(-(unityCords.x)-5, 0, unityCords.z+4.5f);
        var offset2d = new Vector2(offset.x, offset.z);
        var midPixel = MapUtilities.TileToPixel(_lastTileCords);
        var pixelCords = offset2d / (float)unityMapSize * (float)pixelMapSize + midPixel;

        return MapUtilities.PixelToWorld(new Vector2Int((int)pixelCords.x,(int)pixelCords.y),CurrentZoomLevel) ;
    }

    public Vector2 GetCameraWorldCords()
    {
        return UnityToWorldCords(_camera.transform.position);
    }

    public Action OnMapUpdated;

    public void SetZoom(int zoom)
    {
        _zoomLevel = zoom;
    }

    public void SetPosition(Vector2 worldlCords)
    {
        var pixelCords = MapUtilities.WorldToPixel(worldlCords,CurrentZoomLevel);
        _currentTileCords = MapUtilities.PixelToTile(pixelCords);
        _lastTileCords = _currentTileCords;
        _lastTileUnityCord = _getUnityPlaneByIndex(Vector2Int.zero).transform.position;

        OnMapUpdated?.Invoke();
    }

    public void SetPosition(GPSLocation gpsLocation)
    {
        Vector2 worldCords = MapUtilities.LatLonToWorld(gpsLocation);
        var pixelCords = MapUtilities.WorldToPixel(worldCords, CurrentZoomLevel);
        SetPosition(pixelCords);
    }

    public void SetCameraPosition(Vector3 pos)
    {
        _camera.Teleport(pos);
    }

    private void Start()
    {
        _initializePlanes();
    }

    private void Update()
    {
        _setPlanePoitions();
    }

    private void _initializePlanes()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                GameObject plane;
                plane = Instantiate(_planePrefab, new Vector3(i, 0, j) * MapUtilities.UnityTileSize, Quaternion.identity);
                _unityPlanes.Add(plane);
            }
        }

    }

    private void _setPlanePoitions()
    {
        if (_unityPlanes.Count == 0) return;

        var camPos = _camera.CamPosition2D;

        if (camPos.x > _getUnityPlaneByIndex(Vector2Int.zero).transform.position.x + MapUtilities.UnityTileSize / 2)
        {
            _currentTileCords += new Vector2Int(-1, 0);
            for (int i = -1; i < 2; i++)
            {
                var plane = _getUnityPlaneByIndex(new Vector2Int(-1, i));
                plane.transform.position += new Vector3(3 * MapUtilities.UnityTileSize, 0, 0);
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x -1, _currentTileCords.y + i, CurrentZoomLevel));
            }
            _reOrderUnityPlanes();
        }

        else if (camPos.x < _getUnityPlaneByIndex(Vector2Int.zero).transform.position.x - MapUtilities.UnityTileSize / 2)
        {
            _currentTileCords += new Vector2Int(1, 0);
            for (int i = -1; i < 2; i++)
            {
                var plane = _getUnityPlaneByIndex(new Vector2Int(1, i));
                plane.transform.position -= new Vector3(3 * MapUtilities.UnityTileSize, 0, 0);
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x +1, _currentTileCords.y + i, CurrentZoomLevel));
            }
            _reOrderUnityPlanes();
        }

        else if (camPos.y > _getUnityPlaneByIndex(Vector2Int.zero).transform.position.z + MapUtilities.UnityTileSize / 2)
        {
            _currentTileCords += new Vector2Int(0, 1);
            for (int i = -1; i < 2; i++)
            {
                var plane = _getUnityPlaneByIndex(new Vector2Int(i, -1));
                plane.transform.position += new Vector3(0, 0, 3 * MapUtilities.UnityTileSize);
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x - i, _currentTileCords.y + 1, CurrentZoomLevel));
            }
            _reOrderUnityPlanes();
        }

        else if (camPos.y < _getUnityPlaneByIndex(Vector2Int.zero).transform.position.z - MapUtilities.UnityTileSize / 2)
        {
            _currentTileCords += new Vector2Int(0, -1);
            for (int i = -1; i < 2; i++)
            {
                var plane = _getUnityPlaneByIndex(new Vector2Int(i, 1));
                plane.transform.position -= new Vector3(0, 0, 3 * MapUtilities.UnityTileSize);
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x - i, _currentTileCords.y - 1, CurrentZoomLevel));
            }
            _reOrderUnityPlanes();
        }
    }

    public void UpdateAllPlaneTextures()
    {
        for(int j = -1;j< 2;j++)
            for (int i = -1; i < 2; i++)
            {
                var plane = _getUnityPlaneByIndex(new Vector2Int(i, j));
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x - i, _currentTileCords.y + j, CurrentZoomLevel));
            }
    }

    private List<GameObject> _unityPlanes = new();

    private GameObject _getUnityPlaneByIndex(Vector2Int pos)
    {
        return _unityPlanes[(pos.x+1) * 3 + pos.y+1];
    }

    private void _reOrderUnityPlanes()
    {
        _unityPlanes.Sort((plane1, plane2) =>
        {
            int pos1_x = (int)plane1.transform.position.x;
            int pos1_z = (int)plane1.transform.position.z;

            int pos2_x = (int)plane2.transform.position.x;
            int pos2_z = (int)plane2.transform.position.z;

            Vector2Int pos1 = new Vector2Int(pos1_x, pos1_z);
            Vector2Int pos2 = new Vector2Int(pos2_x, pos2_z);

            if (pos1.x == pos2.x)
            {
                return pos1.y.CompareTo(pos2.y);
            }
            return pos1.x.CompareTo(pos2.x);
        });

    }

    private void _renderCurrentArea(GameObject plane,GoogleTiles tileData)
    {
        StartCoroutine(MapTilesAPI.instance.SetTileTexture(tileData, 0, plane.GetComponent<Renderer>()));
    }
}

public struct GoogleTiles
{
    public int X;
    public int Y;
    public int Zoom;

    public GoogleTiles(int x, int y, int zoom)
    {
        X = x;
        Y = y;
        Zoom = zoom;
    }

    public string Id => $"{Zoom}_{X}_{Y}";
}