using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlacesAPI : MonoBehaviour
{
    [SerializeField] private MapPinVisualiser _mapPinVisualiser;
    private string _resultsjson;
    void Start()
    {
        //StartCoroutine(SearchPlaces(_getJsonPayload("Migros", GPS.instance.GetLastGPSLocation(), 500f,5)));



        //     FOR TESTING

        _resultsjson = testJsonPlaces;

        // ADD PLACES INTO NEW QUEST
        var quest = QuestManager.instance.CreateNewQuest("Migros List", QuestType.MainQuest, _resultsjson);

        _resultJsonToPlaces(_resultsjson).places.ForEach(place =>
        {
            var qPlace = QuestManager.instance.PlaceToQuestPlace(place);
            QuestManager.instance.AddPlaceToQuest(quest, qPlace);
        });

        _mapPinVisualiser?.ShowPins(_resultJsonToPlaces(_resultsjson)); // SHOW ON MAP
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
        request.SetRequestHeader("X-Goog-FieldMask", "places.id,places.displayName,places.location,places.adrFormatAddress");

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
            //Debug.Log(request.downloadHandler.text);
            _resultsjson = request.downloadHandler.text;



            _resultJsonToPlaces(_resultsjson).places.ForEach((p)=>print(p.id+" - "+p.displayName.text +" - "+ p.location.latitude +","+p.location.longitude + " - "+ AdressUtilities.ConvertHtmlToAddress(p.adrFormatAddress).Locality ));

            _mapPinVisualiser.ShowPins(_resultJsonToPlaces(_resultsjson)); // SHOW ON MAP
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

    private string testJsonPlaces => "{\"places\":[{\"id\":\"ChIJZaj4i_3BuxQRdaO3UjiY_dM\",\"location\":{\"latitude\":38.376557,\"longitude\":26.883778},\"adrFormatAddress\":\"\\u003cspan class=\\\"street-address\\\"\\u003eYalı, 264. Sk. No: 1\\u003c/span\\u003e, \\u003cspan class=\\\"postal-code\\\"\\u003e35310\\u003c/span\\u003e \\u003cspan class=\\\"locality\\\"\\u003eGüzelbahçe\\u003c/span\\u003e/\\u003cspan class=\\\"region\\\"\\u003eİzmir\\u003c/span\\u003e, \\u003cspan class=\\\"country-name\\\"\\u003eTürkiye\\u003c/span\\u003e\",\"displayName\":{\"text\":\"MM Migros\",\"languageCode\":\"tr\"}},{\"id\":\"ChIJa6HuMifDuxQRh8WGe-E3ZYY\",\"location\":{\"latitude\":38.37826,\"longitude\":26.894579000000004},\"adrFormatAddress\":\"\\u003cspan class=\\\"street-address\\\"\\u003eYalı, Mithatpaşa Cd. No: 345/A\\u003c/span\\u003e, \\u003cspan class=\\\"postal-code\\\"\\u003e35310\\u003c/span\\u003e \\u003cspan class=\\\"locality\\\"\\u003eGüzelbahçe\\u003c/span\\u003e/\\u003cspan class=\\\"region\\\"\\u003eİzmir\\u003c/span\\u003e, \\u003cspan class=\\\"country-name\\\"\\u003eTürkiye\\u003c/span\\u003e\",\"displayName\":{\"text\":\"Migros Jet\",\"languageCode\":\"en\"}},{\"id\":\"ChIJ-Rx9czHruxQR8oqWcoERR0c\",\"location\":{\"latitude\":38.3618483,\"longitude\":26.8830262},\"adrFormatAddress\":\"\\u003cspan class=\\\"street-address\\\"\\u003eKahramandere, Şht. Kemal Cd. No:118\\u003c/span\\u003e, \\u003cspan class=\\\"postal-code\\\"\\u003e35310\\u003c/span\\u003e \\u003cspan class=\\\"locality\\\"\\u003eGüzelbahçe\\u003c/span\\u003e/\\u003cspan class=\\\"region\\\"\\u003eİzmir\\u003c/span\\u003e, \\u003cspan class=\\\"country-name\\\"\\u003eTürkiye\\u003c/span\\u003e\",\"displayName\":{\"text\":\"M Migros\",\"languageCode\":\"en\"}},{\"id\":\"ChIJ-ycUbNfruxQRkro76hIr_cs\",\"location\":{\"latitude\":38.361399999999996,\"longitude\":26.889599999999998},\"adrFormatAddress\":\"\\u003cspan class=\\\"street-address\\\"\\u003eÇelebi, İstikbal Cd. No:164\\u003c/span\\u003e, \\u003cspan class=\\\"postal-code\\\"\\u003e35310\\u003c/span\\u003e \\u003cspan class=\\\"locality\\\"\\u003eGüzelbahçe\\u003c/span\\u003e/\\u003cspan class=\\\"region\\\"\\u003eİzmir\\u003c/span\\u003e, \\u003cspan class=\\\"country-name\\\"\\u003eTürkiye\\u003c/span\\u003e\",\"displayName\":{\"text\":\"M Migros\",\"languageCode\":\"tr\"}},{\"id\":\"ChIJ3UC-_B7quxQRhgGI9cSkMto\",\"location\":{\"latitude\":38.3585031,\"longitude\":26.888542599999997},\"adrFormatAddress\":\"\\u003cspan class=\\\"street-address\\\"\\u003eAtatürk, 555. Sk. No: 9\\u003c/span\\u003e, \\u003cspan class=\\\"postal-code\\\"\\u003e35310\\u003c/span\\u003e \\u003cspan class=\\\"locality\\\"\\u003eGüzelbahçe\\u003c/span\\u003e/\\u003cspan class=\\\"region\\\"\\u003eİzmir\\u003c/span\\u003e, \\u003cspan class=\\\"country-name\\\"\\u003eTürkiye\\u003c/span\\u003e\",\"displayName\":{\"text\":\"Migros Jet\",\"languageCode\":\"tr\"}}]}";
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
    public string id;
    public string adrFormatAddress;
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
