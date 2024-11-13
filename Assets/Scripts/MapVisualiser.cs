using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class MapVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;
    [SerializeField] private GameObject _planePrefab;

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

    [ContextMenu("ShowMap")]
    public void ShowMap()
    {
        _renderAdjacentAreas(lat,lon,zoomMap);
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

    private const int TileSize = 256; // Google Maps base tile size

    public static Vector2 LatLonToWorld(float latitude, float longitude)
    {

        float worldX = (longitude + 180) / 360 * TileSize;

        float sinLatitude = Mathf.Sin(latitude * Mathf.Deg2Rad);
        float worldY = (0.5f - Mathf.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Mathf.PI)) * TileSize;

        return new Vector2(worldX,worldY);
    }

    public static Vector2Int WorldToPixel(Vector2 world,int zoom)
    {
        float scale = Mathf.Pow(2, zoom) * TileSize;
        float pixelX = world.x * scale;
        float pixelY = world.y * scale;

        return new Vector2Int((int)(pixelX / TileSize), (int)(pixelY / TileSize));
    }

    public static Vector2 PixelToWorld(Vector2Int pixel, int zoom)
    {
        float scale = Mathf.Pow(2, zoom) * TileSize;
        float worldx = (pixel.x* TileSize) / scale;
        float worldy = (pixel.y* TileSize) / scale;

        return new Vector2(worldx,worldy);
    }

    public static Vector2Int PixelToTile(Vector2Int pixel)
    {
        return new Vector2Int((int)(pixel.x/TileSize), (int)(pixel.y / TileSize));
    }

    public static Vector2Int TileToPixel(Vector2Int tile)
    {
        return new Vector2Int(tile.x* TileSize, tile.y* TileSize);
    }

    Dictionary<GoogleTiles, GameObject> _planesByTileCords = new Dictionary<GoogleTiles, GameObject>();

    private void _renderAdjacentAreas(float lat,float lon,int zoom)
    {
        var worldCords = LatLonToWorld(lat, lon);
        var pixelCords = WorldToPixel(worldCords,zoom);
        var tileCords = PixelToTile(pixelCords);

        // for now
        foreach (var item in _planesByTileCords.Values)
        {
            Destroy(item.gameObject);
        }
        _planesByTileCords.Clear();
        //

        for (int i = -1;i<2;i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var subWorldCords = PixelToWorld(TileToPixel(new Vector2Int(tileCords.x + i, tileCords.y + j)),zoom);

                _renderCurrentArea(subWorldCords,new GoogleTiles(tileCords.x+i,tileCords.y+j,zoom));
            }
        }
    }

    private void _renderCurrentArea(Vector2 worldPos,GoogleTiles tileData)
    {
        GameObject plane;
        if (!_planesByTileCords.ContainsKey(tileData))
        {
            plane = Instantiate(_planePrefab, new Vector3(-worldPos.x, 0, worldPos.y), Quaternion.identity);
            _planesByTileCords.Add(tileData, plane);
        }
        else
        {
            plane = _planesByTileCords[tileData];
        }    

       // StartCoroutine(MapTilesAPI.instance.SetTileTexture(tileData, 0, plane.GetComponent<Renderer>()));
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