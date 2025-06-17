using System;
using System.Text.RegularExpressions;
using UnityEngine;
public static class MapUtilities
{

    // for map visualisation

    public static int GoogleMapsTileSize = 256;
    public static int UnityTileSize = 10;

    public static bool DoesTileTexturePossible(GoogleTiles googleTiles)
    {
        float scale = Mathf.Pow(2, googleTiles.Zoom);
        return (googleTiles.X > 0 && googleTiles.X < scale && googleTiles.Y > 0 && googleTiles.Y < scale);
    }

    public static Vector2 LatLonToWorld(GPSLocation gpsLocation)
    {

        float worldX = ((float)gpsLocation.longitude + 180) / 360 * GoogleMapsTileSize;

        float sinLatitude = Mathf.Sin((float)gpsLocation.latitude * Mathf.Deg2Rad);
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
        float worldx = pixel.x / scale;
        float worldy = pixel.y / scale;

        return new Vector2(worldx * GoogleMapsTileSize, worldy * GoogleMapsTileSize);
    }

    public static Vector2Int PixelToTile(Vector2Int pixel)
    {
        return new Vector2Int((int)(pixel.x / GoogleMapsTileSize), (int)(pixel.y / GoogleMapsTileSize));
    }

    public static Vector2Int TileToPixel(Vector2Int tile)
    {
        return new Vector2Int(tile.x * GoogleMapsTileSize, tile.y * GoogleMapsTileSize);
    }

    public static GPSLocation WorldToLatLon(Vector2 worldCoords)
    {
        float longitude = (worldCoords.x / GoogleMapsTileSize) * 360 - 180;

        double n = Math.PI - 2.0 * Math.PI * (worldCoords.y / GoogleMapsTileSize);
        float latitude = (float)(Math.Atan(Math.Sinh(n)) * Mathf.Rad2Deg);

        return new GPSLocation(latitude, longitude);
    }

    // 1 Unity unit = ~111,000 meters (approx. for latitude/longitude projection)
    private const float metersPerUnit = 111000f;

    public static int GetDistanceInMeters(this float distance)
    {
        return Mathf.RoundToInt(distance * metersPerUnit);
    }

    public static int GetDistanceInKilometers(this float distance)
    {
        return Mathf.RoundToInt(distance * metersPerUnit / 1000f);
    }

    public static string GetDistanceInKilometersAndMeters(this float distance)
    {
        var meters = distance.GetDistanceInMeters();
        if (meters >= 1000)
            return distance.GetDistanceInKilometers()+"km";
        return meters+"m";
    }
}
