using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private List<Quest> _activeQuests = new();

    public void InitializeQuests()
    {
        ///TODO: on the start of the game this function will pull all active quests from database into the _activeQuests list
    }

    public void CreateNewQuest(string title,QuestType questType,string mapsQuerry)
    {
        Quest quest = new(title, questType, mapsQuerry,new());
        _activeQuests.Add(quest);

        ///TODO: add this quest to the database
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
}
