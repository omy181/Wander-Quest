using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class LocationService : MonoBehaviour
{
	private const string API_KEY = "YOUR_GOOGLE_API_KEY"; // Replace with your Google Maps API key
	private const string BASE_URL = "https://maps.googleapis.com/maps/api/geocode/json?latlng=";

	// This function returns the country name from the GPS coordinates
	public IEnumerator LocationToCountry(GPSLocation loc, System.Action<string> onCountryReceived)
	{
		string url = BASE_URL + loc.latitude + "," + loc.longitude + "&key=" + API_KEY;

		using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.result == UnityWebRequest.Result.Success)
			{
				string jsonResponse = webRequest.downloadHandler.text;
				JObject json = JObject.Parse(jsonResponse);
				string country = GetLocationComponent(json, "country");
				onCountryReceived?.Invoke(country);
			}
			else
			{
				Debug.LogError("Error fetching location: " + webRequest.error);
				onCountryReceived?.Invoke(null); // Return null if there is an error
			}
		}
	}

	// This function returns the city name from the GPS coordinates
	public IEnumerator LocationToCity(GPSLocation loc, System.Action<string> onCityReceived)
	{
		string url = BASE_URL + loc.latitude + "," + loc.longitude + "&key=" + API_KEY;

		using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.result == UnityWebRequest.Result.Success)
			{
				string jsonResponse = webRequest.downloadHandler.text;
				JObject json = JObject.Parse(jsonResponse);
				string city = GetLocationComponent(json, "locality");
				onCityReceived?.Invoke(city);
			}
			else
			{
				Debug.LogError("Error fetching location: " + webRequest.error);
				onCityReceived?.Invoke(null); // Return null if there is an error
			}
		}
	}

	// Helper function to extract the location component (e.g., "country", "locality") from the JSON response
	private string GetLocationComponent(JObject json, string component)
	{
		foreach (var result in json["results"])
		{
			foreach (var addressComponent in result["address_components"])
			{
				if (addressComponent["types"].ToString().Contains(component))
				{
					return addressComponent["long_name"].ToString();
				}
			}
		}
		return null; // Return null if the component is not found
	}
}


