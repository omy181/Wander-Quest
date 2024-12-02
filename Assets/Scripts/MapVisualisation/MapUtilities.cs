using UnityEngine;
public static class MapUtilities
{
    public static Vector3 ConvertGPSToUnityCord(double latitude, double longitude, double mapLatitude, double mapLongitude, double mapWidth, double mapHeight, int zoomLevel)
    {
        double dx = (longitude - mapLongitude) * Mathf.Deg2Rad * 6371000 * Mathf.Cos((float)latitude * Mathf.Deg2Rad);
        double dy = (latitude - mapLatitude) * Mathf.Deg2Rad * 6371000;

        float x = (float)(((dx / mapWidth) + 0.5) * mapWidth);
        float y = (float)(((dy / mapHeight) + 0.5) * mapHeight);

        x /= zoomLevel;
        y /= zoomLevel;

        return new Vector3(x, 0, y);
    }

    // for map visualisation

    public static int GoogleMapsTileSize = 256;
    public static int UnityTileSize = 10;

    public static Vector2 LatLonToWorld(float latitude, float longitude)
    {

        float worldX = (longitude + 180) / 360 * GoogleMapsTileSize;

        float sinLatitude = Mathf.Sin(latitude * Mathf.Deg2Rad);
        float worldY = (0.5f - Mathf.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Mathf.PI)) * GoogleMapsTileSize;

        return new Vector2(worldX, worldY);
    }

    public static Vector2Int WorldToPixel(Vector2 world, int zoom)
    {
        float scale = Mathf.Pow(2, zoom) * GoogleMapsTileSize;
        float pixelX = world.x * scale;
        float pixelY = world.y * scale;

        return new Vector2Int((int)(pixelX / GoogleMapsTileSize), (int)(pixelY / GoogleMapsTileSize));
    }

    public static Vector2 PixelToWorld(Vector2Int pixel, int zoom)
    {
        float scale = Mathf.Pow(2, zoom) * GoogleMapsTileSize;
        float worldx = (pixel.x * GoogleMapsTileSize) / scale;
        float worldy = (pixel.y * GoogleMapsTileSize) / scale;

        return new Vector2(worldx, worldy);
    }

    public static Vector2Int PixelToTile(Vector2Int pixel)
    {
        return new Vector2Int((int)(pixel.x / GoogleMapsTileSize), (int)(pixel.y / GoogleMapsTileSize));
    }

    public static Vector2Int TileToPixel(Vector2Int tile)
    {
        return new Vector2Int(tile.x * GoogleMapsTileSize, tile.y * GoogleMapsTileSize);
    }
}
