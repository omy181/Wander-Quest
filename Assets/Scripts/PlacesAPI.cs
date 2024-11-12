using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlacesAPI : MonoBehaviour
{
    private string _resultsjson;
    void Start()
    {
        StartCoroutine(SearchPlaces(_getJsonPayload("migros", 38.45950483818506f, 27.21467914883218f,500f,1)));
        
    }

    IEnumerator SearchPlaces(string jsonPayload)
    {
        // Define the API endpoint
        string url = "https://places.googleapis.com/v1/places:searchText";

        // Create the UnityWebRequest for a POST request
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Set the content type header to application/json
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Set additional headers
        request.SetRequestHeader("X-Goog-Api-Key", API.GetKey());
        request.SetRequestHeader("X-Goog-FieldMask", "places.displayName,places.location");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            _resultsjson = "";
        }
        else
        {
            //Debug.Log("Response: " + request.downloadHandler.text);
            _resultsjson = request.downloadHandler.text;


            _GetSearchResults().places.ForEach((p)=>print(p.displayName.text));
        }
    }

    private string _getJsonPayload(string querry,float latitude, float longitude, float radius,int resultCount)
    {
        return $@"
        {{
            ""textQuery"": ""{querry}"",
            ""pageSize"": {resultCount},
            ""locationBias"": {{
                ""circle"": {{
                    ""center"": {{
                        ""latitude"": {latitude},
                        ""longitude"": {longitude}
                    }},
                    ""radius"": {radius}
                }}
            }}
        }}";
    }

    private Places _GetSearchResults()
    {
        if(_resultsjson != "")
        {
            return JsonUtility.FromJson<Places>(_resultsjson);
        }
        else
        {
            Debug.LogError("Resultjson is empty!");
            var places = new Places();
            places.places = new List<Place>();
            return places;
        }
    }
}

[Serializable]
public class Places
{
    public List<Place> places;
}

[Serializable]
public class Place
{
    public Location location;
    public DisplayName displayName;
}

[Serializable]
public class Location
{
    public float latitude;
    public float longitude;
}

[Serializable]
public class DisplayName
{
    public string text;
    public string languageCode;
}