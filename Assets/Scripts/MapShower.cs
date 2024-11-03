using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapShower : MonoBehaviour
{
    private string apiKey;

    public float lat = -33.85660618890487f;
    public float lon = 151.2150079157325f;
    public float zoom = 14;

    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;

    public enum type { roadmap, satellite, gybrid, terrain };
    public type mapType = type.roadmap;

    private string url = "";

    private int mapWidth = 640;
    private int mapHeight = 640;
    private bool mapIsLoading = false;  //not used. Can be used to know that the map is loading
    private Rect rect;

    private string apiKeyLast;
    private float latLast = -33.85660618890487f;
    private float lonLast = 151.2150079157325f;
    private float zoomLast = 14;
    private resolution mapResolutionLast = resolution.low;
    private type mapTypeLast = type.roadmap;
    private bool updateMap = true;

    void Start()
    {
        TextAsset apitext = Resources.Load<TextAsset>("apikey");

        if (apitext == null )
        {
            Debug.LogError("API Key not found");
            return;
        }

        apiKey = apitext.text;

        //StartCoroutine(GetGoogleMap());
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Mathf.Round(rect.width);
        mapHeight = (int)Mathf.Round(rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap && (apiKeyLast != apiKey || !Mathf.Approximately(latLast, lat) || !Mathf.Approximately(lonLast, lon) || zoomLast != zoom || mapResolutionLast != mapResolution || mapTypeLast != mapType))
        {
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Mathf.Round(rect.width);
            mapHeight = (int)Mathf.Round(rect.height);
            //StartCoroutine(GetGoogleMap());
            updateMap = false;

            lat = GPS.instance.LastLocation.latitude;
            lon = GPS.instance.LastLocation.longitude;
        }
    }

    IEnumerator GetGoogleMap()
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon + "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + mapResolution + "&maptype=" + mapType + "&key=" + apiKey;
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            mapIsLoading = false;
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            apiKeyLast = apiKey;
            latLast = lat;
            lonLast = lon;
            zoomLast = zoom;
            mapResolutionLast = mapResolution;
            mapTypeLast = mapType;
            updateMap = true;
        }
    }

}
