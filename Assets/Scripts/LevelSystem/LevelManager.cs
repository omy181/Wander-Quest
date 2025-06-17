using Newtonsoft.Json;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private TMP_Text _levelText;
    private float _currentLevel;
    public float CurrentLevel
    {
        get { return _currentLevel; }
        set {
            _updateLevelAndText(value);
            _updateLevelOnFirebase(_currentLevel);
        }
    }

    private void _updateLevelAndText(float level)
    {
        _currentLevel = level;
        _levelSlider.value = _currentLevel % 1;
        _levelText.text = "LVL" + Mathf.FloorToInt(_currentLevel);
    }
    private void _updateLevelOnFirebase(float level)
    {
        _userData.Level = level;
        var json = JsonConvert.SerializeObject(_userData);

        LoginManager.instance.DbReference.Child("users")
            .Child(LoginManager.instance.UserID)
            .Child("UserData")
            .SetRawJsonValueAsync(json)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Quest added successfully.");
                }
                else
                {
                    Debug.LogError("Failed to add quest.");
                }
            });
    }


    private UserData _userData;
    public void InitializeLevelData()
    {
        StartCoroutine(nameof(_loadUserData));
    }

    private IEnumerator _loadUserData()
    {
        var userData = LoginManager.instance.DbReference.Child("users")
            .Child(LoginManager.instance.UserID)
            .Child("UserData").GetValueAsync();
        yield return new WaitUntil(() => userData.IsCompleted);

        if (userData.Result.Exists)
        {
            string userdataJson = userData.Result.GetRawJsonValue();
            UserData userdata = JsonConvert.DeserializeObject<UserData>(userdataJson);
            _userData = userdata;

            _updateLevelAndText(_userData.Level);
        }
        else
        {
            Debug.Log("No userdata found");
        }

    }
}

public class UserData
{
    [JsonProperty] public float Level;

    public UserData(float level = 0)
    {
        Level = level;
    }
}