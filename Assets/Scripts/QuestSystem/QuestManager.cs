using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private List<Quest> _activeQuests = new();

    public void InitializeQuests(Action onQuestsLoadedCallback)
    {
        StartCoroutine(_loadQuests(onQuestsLoadedCallback));
    }

    public Quest CreateNewQuest(QuestType questType, string mapsQuerry)
    {
        Quest quest = new(questType, mapsQuerry, new());
        if (IsQuestAvailable(quest)) return quest;
        _activeQuests.Add(quest);

        string questJson = JsonConvert.SerializeObject(quest);
        LoginManager.instance.DbReference.Child("users").Child(LoginManager.instance.Username).Child("quests").Child(quest.ID).SetRawJsonValueAsync(questJson)
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
        return quest;
    }

    public void DeleteQuest(Quest quest)
    {       
        _activeQuests.Remove(quest);

        ///TODO: delete this quest on database
    }

    public bool IsQuestAvailable(Quest quest)
    {
        return _activeQuests.Any(q => q.ID == quest.ID);
    }

    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(_activeQuests);
    }

    public void AddPlaceToQuest(Quest quest,QuestPlace place)
    {
        if (quest.AddPlace(place))
        {
            ///TODO: update this quest on the database as well
        }
    }

    private IEnumerator _loadQuests(Action onQuestsLoadedCallback)
    {
        var questsData = LoginManager.instance.DbReference.Child("users").Child(LoginManager.instance.Username).Child("quests").GetValueAsync();
        yield return new WaitUntil(() => questsData.IsCompleted);

        if (questsData.Result.Exists)
        {
            foreach (var child in questsData.Result.Children)
            {
                string questJson = child.GetRawJsonValue();
                Quest quest = JsonConvert.DeserializeObject<Quest>(questJson);
                _activeQuests.Add(quest);
            }
        }
        else
        {
            Debug.Log("No quests found for this user.");
        }

        onQuestsLoadedCallback?.Invoke();
    }
}