using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlacesAPI : Singleton<PlacesAPI>
{
    [SerializeField] private MapPinVisualiser _mapPinVisualiser;
    private string _resultsjson;

    public IEnumerator StartSearchPlaces(string querry,Action<List<QuestPlace>> onPlacesFound)
    {
        yield return StartCoroutine(_searchPlaces(_getJsonPayload(querry, GPS.instance.GetLastGPSLocation(), 500f, 5)));

        List<QuestPlace> list = new();
        _resultJsonToPlaces(_resultsjson).places.ForEach(place =>
        {
            list.Add(PlaceToQuestPlace(place));
        });

        onPlacesFound(list);
    }

    private IEnumerator _searchPlaces(string jsonPayload)
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
        request.SetRequestHeader("X-Goog-FieldMask", "places.id,places.displayName,places.location,places.adrFormatAddress");

        // Send the request and wait for a response
        yield return request.SendWebRequest();
        Debug.Log(request.result);
        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            _resultsjson = "";
        }
        else
        {
            //Debug.Log(request.downloadHandler.text);
            _resultsjson = request.downloadHandler.text;

            //_resultJsonToPlaces(_resultsjson).places.ForEach((p)=>print(p.id+" - "+p.displayName.text +" - "+ p.location.latitude +","+p.location.longitude + " - "+ AdressUtilities.ConvertHtmlToAddress(p.adrFormatAddress).Locality ));

        }
    }

    private string _getJsonPayload(string querry,GPSLocation location, float radius,int resultCount)
    {
        return $@"
        {{
            ""textQuery"": ""{querry}"",
            ""pageSize"": {resultCount},
            ""locationBias"": {{
                ""circle"": {{
                    ""center"": {{
                        ""latitude"": {location.latitude},
                        ""longitude"": {location.longitude}
                    }},
                    ""radius"": {radius}
                }}
            }}
        }}";
    }

    private Places _resultJsonToPlaces(string jsonstring)
    {
        if(jsonstring != "")
        {
            return JsonUtility.FromJson<Places>(jsonstring);
        }
        else
        {
            Debug.LogError("Resultjson is empty!");
            var places = new Places();
            places.places = new List<Place>();
            return places;
        }
    }

    public QuestPlace PlaceToQuestPlace(Place place)
    {
        return new QuestPlace(place.location, place.displayName.text, place.id, AdressUtilities.ConvertHtmlToAddress(place.adrFormatAddress),place.isTraveled);
    }

    public Place QuestPlaceToPlace(QuestPlace qPlace)
    {
        var place = new Place();

        place.id = qPlace.ID;
        place.displayName = new DisplayName();
        place.displayName.text = qPlace.Name;
        place.location = qPlace.Location;
        place.adrFormatAddress = AdressUtilities.ConvertAddressToHtml(qPlace.Address);
        place.isTraveled = qPlace.IsTraveled;

        return place;
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
    public GPSLocation location;
    public DisplayName displayName;
    public string id;
    public string adrFormatAddress;
    public bool isTraveled;
}

[Serializable]
public class DisplayName
{
    public string text;
    public string languageCode;
}
