using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private List<Quest> _activeQuests = new();

    public void InitializeQuests()
    {
        ///TODO: on the start of the game this function will pull all active quests from database into the _activeQuests list
    }

    public Quest CreateNewQuest(string title,QuestType questType,string mapsQuerry)
    {
        Quest quest = new(title, questType, mapsQuerry,new());
        if (IsQuestAvailable(quest)) return quest;
        
        _activeQuests.Add(quest);

        ///TODO: add this quest to the database
        return quest;
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

    public QuestPlace PlaceToQuestPlace(Place place)
    {
        return new QuestPlace(new GPSLocation(place.location.latitude, place.location.longitude), place.displayName.text, place.id, AdressUtilities.ConvertHtmlToAddress(place.adrFormatAddress));
    }
}
