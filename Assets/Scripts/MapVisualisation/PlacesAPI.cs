using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlacesAPI : Singleton<PlacesAPI>
{
    [SerializeField] private MapPinVisualiser _mapPinVisualiser;
    private string _resultsjson;

    public IEnumerator StartSearchPlaces(string query, Action<List<QuestPlace>> onPlacesFound)
    {
        yield return StartCoroutine(_searchPlaces(_getJsonPayload(query, GPS.instance.GetLastGPSLocation(), 500f, 5)));

        List<QuestPlace> list = new List<QuestPlace>();
        var placesResult = _resultJsonToPlaces(_resultsjson);
        if (placesResult != null && placesResult.places != null)
        {
            placesResult.places.ForEach(place =>
            {
                list.Add(PlaceToQuestPlace(place));
            });
        }

        onPlacesFound(list);
    }

    private IEnumerator _searchPlaces(string jsonPayload)
    {
        string url = "https://places.googleapis.com/v1/places:searchText";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("X-Goog-Api-Key", API.GetKey());
        request.SetRequestHeader("X-Goog-FieldMask", "places.id,places.displayName,places.location,places.adrFormatAddress");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error: {request.error}\nResponse: {request.downloadHandler?.text}");
            _resultsjson = "";
        }
        else
        {
            _resultsjson = request.downloadHandler.text;
            //Debug.Log("Received response: " + _resultsjson);
        }
    }

    private string _getJsonPayload(string query, GPSLocation location, float radius, int resultCount)
    {
        return $@"{{
            ""textQuery"": ""{EscapeJsonString(query)}"",
            ""pageSize"": {resultCount},
            ""locationBias"": {{
                ""circle"": {{
                    ""center"": {{
                        ""latitude"": {location.latitude.ToString(CultureInfo.InvariantCulture)},
                        ""longitude"": {location.longitude.ToString(CultureInfo.InvariantCulture)}
                    }},
                    ""radius"": {radius.ToString(CultureInfo.InvariantCulture)}
                }}
            }}
        }}";
    }

    private Places _resultJsonToPlaces(string jsonstring)
    {
        if (!string.IsNullOrEmpty(jsonstring))
        {
            try
            {
                return JsonUtility.FromJson<Places>(jsonstring);
            }
            catch (Exception e)
            {
                Debug.LogError($"JSON parsing error: {e.Message}");
                return new Places { places = new List<Place>() };
            }
        }
        Debug.LogError("Result JSON is empty!");
        return new Places { places = new List<Place>() };
    }

    private string EscapeJsonString(string input)
    {
        return input.Replace("\"", "\\\"");
    }

    public QuestPlace PlaceToQuestPlace(Place place)
    {
        return new QuestPlace(
            place.location,
            place.displayName.text,
            place.id,
            AdressUtilities.ConvertHtmlToAddress(place.adrFormatAddress),
            place.isTraveled
        );
    }

    public Place QuestPlaceToPlace(QuestPlace qPlace)
    {
        return new Place
        {
            id = qPlace.ID,
            displayName = new DisplayName { text = qPlace.Name },
            location = qPlace.Location,
            adrFormatAddress = AdressUtilities.ConvertAddressToHtml(qPlace.Address),
            isTraveled = qPlace.IsTraveled
        };
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
