using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MapVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _pinObject;
    [SerializeField] private Renderer _renderer;

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

    public void ShowMap(double latitude, double longitude)
    {

    }

    [ContextMenu("ShowMap")]
    public void ShowMap()
    {
        var pixelCords = LatLonToPixel(lat,lon,zoomMap);
        var tileCords = PixelToTile(pixelCords);

        StartCoroutine(MapTilesAPI.instance.SetTileTexture(zoomMap, tileCords.x, tileCords.y, 0,_renderer));
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

    public static Vector2Int LatLonToPixel(float latitude, float longitude, int zoomLevel)
    {

        float worldX = (longitude + 180) / 360 * TileSize;

        float sinLatitude = Mathf.Sin(latitude * Mathf.Deg2Rad);
        float worldY = (0.5f - Mathf.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Mathf.PI)) * TileSize;


        float scale = Mathf.Pow(2, zoomLevel) * TileSize;
        float pixelX = worldX * scale;
        float pixelY = worldY * scale;

        return new Vector2Int((int)(pixelX / TileSize), (int)(pixelY / TileSize));
    }

    public static Vector2Int PixelToTile(Vector2Int pixel)
    {
        return new Vector2Int((int)(pixel.x/TileSize), (int)(pixel.y / TileSize));
    }
}
