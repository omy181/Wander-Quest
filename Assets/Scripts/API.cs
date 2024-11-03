using UnityEngine;

public static class API 
{
    public static string GetKey(){
        TextAsset apitext = Resources.Load<TextAsset>("apikey");

        if (apitext == null )
        {
            Debug.LogError("API Key not found");
            return string.Empty;
        }

        return  apitext.text;
    }
}
