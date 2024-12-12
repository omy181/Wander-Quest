using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class MapTilesAPI : Singleton<MapTilesAPI>
{
    private SessionResponse _sessionResponse;

    private Dictionary<GoogleTiles, Texture2D> _cachedTileTextures = new();
    private Dictionary<Renderer, int> _rendererUpToDateness = new();
    private int _rendererCounter = 0;

    public IEnumerator StartMapTiles(Action OnStarted)
    {
        yield return StartCoroutine(_startMapTileSession(_getJsonPayload()));
        OnStarted();
    }

    private IEnumerator _startMapTileSession(string jsonPayload)
    {
        string url = $"https://tile.googleapis.com/v1/createSession?key={API.GetKey()}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            _sessionResponse = null;
        }
        else
        {
            _sessionResponse = JsonUtility.FromJson<SessionResponse>(request.downloadHandler.text);
        }
    }



    public IEnumerator SetTileTexture(GoogleTiles tileData,int orientation,Renderer renderer)
    {
        if (!_rendererUpToDateness.ContainsKey(renderer)) _rendererUpToDateness[renderer] = -1;

        if (_cachedTileTextures.ContainsKey(tileData))
        {
            renderer.material.mainTexture = _cachedTileTextures[tileData];
        }
        else if (!MapUtilities.DoesTileTexturePossible(tileData))
        {

            renderer.material.mainTexture = null;
        }
        else if (_sessionResponse != null)
        {

            string url = $"https://tile.googleapis.com/v1/2dtiles/{tileData.Zoom}/{tileData.X}/{tileData.Y}?session={_sessionResponse.session}&key={API.GetKey()}&orientation={orientation}";

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Tile2D ERROR: " + request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                _cachedTileTextures[tileData] = texture;

                // Apply the texture to the target renderer if available
                if (renderer != null && _rendererUpToDateness[renderer] <= _rendererCounter)
                {
                    renderer.material.mainTexture = texture;
                }
                else
                {
                    Debug.LogWarning("Target Renderer is not assigned.");
                }

                _rendererCounter++;
            }
        }
        else
        {
            Debug.LogError("Invalid Session Token");
        }
    }

    private string _getJsonPayload()
    {
        return @"       
             {
  ""mapType"": ""roadmap"",
  ""language"": ""en-US"",
  ""region"": ""US"",
  ""styles"": [
    {
      ""featureType"": ""administrative.country"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""off"" }
      ]
    },
    {
      ""featureType"": ""administrative.locality"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""off"" }
      ]
    },
    {
      ""featureType"": ""administrative"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""off"" }
      ]
    },
    {
      ""featureType"": ""poi"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""off"" }
      ]
    },
    {
      ""featureType"": ""road"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""off"" }
      ]
    },
    {
      ""featureType"": ""transit"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""off"" }
      ]
    },
    {
      ""featureType"": ""water"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""off"" },
        { ""color"": ""#185abc"" }
      ]
    },
    {
      ""featureType"": ""landscape"",
      ""elementType"": ""labels"",
      ""stylers"": [
        { ""visibility"": ""on"" }
      ]
    }
  ]
}";
    }


}

[Serializable]
public class SessionResponse
{
    public string session;
    public string expiry;
    public int tileWidth;
    public string imageFormat;
    public int tileHeight;
}