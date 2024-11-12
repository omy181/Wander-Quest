using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlacesAPI : MonoBehaviour
{

    void Start()
    {
        string call ="https://example.com";

        StartCoroutine(MakeApiCall(call));
        
    }

    IEnumerator MakeApiCall(string url)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("request ERROR: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
        }
    }
}
