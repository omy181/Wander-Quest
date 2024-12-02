using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _planePrefab;
    [SerializeField] private MapCamera _camera;

    [Header("Map Controls")]
    public static int zoomMap = 19;


    private Vector2Int _currentTileCords;

    public void SetPosition(float lat,float lon,int zoom)
    {
        var worldCords = MapUtilities.LatLonToWorld(lat, lon);
        var pixelCords = MapUtilities.WorldToPixel(worldCords, zoom);
        _currentTileCords = MapUtilities.PixelToTile(pixelCords);
    }

    private void Start()
    {
        _initializePlanes();
        SetPosition(38.05508810860761f, 27.023444230965133f, zoomMap);
    }

    private void Update()
    {
        _setPlanePoitions(zoomMap);
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

    private void _setPlanePoitions(int zoom)
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
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x -1, _currentTileCords.y + i, zoom));
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
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x +1, _currentTileCords.y + i, zoom));
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
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x - i, _currentTileCords.y + 1, zoom));
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
                _renderCurrentArea(plane, new GoogleTiles(_currentTileCords.x - i, _currentTileCords.y - 1, zoom));
            }
            _reOrderUnityPlanes();
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
}