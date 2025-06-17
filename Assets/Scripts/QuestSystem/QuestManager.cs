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
        CleanQuests();

        StartCoroutine(_loadQuests(onQuestsLoadedCallback));
    }

    public void CleanQuests()
    {
        _activeQuests.Clear();
    }

    public Quest CreateNewQuest(QuestType questType, string mapsQuerry)
    {
        Quest quest = new(questType, mapsQuerry, new(),LoginManager.instance.UserName);
        if (IsQuestAvailable(quest)) return quest;
        _activeQuests.Add(quest);

        string questJson = JsonConvert.SerializeObject(quest);
        LoginManager.instance.DbReference.Child("users")
            .Child(LoginManager.instance.UserID)
            .Child("quests")
            .Child(quest.ID)
            .SetRawJsonValueAsync(questJson)
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

		LoginManager.instance.DbReference.Child("users")
			.Child(LoginManager.instance.UserID)
			.Child("quests")
			.Child(quest.ID)
			.RemoveValueAsync()
			.ContinueWith(task =>
			{
				if (task.IsCompleted)
				{
					Debug.Log($"Quest {quest.ID} deleted successfully.");
				}
				else
				{
					Debug.LogError($"Failed to delete quest {quest.ID}: {task.Exception}");
				}
			});
	}

    public bool IsQuestAvailable(Quest quest)
    {
        return _activeQuests.Any(q => q.ID == quest.ID);
    }

    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(_activeQuests);
    }

    public Quest GetMostRecentQuest()
    {
        if(_activeQuests.Count == 0) return null;

        return _activeQuests.OrderByDescending(a=>a.TotalPlaceCount).First();
    }

    public void AddPlaceToQuest(Quest quest,QuestPlace place)
    {
		if (quest.AddPlace(place))
		{
			string updatedQuestJson = JsonConvert.SerializeObject(quest);
			LoginManager.instance.DbReference.Child("users")
				.Child(LoginManager.instance.UserID)
				.Child("quests")
				.Child(quest.ID)
				.SetRawJsonValueAsync(updatedQuestJson)
				.ContinueWith(task =>
				{
					if (task.IsCompleted)
					{                        
						Debug.Log($"Quest {quest.ID} updated successfully with new place.");
					}
					else
					{
						Debug.LogError($"Failed to update quest {quest.ID}: {task.Exception}");
					}
				});
		}
		else
		{
			Debug.LogWarning("Place was not added to the quest (duplicate or invalid).");
		}
	}

    public void UpdatePlaceData(Quest quest,QuestPlace place)
    {
        string updatedQuestJson = JsonConvert.SerializeObject(quest);
        LoginManager.instance.DbReference.Child("users")
            .Child(LoginManager.instance.UserID)
            .Child("quests")
            .Child(quest.ID)
            .SetRawJsonValueAsync(updatedQuestJson)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log($"Quest {quest.ID} updated successfully with new place.");
                }
                else
                {
                    Debug.LogError($"Failed to update quest {quest.ID}: {task.Exception}");
                }
            });
    }

    private IEnumerator _loadQuests(Action onQuestsLoadedCallback)
    {
        var questsData = LoginManager.instance.DbReference.Child("users")
            .Child(LoginManager.instance.UserID)
            .Child("quests").GetValueAsync();
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

    public IEnumerator LoadAQuestOfAllUsers(Quest questToSearch,Action<List<QuestLeaderBoard>> onQuestsLoadedCallback)
    {
        var usersRef = LoginManager.instance.DbReference.Child("users");
        var usersData = usersRef.GetValueAsync();
        yield return new WaitUntil(() => usersData.IsCompleted);

        List<QuestLeaderBoard> leaderboards = new List<QuestLeaderBoard>();

        if (usersData.Result.Exists)
        {
            foreach (var userNode in usersData.Result.Children)
            {
                string userid = userNode.Key;
                var questsNode = userNode.Child("quests");

                foreach (var questNode in questsNode.Children)
                {
                    string json = questNode.GetRawJsonValue();
                    Quest q = JsonConvert.DeserializeObject<Quest>(json);

                    if (q.Title == questToSearch.Title)
                    {
                        leaderboards.Add(new QuestLeaderBoard(q, userid));
                        break; 
                    }
                }
            }
        }

        onQuestsLoadedCallback?.Invoke(leaderboards);
    }

    public void AddDefaultSideQuests()
    {
        _searchAndAddQuest("Migros",QuestType.SideQuest);
        _searchAndAddQuest("Cafe Nero", QuestType.SideQuest);
        _searchAndAddQuest("Pablo", QuestType.SideQuest);

        _searchAndAddQuest("Gym", QuestType.DailyQuest);
        _searchAndAddQuest("Park", QuestType.DailyQuest);
    }

    private void _searchAndAddQuest(string querry,QuestType questType)
    {
        if (IsQuestAvailable(new Quest(questType, querry, new(), "test"))) return;

        StartCoroutine(PlacesAPI.instance.StartSearchPlaces(querry, 10, (List<QuestPlace> places) =>
        {
            FindFirstObjectByType<MapPinVisualiser>().CreatePins(places);
            var quest = CreateNewQuest(questType, querry);

            places.ForEach(p => {
                AddPlaceToQuest(quest, p);
            });
        }));
    }
}