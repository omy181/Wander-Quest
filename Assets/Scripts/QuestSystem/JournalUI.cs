using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private GameObject _placePrefab;
    [SerializeField] private Window _journalWindow;
    [SerializeField] private Transform _content;

    public void ShowQuestsOfType(QuestType questType)
    {
        _clearContents();

        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            if(quest.QuestType == questType)
            {
                var questPrefab = Instantiate(_questPrefab, _content).GetComponent<QuestPrefab>();
                questPrefab.SetQuestData(quest,()=> ListPlaces(quest));
            }
            
        });
    }

    public void ListPlaces(Quest quest)
    {
        _clearContents();
        quest.GetPlaces().ForEach(place => {
            var placePrefab = Instantiate(_placePrefab, _content).GetComponent<PlaceSlotUI>();
            placePrefab.SetPlaceData(place,null);
        });
    }

    private void _clearContents(){
        foreach (Transform child in _content) {
            Destroy(child.gameObject);
        }
    }
    private void _refreshQuests()
    {
        ShowQuestsOfType(QuestType.MainQuest);
    }

    void Awake()
    {     
        _journalWindow.OnWindowActivated += _refreshQuests;

        var q = QuestManager.instance.CreateNewQuest(QuestType.MainQuest,"migros");
        q.AddPlace(new QuestPlace(GPS.instance.GetLastGPSLocation(),"Migros MM","f",new Address(),true));
        q.AddPlace(new QuestPlace(GPS.instance.GetLastGPSLocation(), "Migros M", "3", new Address(), false));
        q.AddPlace(new QuestPlace(GPS.instance.GetLastGPSLocation(), "Migros ", "4", new Address(), true));
    }

    
}
