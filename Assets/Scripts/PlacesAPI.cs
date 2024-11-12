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
        //StartCoroutine(SearchPlaces(_getJsonPayload("migros", 38.05508810860761f, 27.023444230965133f, 500f,1)));

        _resultsjson = _testJson;

        FindObjectOfType<MapVisualiser>().ShowPins(_resultJsonToPlaces(_resultsjson));
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
            //Debug.Log(request.downloadHandler.text);
            _resultsjson = request.downloadHandler.text;


            _resultJsonToPlaces(_resultsjson).places.ForEach((p)=>print(p.displayName.text +" - "+ p.location.latitude +","+p.location.longitude));
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

    private string _testJson = @"{
  ""places"": [
    {
      ""location"": {
        ""latitude"": 38.047744699999996,
        ""longitude"": 27.0553645
      },
      ""displayName"": {
        ""text"": ""MM Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.0733037,
        ""longitude"": 27.017593899999998
      },
      ""displayName"": {
        ""text"": ""Migros Jet"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.080371899999996,
        ""longitude"": 26.967442199999997
      },
      ""displayName"": {
        ""text"": ""M Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.0798,
        ""longitude"": 26.9648
      },
      ""displayName"": {
        ""text"": ""Migros Jet"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.076008,
        ""longitude"": 26.936555499999997
      },
      ""displayName"": {
        ""text"": ""M Migros"",
        ""languageCode"": ""en""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.020736,
        ""longitude"": 27.097068999999998
      },
      ""displayName"": {
        ""text"": ""M Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.0147,
        ""longitude"": 27.103399999999997
      },
      ""displayName"": {
        ""text"": ""Migros Jet"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.312611,
        ""longitude"": 27.143269
      },
      ""displayName"": {
        ""text"": ""5M Migros"",
        ""languageCode"": ""en""
      }
    },
    {
      ""location"": {
        ""latitude"": 37.8472893,
        ""longitude"": 27.258984899999998
      },
      ""displayName"": {
        ""text"": ""MMM Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.194679,
        ""longitude"": 26.833592
      },
      ""displayName"": {
        ""text"": ""M Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.402346,
        ""longitude"": 27.117364
      },
      ""displayName"": {
        ""text"": ""MMM Migros"",
        ""languageCode"": ""en""
      }
    },
    {
      ""location"": {
        ""latitude"": 37.808501199999995,
        ""longitude"": 27.277185
      },
      ""displayName"": {
        ""text"": ""5M Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.338522,
        ""longitude"": 27.134728
      },
      ""displayName"": {
        ""text"": ""5M"",
        ""languageCode"": ""en""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.250509199999996,
        ""longitude"": 27.131539300000004
      },
      ""displayName"": {
        ""text"": ""M Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 37.8118942,
        ""longitude"": 27.2818082
      },
      ""displayName"": {
        ""text"": ""5M Migros"",
        ""languageCode"": ""en""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.397170599999995,
        ""longitude"": 27.024866199999998
      },
      ""displayName"": {
        ""text"": ""MMM Migros"",
        ""languageCode"": ""en""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.370253,
        ""longitude"": 27.178812999999998
      },
      ""displayName"": {
        ""text"": ""Migros MMM"",
        ""languageCode"": ""en""
      }
    },
    {
      ""location"": {
        ""latitude"": 37.8663204,
        ""longitude"": 27.2741498
      },
      ""displayName"": {
        ""text"": ""MM Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.193846,
        ""longitude"": 26.7870332
      },
      ""displayName"": {
        ""text"": ""M Migros"",
        ""languageCode"": ""tr""
      }
    },
    {
      ""location"": {
        ""latitude"": 38.440933,
        ""longitude"": 27.146427199999998
      },
      ""displayName"": {
        ""text"": ""MM Migros"",
        ""languageCode"": ""tr""
      }
    }
  ]
}";
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