using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapMode
{
    CloseUp,Country,Global
}

public class MapModeChanger : MonoBehaviour
{
    [SerializeField] private MapVisualiser _mapVisualiser;

    public MapMode mapMode;
    [ContextMenu("MapMode")]
    public void ChangeMapModeInspector()
    {
        ChangeMapMode(mapMode);
    }

    public void ChangeMapMode(MapMode mode)
    {
        int zoom = 0;
        switch (mode)
        {
            case MapMode.CloseUp:
                zoom = 17;
                break;
            case MapMode.Country:
                zoom = 4;
                break;
            case MapMode.Global:
                zoom = 3;
                break;
        }
        _mapVisualiser.SetZoom(zoom);
        _mapVisualiser.SetPosition(GPS.instance.GetLastGPSLocation());
        _mapVisualiser.UpdateAllPlaneTextures();
    }
}
