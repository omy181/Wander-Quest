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


    private void _addPlaceholderQuests(){
        Quest q1 = QuestManager.instance.CreateNewQuest("Migros", QuestType.MainQuest, "aa");
        QuestManager.instance.AddPlaceToQuest(q1, new QuestPlace(GPS.instance.GetLastGPSLocation(), "Migros M","12"));
        QuestManager.instance.AddPlaceToQuest(q1, new QuestPlace(GPS.instance.GetLastGPSLocation(), "The Migros", "11"));
        QuestManager.instance.AddPlaceToQuest(q1, new QuestPlace(GPS.instance.GetLastGPSLocation(), "Denizli Migros", "14"));
        QuestManager.instance.CreateNewQuest("Metro Market", QuestType.MainQuest, "cc");
        QuestManager.instance.CreateNewQuest("Bim", QuestType.DailyQuest, "bb");
        QuestManager.instance.CreateNewQuest("a-101", QuestType.DailyQuest, "dd");  
        QuestManager.instance.CreateNewQuest("Sok", QuestType.SideQuest, "ee");
    }

    private void _refreshQuests()
    {
        ShowQuestsOfType(QuestType.MainQuest);
    }

    void Awake()
    {     
        _addPlaceholderQuests();
        _journalWindow.OnWindowActivated += _refreshQuests;
    }

    
}
