using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;
    [SerializeField] private GameObject _planePrefab;
    [SerializeField] private MapCamera _camera;

    public float zoomLevel = 1000;

    [Header("Map Controls")]
    public int zoomMap = 10;
    private float lat = 38.05508810860761f;
    private float lon = 27.023444230965133f;
    public void ShowPins(Places places)
    {
        foreach (Place p in places.places)
        {
            var pin = Instantiate(_pinObject, ConvertGPSToUnityCord(p.location.latitude,p.location.longitude, 0, 0, 1, 1),Quaternion.identity);
            pin.GetComponent<PinObject>().Initialize(p);
        }
    }

    private void Start()
    {
        _initializePlanes();
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
                plane = Instantiate(_planePrefab, new Vector3(i, 0, j) * UnityTileSize, Quaternion.identity);
                _unityPlanes.Add(plane);
            }
        }

    }

    private void _setPlanePoitions(int zoom)
    {
        if (_unityPlanes.Count == 0) return;

        var camPos = _camera.CamPosition2D;

        if (camPos.x > _getUnityPlaneByIndex(Vector2Int.zero).transform.position.x + UnityTileSize / 2)
        {
            for (int i = -1; i < 2; i++)
            {
                var worldCords = LatLonToWorld(lat, lon);
                var pixelCords = WorldToPixel(worldCords, zoom);
                var tileCords = PixelToTile(pixelCords);

                var plane = _getUnityPlaneByIndex(new Vector2Int(-1, i));
                plane.transform.position += new Vector3(3 * UnityTileSize, 0, 0);
                _renderCurrentArea(plane, new GoogleTiles(tileCords.x -1, tileCords.y + i, zoom));
            }
            _reOrderUnityPlanes();
        }

        else if (camPos.x < _getUnityPlaneByIndex(Vector2Int.zero).transform.position.x - UnityTileSize / 2)
        {
            for (int i = -1; i < 2; i++)
            {
                var worldCords = LatLonToWorld(lat, lon);
                var pixelCords = WorldToPixel(worldCords, zoom);
                var tileCords = PixelToTile(pixelCords);

                var plane = _getUnityPlaneByIndex(new Vector2Int(1, i));
                plane.transform.position -= new Vector3(3 * UnityTileSize, 0, 0);
                _renderCurrentArea(plane, new GoogleTiles(tileCords.x +1, tileCords.y + i, zoom));
            }
            _reOrderUnityPlanes();
        }

        else if (camPos.y > _getUnityPlaneByIndex(Vector2Int.zero).transform.position.z + UnityTileSize / 2)
        {
            for (int i = -1; i < 2; i++)
            {
                var worldCords = LatLonToWorld(lat, lon);
                var pixelCords = WorldToPixel(worldCords, zoom);
                var tileCords = PixelToTile(pixelCords);

                var plane = _getUnityPlaneByIndex(new Vector2Int(i, -1));
                plane.transform.position += new Vector3(0, 0, 3 * UnityTileSize);
                _renderCurrentArea(plane, new GoogleTiles(tileCords.x + i, tileCords.y - 1, zoom));
            }
            _reOrderUnityPlanes();
        }

        else if (camPos.y < _getUnityPlaneByIndex(Vector2Int.zero).transform.position.z - UnityTileSize / 2)
        {
            var worldCords = LatLonToWorld(lat, lon);
            var pixelCords = WorldToPixel(worldCords, zoom);
            var tileCords = PixelToTile(pixelCords);

            for (int i = -1; i < 2; i++)
            {
                var plane = _getUnityPlaneByIndex(new Vector2Int(i, 1));
                plane.transform.position -= new Vector3(0, 0, 3 * UnityTileSize);
                _renderCurrentArea(plane, new GoogleTiles(tileCords.x + i, tileCords.y + 1, zoom));
            }
            _reOrderUnityPlanes();
        }
    }

    // for pin placement
    public Vector3 ConvertGPSToUnityCord(double latitude, double longitude, double mapLatitude, double mapLongitude, double mapWidth, double mapHeight)
    {
        double dx = (longitude - mapLongitude) * Mathf.Deg2Rad * 6371000 * Mathf.Cos((float)latitude * Mathf.Deg2Rad);
        double dy = (latitude - mapLatitude) * Mathf.Deg2Rad * 6371000;

        float x = (float)(((dx / mapWidth) + 0.5) * mapWidth);
        float y = (float)(((dy / mapHeight) + 0.5) * mapHeight);

        x /= zoomLevel;
        y /= zoomLevel;

        return new Vector3(x,0, y);
    }


    // for map visualisation

    private const int GoogleMapsTileSize = 256;
    private const int UnityTileSize = 10;

    public static Vector2 LatLonToWorld(float latitude, float longitude)
    {

        float worldX = (longitude + 180) / 360 * GoogleMapsTileSize;

        float sinLatitude = Mathf.Sin(latitude * Mathf.Deg2Rad);
        float worldY = (0.5f - Mathf.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Mathf.PI)) * GoogleMapsTileSize;

        return new Vector2(worldX,worldY);
    }

    public static Vector2Int WorldToPixel(Vector2 world,int zoom)
    {
        float scale = Mathf.Pow(2, zoom) * GoogleMapsTileSize;
        float pixelX = world.x * scale;
        float pixelY = world.y * scale;

        return new Vector2Int((int)(pixelX / GoogleMapsTileSize), (int)(pixelY / GoogleMapsTileSize));
    }

    public static Vector2 PixelToWorld(Vector2Int pixel, int zoom)
    {
        float scale = Mathf.Pow(2, zoom) * GoogleMapsTileSize;
        float worldx = (pixel.x* GoogleMapsTileSize) / scale;
        float worldy = (pixel.y* GoogleMapsTileSize) / scale;

        return new Vector2(worldx,worldy);
    }

    public static Vector2Int PixelToTile(Vector2Int pixel)
    {
        return new Vector2Int((int)(pixel.x/GoogleMapsTileSize), (int)(pixel.y / GoogleMapsTileSize));
    }

    public static Vector2Int TileToPixel(Vector2Int tile)
    {
        return new Vector2Int(tile.x* GoogleMapsTileSize, tile.y* GoogleMapsTileSize);
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

    private void _renderAdjacentAreas(float lat,float lon,int zoom)
    {
        var worldCords = LatLonToWorld(lat, lon);
        var pixelCords = WorldToPixel(worldCords,zoom);
        var tileCords = PixelToTile(pixelCords);


        for (int i = -1;i<2;i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //var subWorldCords = PixelToWorld(TileToPixel(new Vector2Int(tileCords.x + i, tileCords.y + j)),zoom);

                _renderCurrentArea(_getUnityPlaneByIndex(new Vector2Int(i,j)), new GoogleTiles(tileCords.x+i,tileCords.y+j,zoom));
            }
        }
    }

    private void _renderCurrentArea(GameObject plane,GoogleTiles tileData)
    {
        //StartCoroutine(MapTilesAPI.instance.SetTileTexture(tileData, 0, plane.GetComponent<Renderer>()));
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